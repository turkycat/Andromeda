﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.UpdateableSystems;
using BEPUphysics;
using BEPUutilities;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Materials;
using BEPUphysics.PositionUpdating;
using System.Threading;

namespace BEPUphysicsDemos.AlternateMovement.SphereCharacter
{
    /// <summary>
    /// Gives a physical object simple and cheap FPS-like control.
    /// This character has less features than the full CharacterController but is a little bit faster.
    /// </summary>
    public class SphereCharacterController : Updateable, IBeforeSolverUpdateable
    {
        /// <summary>
        /// Gets the physical body of the character.
        /// </summary>
        public Sphere Body { get; private set; }

        /// <summary>
        /// Gets the support system which other systems use to perform local ray casts and contact queries.
        /// </summary>
        public QueryManager QueryManager { get; private set; }

        /// <summary>
        /// Gets the constraint used by the character to handle horizontal motion.  This includes acceleration due to player input and deceleration when the relative velocity
        /// between the support and the character exceeds specified maximums.
        /// </summary>
        public HorizontalMotionConstraint HorizontalMotionConstraint { get; private set; }

        /// <summary>
        /// Gets the constraint used by the character to stay glued to surfaces it stands on.
        /// </summary>
        public VerticalMotionConstraint VerticalMotionConstraint { get; private set; }

        private Vector3 down = new Vector3(0, -1, 0);
        /// <summary>
        /// Gets or sets the down direction of the character. Controls the interpretation of movement and support finding.
        /// </summary>
        public Vector3 Down
        {
            get
            {
                return down;
            }
            set
            {
                float lengthSquared = value.LengthSquared();
                if (lengthSquared < Toolbox.Epsilon)
                    return; //Silently fail. Assuming here that a dynamic process is setting this property; don't need to make a stink about it.
                Vector3.Divide(ref value, (float)Math.Sqrt(lengthSquared), out value);
                down = value;
                UpdateHorizontalViewDirection();
            }
        }

        Vector3 viewDirection = new Vector3(0, 0, -1);
        Vector3 horizontalViewDirection = new Vector3(0, 0, -1);

        /// <summary>
        /// Gets the horizontal view direction computed using the Down vector and the ViewDirection.
        /// </summary>
        public Vector3 HorizontalViewDirection
        {
            get
            {
                return horizontalViewDirection;
            }
        }

        /// <summary>
        /// Gets the axis along which the character can strafe.
        /// </summary>
        public Vector3 StrafeDirection
        {
            get
            {
                return Vector3.Cross(Down, horizontalViewDirection);
            }
        }

        /// <summary>
        /// Gets or sets the view direction associated with the character.
        /// Also sets the horizontal view direction internally based on the current down vector.
        /// This is used to interpret the movement directions.
        /// </summary>
        public Vector3 ViewDirection
        {
            get
            {
                return viewDirection;
            }
            set
            {
                viewDirection = value;
                UpdateHorizontalViewDirection();
            }
        }

        void UpdateHorizontalViewDirection()
        {
            float dot = Vector3.Dot(viewDirection, Down);
            Vector3 toRemove = Down * dot;
            Vector3.Subtract(ref viewDirection, ref toRemove, out horizontalViewDirection);
            float length = horizontalViewDirection.LengthSquared();
            if (length > 0)
            {
                Vector3.Divide(ref horizontalViewDirection, (float)Math.Sqrt(length), out horizontalViewDirection);
            }
            else
                horizontalViewDirection = new Vector3();
        }

        private float jumpSpeed = 4.5f;
        /// <summary>
        /// Gets or sets the speed at which the character leaves the ground when it jumps.
        /// </summary>
        public float JumpSpeed
        {
            get
            {
                return jumpSpeed;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Value must be nonnegative.");
                jumpSpeed = value;
            }
        }
        float slidingJumpSpeed = 3;
        /// <summary>
        /// Gets or sets the speed at which the character leaves the ground when it jumps without traction.
        /// </summary>
        public float SlidingJumpSpeed
        {
            get
            {
                return slidingJumpSpeed;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Value must be nonnegative.");
                slidingJumpSpeed = value;
            }
        }
        float jumpForceFactor = 1f;
        /// <summary>
        /// Gets or sets the amount of force to apply to supporting dynamic entities as a fraction of the force used to reach the jump speed.
        /// </summary>
        public float JumpForceFactor
        {
            get
            {
                return jumpForceFactor;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Value must be nonnegative.");
                jumpForceFactor = value;
            }
        }


