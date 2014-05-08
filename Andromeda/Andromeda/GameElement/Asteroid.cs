using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.PositionUpdating;
using ConversionHelper;
using BEPUphysicsDemos;

namespace Andromeda.GameElement
{
    class Asteroid : MobileElement
    {
        private Asteroid( Game game, string modelID, float mass, Vector3 position, Vector3 velocity )
            : base( game, modelID )
        {
            mesh.Tag = "ASTEROID";
            mesh.CollisionInformation.CollisionRules.Group = Resources.Instance.GetGroup( "asteroid" );
            mesh.Position = new BEPUutilities.Vector3( position.X, position.Y, position.Z );
            mesh.LinearMomentum = new BEPUutilities.Vector3( velocity.X, velocity.Y, velocity.Z );
            //mesh.AngularVelocity = new BEPUutilities.Vector3( Resources.Instance.Random.Next( 50 ), Resources.Instance.Random.Next( 50 ), Resources.Instance.Random.Next( 50 ) );
            mesh.AngularVelocity = new BEPUutilities.Vector3( (float) Resources.Instance.Random.NextDouble(), (float) Resources.Instance.Random.NextDouble(), (float) Resources.Instance.Random.NextDouble() );
            mesh.LinearDamping = 0f;
            mesh.AngularDamping = 0f;
        }


        public static Asteroid GenerateAsteroid( Game game, int size, Vector3 position, Vector3 velocity )
        {
            int rnd = Resources.Instance.Random.Next( 2 );

            //size is 1, 2, or 3
            switch ( size )
            {
                case 1:
                    if ( rnd == 0 )
                    {
                        return new Asteroid( game, "asteroid_large1", 360f, position, velocity );
                    }

                    return new Asteroid( game, "asteroid_large2", 360f, position, velocity );

                case 2:

                    if ( rnd == 0 )
                    {
                        return new Asteroid( game, "asteroid_medium1", 120f, position, velocity );
                    }

                    return new Asteroid( game, "asteroid_medium2", 120f, position, velocity );

                default:

                    if ( rnd == 0 )
                    {
                        return new Asteroid( game, "asteroid_small1", 40f, position, velocity );
                    }

                    return new Asteroid( game, "asteroid_small2", 40f, position, velocity );
            }
        }


        public static Asteroid GenerateAsteroid( Game game, int size )
        {
            return GenerateAsteroid( game, size, new Vector3( Resources.Instance.Random.Next( 300 ) - 150, Resources.Instance.Random.Next( 300 ) - 150, Resources.Instance.Random.Next( 300 ) - 150 ),
                new Vector3( Resources.Instance.Random.Next( 3000 ), Resources.Instance.Random.Next( 3000 ), Resources.Instance.Random.Next( 3000 ) ) );
        }
    }
}
