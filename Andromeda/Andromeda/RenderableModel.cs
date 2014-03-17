using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CS437BepuPhysicsDemo
{
    /// <summary>
    /// Base class for things that want to have a model
    /// </summary>
    public class RenderableModel : ITargetable
    {
        protected Model _model;
        protected Matrix _world;

        public virtual Matrix World
        {
            get { return _world; }
        }

        public RenderableModel(PhysDemo game, string model)
        {
            _model = game.Content.Load<Model>(model);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect b in mesh.Effects)
                {
                    b.LightingEnabled = true;
                    b.PreferPerPixelLighting = true;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, Camera camera)
        {
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect b in mesh.Effects)
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