        /// <summary>
        /// Gets the support finder used by the character.
        /// The support finder analyzes the character's contacts to see if any of them provide support and/or traction.
        /// </summary>
        public SupportFinder SupportFinder { get; private set; }


        /// <summary>
        /// Constructs a new character controller with the default configuration.
        /// </summary>
        public SphereCharacterController()
            : this(new Vector3(), 1, 10)
        {

        }

        /// <summary>
        /// Constructs a new character controller with the most common configuration options.
        /// </summary>
        /// <param name="position">Initial position of the character.</param>
        /// <param name="radius">Radius of the character body.</param>
        /// <param name="mass">Mass of the character body.</param>
        public SphereCharacterController(Vector3 position, float radius, float mass)
        {
            Body = new Sphere(position, radius, mass);
            Body.IgnoreShapeChanges = true; //Wouldn't want inertia tensor recomputations to occur if the shape changes.
            //Making the character a continuous object prevents it from flying through walls which would be pretty jarring from a player's perspective.
            Body.PositionUpdateMode = PositionUpdateMode.Continuous;
            Body.LocalInertiaTensorInverse = new Matrix3x3();
            //TODO: In v0.16.2, compound bodies would override the material properties that get set in the CreatingPair event handler.
            //In a future version where this is changed, change this to conceptually minimally required CreatingPair.
            Body.CollisionInformation.Events.DetectingInitialCollision += RemoveFriction;
            Body.LinearDamping = 0;
            SupportFinder = new SupportFinder(this);
            HorizontalMotionConstraint = new HorizontalMotionConstraint(this);
            VerticalMotionConstraint = new VerticalMotionConstraint(this);
            QueryManager = new QueryManager(this);

            //Enable multithreading for the sphere characters.  
            //See the bottom of the Update method for more information about using multithreading with this character.
            IsUpdatedSequentially = false;

            //Link the character body to the character controller so that it can be identified by the locker.
            //Any object which replaces this must implement the ICharacterTag for locking to work properly.
            Body.CollisionInformation.Tag = new CharacterSynchronizer(Body);



        }

        List<ICharacterTag> involvedCharacters = new List<ICharacterTag>();
        public void LockCharacterPairs()
        {
            //If this character is colliding with another character, there's a significant danger of the characters
            //changing the same collision pair handlers.  Rather than infect every character system with micro-locks,
            //we lock the entirety of a character update.

            foreach (var pair in Body.CollisionInformation.Pairs)
            {
                //Is this a pair with another character?
                var other = pair.BroadPhaseOverlap.EntryA == Body.CollisionInformation ? pair.BroadPhaseOverlap.EntryB : pair.BroadPhaseOverlap.EntryA;
                var otherCharacter = other.Tag as ICharacterTag;
                if (otherCharacter != null)
                {
                    involvedCharacters.Add(otherCharacter);
                }
            }
            if (involvedCharacters.Count > 0)
            {
                //If there were any other characters, we also need to lock ourselves!
                involvedCharacters.Add((ICharacterTag)Body.CollisionInformation.Tag);

                //However, the characters cannot be locked willy-nilly.  There needs to be some defined order in which pairs are locked to avoid deadlocking.
                involvedCharacters.Sort(comparer);

                for (int i = 0; i < involvedCharacters.Count; ++i)
                {
                    Monitor.Enter(involvedCharacters[i]);
                }
            }
        }

        private static Comparer comparer = new Comparer();
        class Comparer : IComparer<ICharacterTag>
        {
            public int Compare(ICharacterTag x, ICharacterTag y)
            {
                if (x.InstanceId < y.InstanceId)
                    return -1;
                if (x.InstanceId > y.InstanceId)
                    return 1;
                return 0;
            }
        }

