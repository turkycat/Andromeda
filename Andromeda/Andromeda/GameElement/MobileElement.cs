using BEPUphysics.Entities.Prefabs;
using BEPUphysicsDemos;
using ConversionHelper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.GameElement
{
    public class MobileElement : RenderableElement
    {
        protected MobileMesh mesh;

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

        public MobileElement( Game game, string modelID )
            : base( game, modelID )
        {
            BEPUutilities.Vector3[] vertices;
            int[] indices;
            ModelDataExtractor.GetVerticesAndIndicesFromModel( model, out vertices, out indices );
            mesh = new MobileMesh( vertices, indices, BEPUutilities.AffineTransform.Identity, BEPUphysics.CollisionShapes.MobileMeshSolidity.Solid, 200f );
            mesh.IsAffectedByGravity = false;
        }
    }
}
