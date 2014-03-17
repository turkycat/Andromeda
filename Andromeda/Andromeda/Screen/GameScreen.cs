using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andromeda.Entity;
using GregsCameraClass;

namespace Andromeda.Screen
{
    class GameScreen : Screen
    {
        private ITargetable activeTarget;

        public GameScreen( Game game ) : base( game )
        {
            Asteroid asteroid = new Asteroid( this.Game );
            activeTarget = asteroid;
            models.Add( asteroid );
        }


        /**
         * returns the active target as the focus point of this screen
         */
        public override ITargetable GetCameraFocus()
        {
            return activeTarget;
        }


        public override void Update( GameTime gameTime )
        {
            //do nothing (for now)
        }


        /**
         * called by the screen manager when this is the active screen
         */
        public override void Draw( GameTime gameTime, Camera camera )
        {
            this.Game.GraphicsDevice.Clear( Color.CornflowerBlue );

            foreach ( RenderableModel model in models )
                model.Draw( gameTime, camera );
        }
    }
}
