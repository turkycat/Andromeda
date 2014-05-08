using GregsCameraClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/**
 * based on a class by the same name, authored by Greg Hanes
 *  - class modified, tweaked, and customized by Jesse Frush
 */

namespace Andromeda.GameElement
{

    public abstract class RenderableElement : ElementBase
    {
        protected Model model;
        protected Matrix world;

        public override Matrix World
        {
            get { return world; }
        }


        public bool IsCollidable
        {
            get
            {
                return Entity != null;
            }
        }


        public RenderableElement( Game game, string modelID )
        {
            this.model = Resources.Instance.GetModel( modelID );
            //this.model = game.Content.Load<Model>( model );

            foreach ( ModelMesh mesh in this.model.Meshes )
            {
                foreach ( BasicEffect b in mesh.Effects )
                {
                    b.LightingEnabled = true;
                    b.PreferPerPixelLighting = true;
                    b.EnableDefaultLighting();
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
