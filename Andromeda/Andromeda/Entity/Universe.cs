using BEPUphysics.Entities.Prefabs;
using BEPUphysicsDemos;
using ConversionHelper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysicsDemos;
using BEPUphysics.BroadPhaseEntries;

namespace Andromeda.Entity
{
    class Universe : RenderableModel
    {
        private Sphere mesh;
        //private StaticMesh mesh;

        public override Matrix World
        {
            get
            {
                return Matrix.CreateScale( new Vector3( 100f, 100f, 100f ) ) *( MathConverter.Convert( mesh.WorldTransform ) );
            }
        }


        public override BEPUphysics.Entities.Entity Entity
        {
            get
            {
                //return new Sphere( new BEPUutilities.Vector3( 0f, 0f, 0f ), 1000f );
                return mesh;
            }
        }

        public Universe( Game game, string modelID ) : base( game, modelID )
        {
            BEPUutilities.Vector3[] vertices;
            int[] indices;
            ModelDataExtractor.GetVerticesAndIndicesFromModel( model, out vertices, out indices );
            mesh = new Sphere( BEPUutilities.Vector3.Zero, 1000f );
            //mesh = new StaticMesh( vertices, indices );
            //mesh.LinearVelocity = BEPUutilities.Vector3.Zero;
            //mesh.LinearMomentum = BEPUutilities.Vector3.Zero;
            mesh.BecomeKinematic();
            //mesh = new MobileMesh( vertices, indices, BEPUutilities.AffineTransform.Identity, BEPUphysics.CollisionShapes.MobileMeshSolidity.Solid );
        }

        public override void Draw( GameTime gameTime, GregsCameraClass.Camera camera )
        {
            //do not draw the universe!
            base.Draw( gameTime, camera );
        }
    }
}