        public void UnlockCharacterPairs()
        {
            //Unlock the pairs, LIFO.
            for (int i = involvedCharacters.Count - 1; i >= 0; i--)
            {
                Monitor.Exit(involvedCharacters[i]);
            }
            involvedCharacters.Clear();
        }


        void RemoveFriction(EntityCollidable sender, BroadPhaseEntry other, NarrowPhasePair pair)
        {
            var collidablePair = pair as CollidablePairHandler;
            if (collidablePair != null)
            {
                //The default values for InteractionProperties is all zeroes- zero friction, zero bounciness.
                //That's exactly how we want the character to behave when hitting objects.
                collidablePair.UpdateMaterialProperties(new InteractionProperties());
            }
        }

        void ExpandBoundingBox()
        {
            if (Body.ActivityInformation.IsActive)
            {
                //This runs after the bounding box updater is run, but before the broad phase.
                //The expansion allows the downward pointing raycast to collect hit points.
                Vector3 expansion = SupportFinder.MaximumAssistedDownStepHeight * down;
                BoundingBox box = Body.CollisionInformation.BoundingBox;
                if (down.X < 0)
                    box.Min.X += expansion.X;
                else
                    box.Max.X += expansion.X;
                if (down.Y < 0)
                    box.Min.Y += expansion.Y;
                else
                    box.Max.Y += expansion.Y;
                if (down.Z < 0)
                    box.Min.Z += expansion.Z;
                else
                    box.Max.Z += expansion.Z;
                Body.CollisionInformation.BoundingBox = box;
            }


        }


        void CollectSupportData()
        {
            //Identify supports.
            SupportFinder.UpdateSupports();

            //Collect the support data from the support, if any.
            if (SupportFinder.HasSupport)
            {
                if (SupportFinder.HasTraction)
                    supportData = SupportFinder.TractionData.Value;
                else
                    supportData = SupportFinder.SupportData.Value;
            }
            else
                supportData = new SupportData();
        }

        SupportData supportData;

