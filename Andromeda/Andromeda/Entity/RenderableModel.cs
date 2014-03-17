using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/**
 * based on a class by the same name, authored by Greg Hanes
 *  - class modified, tweaked, and customized by Jesse Frush
 */

namespace Andromeda.Entity
{
    using GregsCameraClass;

    public abstract class RenderableModel : ITargetable
    {
        protected Model model;
        protected Matrix world;

        public virtual Matrix World
        {
            get { return world; }
        }


        public virtual BEPUphysics.Entities.Entity Entity
        {
            get;
            protected set;
        }


        public RenderableModel( Game game, string model )
        {
            this.model = game.Content.Load<Model>( model );

            foreach ( ModelMesh mesh in this.model.Meshes )
            {
                foreach ( BasicEffect b in mesh.Effects )
                {
                    b.LightingEnabled = true;
                    b.PreferPerPixelLighting = true;
                }
            }
        }

        public virtual void Draw( GameTime gameTime, Camera camera )
        {
            foreach ( ModelMesh mesh in this.model.Meshes )
            {
                foreach ( BasicEffect b in mesh.Effects )
                {
                    b.World = this.World;
                    b.View = camera.View;
                    b.Projection = camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
