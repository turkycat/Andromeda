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
        //private float mass;

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


        public Asteroid( Game game, string modelID, float mass ) : base( game, modelID )
        {
            //this.mass = mass;
            BEPUutilities.Vector3[] vertices;
            int[] indices;
            ModelDataExtractor.GetVerticesAndIndicesFromModel( model, out vertices, out indices );
            mesh = new MobileMesh( vertices, indices, BEPUutilities.AffineTransform.Identity, BEPUphysics.CollisionShapes.MobileMeshSolidity.Solid, mass );

            mesh.LinearMomentum = new BEPUutilities.Vector3( 0f, 50f, 0f );
            mesh.AngularVelocity = BEPUutilities.Vector3.Zero;
            mesh.LinearDamping = 0f;
            mesh.AngularDamping = 0f;
            mesh.IsAffectedByGravity = false;
            //mesh.Mass = mass;
            //GameState.Instance.AddEntity( mesh );
            //collider = new MobileMesh( _model.Meshes.
            //sphere = new Sphere(MathConverter.Convert( Vector3.Zero ), 0.5f, 8.0f );
        }
    }
}
