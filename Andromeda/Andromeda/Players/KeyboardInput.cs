using Andromeda.GameElement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GregsCameraClass;

namespace Andromeda.Players
{
    public class KeyboardInput : InputDevice
    {
        public KeyboardInput( PlayerIndex index ) : base( index ) {}

        public override void ProcessMove( GameTime gameTime, Ship ship, Camera camera )
        {
            KeyboardState state = Keyboard.GetState( PlayerIndex );

            BEPUutilities.Vector3 movement = BEPUutilities.Vector3.Zero;

            if ( state.IsKeyDown( Keys.W ) )
            {
                movement += new BEPUutilities.Vector3( 0f, 0f, -1f );
            }

            if ( state.IsKeyDown( Keys.S ) )
            {
                movement += new BEPUutilities.Vector3( 0f, 0f, 1f );
            }

            if ( state.IsKeyDown( Keys.A ) )
            {
                movement += new BEPUutilities.Vector3( -1f, 0f, 0f );
            }

            if ( state.IsKeyDown( Keys.D ) )
            {
                movement += new BEPUutilities.Vector3( 1f, 0f, 0f );
            }

            if ( state.IsKeyDown( Keys.Q ) )
            {
                movement += new BEPUutilities.Vector3( 0f, 1f, 0f );
            }

            if ( state.IsKeyDown( Keys.Z ) )
            {
                movement += new BEPUutilities.Vector3( 0f, -1f, 0f );
            }


            

            ship.Entity.LinearVelocity += movement * 0.25f;// * 10;
            ship.Update();
        }
    }
}
