using BEPUphysics.Entities.Prefabs;
using BEPUphysicsDemos;
using ConversionHelper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities;

namespace Andromeda.GameElement
{
    class UniverseOld : RenderableElement
    {
        private MobileMesh mesh;

        public override Entity Entity
        {
            get
            {
                return mesh;
            }
        }

        public override Matrix World
        {
            get
            {
                return MathConverter.Convert( mesh.WorldTransform );
            }
        }

        public UniverseOld( Game game, string modelID ) : base( game, modelID )
        {
            //mesh = new Sphere( BEPUutilities.Vector3.Zero, 2000f );
            BEPUutilities.Vector3[] vertices;
            int[] indices;
            ModelDataExtractor.GetVerticesAndIndicesFromModel( model, out vertices, out indices );
            mesh = new MobileMesh( vertices, indices, BEPUutilities.AffineTransform.Identity, BEPUphysics.CollisionShapes.MobileMeshSolidity.Solid );
            mesh.CollisionInformation.CollisionRules.Group = Resources.Instance.GetGroup( "universe" );
            //mesh.
            //mesh.BecomeKinematic();
        }
    }
}