        void IBeforeSolverUpdateable.Update(float dt)
        {
            //Someone may want to use the Body.CollisionInformation.Tag for their own purposes.
            //That could screw up the locking mechanism above and would be tricky to track down.
            //Consider using the making the custom tag implement ICharacterTag, modifying LockCharacterPairs to analyze the different Tag type, or using the Entity.Tag for the custom data instead.
            Debug.Assert(Body.CollisionInformation.Tag is ICharacterTag, "The character.Body.CollisionInformation.Tag must implement ICharacterTag to link the SphereCharacterController and its body together for character-related locking to work in multithreaded simulations.");

            //We can't let multiple characters manage the same pairs simultaneously.  Lock it up!
            LockCharacterPairs();
            try
            {
                bool hadSupport = SupportFinder.HasSupport;

                CollectSupportData();

                //Compute the initial velocities relative to the support.
                Vector3 relativeVelocity;
                ComputeRelativeVelocity(ref supportData, out relativeVelocity);
                float verticalVelocity = Vector3.Dot(supportData.Normal, relativeVelocity);



                //Don't attempt to use an object as support if we are flying away from it (and we were never standing on it to begin with).
                if (SupportFinder.HasSupport && !hadSupport && verticalVelocity < 0)
                {
                    SupportFinder.ClearSupportData();
                    supportData = new SupportData();
                }



                //Attempt to jump.
                if (tryToJump) //Jumping while crouching would be a bit silly.
                {
                    //In the following, note that the jumping velocity changes are computed such that the separating velocity is specifically achieved,
                    //rather than just adding some speed along an arbitrary direction.  This avoids some cases where the character could otherwise increase
                    //the jump speed, which may not be desired.
                    if (SupportFinder.HasTraction)
                    {
                        //The character has traction, so jump straight up.
                        float currentDownVelocity;
                        Vector3.Dot(ref down, ref relativeVelocity, out currentDownVelocity);
                        //Target velocity is JumpSpeed.
                        float velocityChange = Math.Max(jumpSpeed + currentDownVelocity, 0);
                        ApplyJumpVelocity(ref supportData, down * -velocityChange, ref relativeVelocity);


                        //Prevent any old contacts from hanging around and coming back with a negative depth.
                        foreach (var pair in Body.CollisionInformation.Pairs)
                            pair.ClearContacts();
                        SupportFinder.ClearSupportData();
                        supportData = new SupportData();
                    }
                    else if (SupportFinder.HasSupport)
                    {
                        //The character does not have traction, so jump along the surface normal instead.
                        float currentNormalVelocity = Vector3.Dot(supportData.Normal, relativeVelocity);
                        //Target velocity is JumpSpeed.
                        float velocityChange = Math.Max(slidingJumpSpeed - currentNormalVelocity, 0);
                        ApplyJumpVelocity(ref supportData, supportData.Normal * -velocityChange, ref relativeVelocity);

                        //Prevent any old contacts from hanging around and coming back with a negative depth.
                        foreach (var pair in Body.CollisionInformation.Pairs)
                            pair.ClearContacts();
                        SupportFinder.ClearSupportData();
                        supportData = new SupportData();
                    }
                }
                tryToJump = false;
            }
            finally
            {
                UnlockCharacterPairs();
            }


            //if (SupportFinder.HasTraction && SupportFinder.Supports.Count == 0)
            //{
            //    //There's another way to step down that is a lot cheaper, but less robust.
            //    //This modifies the velocity of the character to make it fall faster.
            //    //Impacts with the ground will be harder, so it will apply superfluous force to supports.
            //    //Additionally, it will not be consistent with instant up-stepping.
            //    //However, because it does not do any expensive queries, it is very fast!

            //    //We are being supported by a ray cast, but we're floating.
            //    //Let's try to get to the ground faster.
            //    //How fast?  Try picking an arbitrary velocity and setting our relative vertical velocity to that value.
            //    //Don't go farther than the maximum distance, though.
            //    float maxVelocity = (SupportFinder.SupportRayData.Value.HitData.T - SupportFinder.RayLengthToBottom);
            //    if (maxVelocity > 0)
            //    {
            //        maxVelocity = (maxVelocity + .01f) / dt;

            //        float targetVerticalVelocity = -3;
            //        verticalVelocity = Vector3.Dot(Body.OrientationMatrix.Up, relativeVelocity);
            //        float change = MathHelper.Clamp(targetVerticalVelocity - verticalVelocity, -maxVelocity, 0);
            //        ChangeVelocityUnilaterally(Body.OrientationMatrix.Up * change, ref relativeVelocity);
            //    }
            //}





            //Vertical support data is different because it has the capacity to stop the character from moving unless
            //contacts are pruned appropriately.
            SupportData verticalSupportData;
            Vector3 movement3d;
            HorizontalMotionConstraint.GetMovementDirectionIn3D(out movement3d);
            SupportFinder.GetTractionInDirection(ref movement3d, out verticalSupportData);



            HorizontalMotionConstraint.SupportData = supportData;
            VerticalMotionConstraint.SupportData = verticalSupportData;







        }


        void TeleportToPosition(Vector3 newPosition, float dt)
        {
            Body.Position = newPosition;
            var orientation = Body.Orientation;
            //The re-do of contacts won't do anything unless we update the collidable's world transform.
            Body.CollisionInformation.UpdateWorldTransform(ref newPosition, ref orientation);
            //Refresh all the narrow phase collisions.
            foreach (var pair in Body.CollisionInformation.Pairs)
            {
                //Clear out the old contacts.  This prevents contacts in persistent manifolds from surviving the step
                //Such old contacts might still have old normals which blocked the character's forward motion.
                pair.ClearContacts();
                pair.UpdateCollision(dt);
            }
            //Also re-collect supports.
            //This will ensure the constraint and other velocity affectors have the most recent information available.
            CollectSupportData();
        }

