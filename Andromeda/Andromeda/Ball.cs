using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.PositionUpdating;
using ConversionHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;

namespace CS437BepuPhysicsDemo
{
    public class Ball : RenderableModel
    {
        SoundEffect _bounceSound;
        private Sphere _sphere;

        public override Matrix World
        {
            get
            {
                return MathConverter.Convert(_sphere.WorldTransform);
            }
        }

        public Ball(PhysDemo game, string model)
            : base(game, model)
        {
            _sphere = new Sphere(2 * MathConverter.Convert(Vector3.Up), 0.5f, 8.0f);

            _bounceSound = game.Content.Load<SoundEffect>("smack");

            _sphere.CollisionInformation.Events.InitialCollisionDetected += Events_InitialCollisionDetected;
            _sphere.PositionUpdateMode = PositionUpdateMode.Continuous;
            (game.Services.GetService(typeof(Space)) as Space).Add(_sphere);
        }

        void Events_InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            if ("Environment".Equals(other.Tag))
            {
                _bounceSound.Play();
            }
        }

        public void Update(GameTime gameTime)
        {
            BEPUutilities.Vector3 impulse = BEPUutilities.Vector3.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                impulse += -BEPUutilities.Vector3.Forward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                impulse += -BEPUutilities.Vector3.Backward;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                impulse += -BEPUutilities.Vector3.Left;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                impulse += -BEPUutilities.Vector3.Right;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (_sphere.CollisionInformation.Pairs.Count != 0)
                {
                    _sphere.LinearVelocity += BEPUutilities.Vector3.Up * 10;
                }
            }

            _sphere.LinearMomentum += 3 * impulse;
        }
    }
}
