using Andromeda.GameElement;
using GregsCameraClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.Players
{
    public abstract class InputDevice
    {
        public PlayerIndex PlayerIndex
        {
            get;
            private set;
        }

        public InputDevice( PlayerIndex index )
        {
            this.PlayerIndex = index;
        }

        public abstract void ProcessMove( GameTime gameTime, Ship ship, Camera camera );
    }
}