        void ComputeRelativeVelocity(ref SupportData supportData, out Vector3 relativeVelocity)
        {

            //Compute the relative velocity between the body and its support, if any.
            //The relative velocity will be updated as impulses are applied.
            relativeVelocity = Body.LinearVelocity;
            if (SupportFinder.HasSupport)
            {
                //Only entities have velocity.
                var entityCollidable = supportData.SupportObject as EntityCollidable;
                if (entityCollidable != null)
                {
                    //It's possible for the support's velocity to change due to another character jumping if the support is dynamic.
                    //Don't let that happen while the character is computing a relative velocity!
                    Vector3 entityVelocity;
                    bool locked;
                    if (locked = entityCollidable.Entity.IsDynamic)
                        entityCollidable.Entity.Locker.Enter();
                    try
                    {
                        entityVelocity = Toolbox.GetVelocityOfPoint(supportData.Position, entityCollidable.Entity.Position, entityCollidable.Entity.LinearVelocity, entityCollidable.Entity.AngularVelocity);
                    }
                    finally
                    {
                        if (locked)
                            entityCollidable.Entity.Locker.Exit();
                    }
                    Vector3.Subtract(ref relativeVelocity, ref entityVelocity, out relativeVelocity);
                }
            }

        }

        /// <summary>
        /// Changes the relative velocity between the character and its support.
        /// </summary>
        /// <param name="supportData">Support data to use to jump.</param>
        /// <param name="velocityChange">Change to apply to the character and support relative velocity.</param>
        /// <param name="relativeVelocity">Relative velocity to update.</param>
        void ApplyJumpVelocity(ref SupportData supportData, Vector3 velocityChange, ref Vector3 relativeVelocity)
        {
            Body.LinearVelocity += velocityChange;
            var entityCollidable = supportData.SupportObject as EntityCollidable;
            if (entityCollidable != null)
            {
                if (entityCollidable.Entity.IsDynamic)
                {
                    Vector3 change = velocityChange * jumpForceFactor;
                    //Multiple characters cannot attempt to modify another entity's velocity at the same time.
                    entityCollidable.Entity.Locker.Enter();
                    try
                    {
                        entityCollidable.Entity.LinearMomentum += change * -Body.Mass;
                    }
                    finally
                    {
                        entityCollidable.Entity.Locker.Exit();
                    }
                    velocityChange += change;
                }
            }

            //Update the relative velocity as well.  It's a ref parameter, so this update will be reflected in the calling scope.
            Vector3.Add(ref relativeVelocity, ref velocityChange, out relativeVelocity);

        }

        /// <summary>
        /// In some cases, an applied velocity should only modify the character.
        /// This allows partially non-physical behaviors, like gluing the character to the ground.
        /// </summary>
        /// <param name="velocityChange">Change to apply to the character.</param>
        /// <param name="relativeVelocity">Relative velocity to update.</param>
        void ChangeVelocityUnilaterally(Vector3 velocityChange, ref Vector3 relativeVelocity)
        {
            Body.LinearVelocity += velocityChange;
            //Update the relative velocity as well.  It's a ref parameter, so this update will be reflected in the calling scope.
            Vector3.Add(ref relativeVelocity, ref velocityChange, out relativeVelocity);

        }






        bool tryToJump;
        /// <summary>
        /// Jumps the character off of whatever it's currently standing on.  If it has traction, it will go straight up.
        /// If it doesn't have traction, but is still supported by something, it will jump in the direction of the surface normal.
        /// </summary>
        public void Jump()
        {
            //The actual jump velocities are applied next frame.  This ensures that gravity doesn't pre-emptively slow the jump, and uses more
            //up-to-date support data.
            tryToJump = true;
        }

        public override void OnAdditionToSpace(Space newSpace)
        {
            //Add any supplements to the space too.
            newSpace.Add(Body);
            newSpace.Add(HorizontalMotionConstraint);
            newSpace.Add(VerticalMotionConstraint);
            //This character controller requires the standard implementation of Space.
            newSpace.BoundingBoxUpdater.Finishing += ExpandBoundingBox;

            Body.AngularVelocity = new Vector3();
            Body.LinearVelocity = new Vector3();
        }
        public override void OnRemovalFromSpace(Space oldSpace)
        {
            //Remove any supplements from the space too.
            oldSpace.Remove(Body);
            oldSpace.Remove(HorizontalMotionConstraint);
            oldSpace.Remove(VerticalMotionConstraint);
            //This character controller requires the standard implementation of Space.
            oldSpace.BoundingBoxUpdater.Finishing -= ExpandBoundingBox;
            SupportFinder.ClearSupportData();
            Body.AngularVelocity = new Vector3();
            Body.LinearVelocity = new Vector3();
        }


    }
}

