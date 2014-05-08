using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysicsDemos;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.GameElement
{
    public class Ship : MobileElement
    {
        //the given orientation of the ship's pitch, in degrees
        public float Pitch
        {
            get;
            set;
        }

        //the given orientation of the ship's yaw, in degrees
        public float Yaw
        {
            get;
            set;
        }

        //the given orientation of the ship's roll, in degrees
        public float Roll
        {
            get;
            set;
        }

        public Ship( Game game, string modelID ) : base( game, modelID )
        {
            //mesh.Tag = "SHIP";
            Pitch = 180;
            Yaw = 90;
            Roll = 90;

            mesh.CollisionInformation.CollisionRules.Group = Resources.Instance.GetGroup( "ship" );
            mesh.Position = new BEPUutilities.Vector3( -8f, -5f, 0f );

            mesh.AngularDamping = 0.85f;
            mesh.LinearDamping = 0.85f;

            mesh.CollisionInformation.Events.InitialCollisionDetected += CollisionDetected;

            mesh.BecomeKinematic();
        }


        /**
         * updates the model's orientation based on the Pitch, Yaw, and Roll properties
         */
        public void Update()
        {
            //the order looks weird, but it's due to the model's orientation. despite it's positioning in Blender, XNA decided to flip it every which way
            mesh.Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll( MathHelper.ToRadians( Roll ), MathHelper.ToRadians( Yaw ), MathHelper.ToRadians( Pitch ) );
            mesh.LinearVelocity *= 0.99f;
        }


        void CollisionDetected( EntityCollidable sender, Collidable other, CollidablePairHandler pair )
        {
            //if( "ASTEROID".Equals( ((string) other.Tag) ) )
            CollisionGroup group = other.CollisionRules.Group;
            if ( group != null )
            {
                if ( group.Equals( Resources.Instance.GetGroup( "asteroid" ) ) )
                {

                }
                else if ( group.Equals( Resources.Instance.GetGroup( "universe" ) ) )
                {
                    mesh.LinearVelocity = mesh.LinearVelocity * -1;
                }
            }
        }
    }
}
