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

namespace Andromeda.Entity
{
    class Asteroid : RenderableModel
    {
        private MobileMesh mesh;

        public override Matrix World
        {
            get
            {
                return MathConverter.Convert( mesh.WorldTransform );
            }
        }


        public override BEPUphysics.Entities.Entity Entity
        {
            get
            {
                return mesh;
            }
        }


        public Asteroid( Game game ) : base( game, @"Models/asteroid_large1" )
        {
            BEPUutilities.Vector3[] vertices;
            int[] indices;
            ModelDataExtractor.GetVerticesAndIndicesFromModel( model, out vertices, out indices );
            mesh = new MobileMesh( vertices, indices, BEPUutilities.AffineTransform.Identity, BEPUphysics.CollisionShapes.MobileMeshSolidity.Solid );
            mesh.AngularVelocity = new BEPUutilities.Vector3( .25f, .6f, .1f );
            //GameState.Instance.AddEntity( mesh );
            //collider = new MobileMesh( _model.Meshes.
            //sphere = new Sphere(MathConverter.Convert( Vector3.Zero ), 0.5f, 8.0f );
        }
    }
}
