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
    class GameScreen : ScreenBase
    {

        public GameScreen( Game game ) : base( game )
        {
            //Universe universe = new Universe( game, "universe" );
            //models.Add( universe );

            Asteroid asteroid = new Asteroid( this.Game, "asteroid_large1", 360f );
            asteroid.Entity.Position = new BEPUutilities.Vector3( asteroid.Entity.Position.X + 12f, asteroid.Entity.Position.Y, asteroid.Entity.Position.Z );
            asteroid.Entity.LinearVelocity = new BEPUutilities.Vector3( -10f, 0f, 0f );
            models.Add( asteroid );

            asteroid = new Asteroid( this.Game, "asteroid_large2", 360f );
            asteroid.Entity.Position = new BEPUutilities.Vector3( asteroid.Entity.Position.X, asteroid.Entity.Position.Y + 100f, asteroid.Entity.Position.Z );
            asteroid.Entity.LinearVelocity = new BEPUutilities.Vector3( 0f, -20f, 0f );
            models.Add( asteroid );

            asteroid = new Asteroid( this.Game, "asteroid_medium1", 120f );
            asteroid.Entity.LinearVelocity = new BEPUutilities.Vector3( 10f, 0f, 0f );
            models.Add( asteroid );

            asteroid = new Asteroid( this.Game, "asteroid_medium2", 120f );
            asteroid.Entity.Position = new BEPUutilities.Vector3( asteroid.Entity.Position.X, asteroid.Entity.Position.Y, asteroid.Entity.Position.Z - 20f );
            asteroid.Entity.LinearVelocity = new BEPUutilities.Vector3( 0f, 0f, 20f );
            models.Add( asteroid );

            asteroid = new Asteroid( this.Game, "asteroid_small1", 40f );
            asteroid.Entity.LinearVelocity = new BEPUutilities.Vector3( 40f, 0f, 0f );
            asteroid.Entity.Position = new BEPUutilities.Vector3( asteroid.Entity.Position.X - 10f, asteroid.Entity.Position.Y, asteroid.Entity.Position.Z );
            models.Add( asteroid );

            asteroid = new Asteroid( this.Game, "asteroid_small2", 40f );
            asteroid.Entity.LinearVelocity = new BEPUutilities.Vector3( 20f, 0f, 0f );
            asteroid.Entity.Position = new BEPUutilities.Vector3( asteroid.Entity.Position.X - 20f, asteroid.Entity.Position.Y, asteroid.Entity.Position.Z );
            models.Add( asteroid );

            ActiveTarget = asteroid; 
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
            //this.Game.GraphicsDevice.Clear( Color.CornflowerBlue );

            foreach ( RenderableModel model in models )
                model.Draw( gameTime, camera );
        }
    }
}
