using Andromeda.GameElement;
using GregsCameraClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.Players
{
    class Player
    {
        private Ship ship;
        private InputDevice input;
        private Skybox skybox;

        public Ship Ship
        {
            get
            {
                return ship;
            }
        }

        public PlayerIndex PlayerIndex
        {
            get
            {
                return input.PlayerIndex;
            }
        }

        public Skybox Skybox
        {
            get
            {
                return skybox;
            }
        }

        public Player( Game game, PlayerIndex index )
        {
            //input = new Controller( index );
            input = new KeyboardInput( index );

            if ( index == PlayerIndex.One )
            {
                ship = new Ship( game, "redship" );
            }
            else
            {
                ship = new Ship( game, "blueship" );
            }
            skybox = new Skybox( game, ship );
        }


        public void Update( GameTime gameTime, Camera camera )
        {
            input.ProcessMove( gameTime, ship, camera );
            camera.Update( gameTime );
        }
    }
}
