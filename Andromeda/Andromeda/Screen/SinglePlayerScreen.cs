using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andromeda.GameElement;
using GregsCameraClass;
using Andromeda.Players;

namespace Andromeda.Screen
{
    class SinglePlayerScreen : ScreenBase
    {
        // a reference to the invisible sphere in which the game takes place
        protected Universe universe;

        protected Player redPlayer;
        protected Camera redCamera;

        public SinglePlayerScreen( Game game ) : base( game )
        {
            //universe = new Universe( game, "universe" );
            universe = new Universe( game );

            redCamera = new Camera( game.GraphicsDevice );

            redPlayer = new Player( game, PlayerIndex.One );
            models.Add( redPlayer.Ship );

            //Skybox skybox = new Skybox( game, redCamera );
            //models.Add( skybox );

            for ( int i = 0; i < 30; ++i )
            {
                Asteroid asteroid = Asteroid.GenerateAsteroid( game, i % 3 );
                models.Add( asteroid );
            }

            redCamera.Target = redPlayer.Ship;
        }


        public override void Update( GameTime gameTime )
        {
            //SetCameraFocusPoint();
            redPlayer.Update( gameTime, redCamera );
        }


        /**
         * called by the screen manager when this is the active screen
         */
        public override void Draw( GameTime gameTime )
        {
            redPlayer.Skybox.Draw( gameTime, redCamera );
            foreach ( RenderableElement model in models )
                model.Draw( gameTime, redCamera );
        }

        /**
         * returns a list of Entities used by this Screen. Should correspond to items which should exist in the physics environment
         */
        public override List<BEPUphysics.Entities.Entity> GetEntities()
        {
            List<BEPUphysics.Entities.Entity> toReturn = base.GetEntities();

            toReturn.Add( universe.Entity );

            return toReturn;
        }
    }
}
