using Andromeda.GameElement;
using GregsCameraClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConversionHelper;

namespace Andromeda.Players
{
    class Controller : InputDevice
    {

        public Controller( PlayerIndex index ) : base( index ) {}

        public override void ProcessMove( GameTime gameTime, Ship ship, Camera camera )
        {
            GamePadState state = GamePad.GetState( PlayerIndex );

            float thumbstickLeft = -state.ThumbSticks.Left.Y;

            BEPUutilities.Vector3 movement = new BEPUutilities.Vector3( state.ThumbSticks.Left.X, 0f, state.ThumbSticks.Left.Y * camera.View.Forward.Z );

            BEPUutilities.Vector3 rotation = new BEPUutilities.Vector3( state.ThumbSticks.Right.Y, -state.ThumbSticks.Right.X, 0f );
            rotation *= 0.25f;

            ship.Entity.LinearVelocity += movement * 0.25f;// * 10;
            camera.CameraRotation += rotation.Y;
            ship.Yaw -= rotation.Y;
            ship.Pitch += rotation.X;
            ship.Update();
        }
    }
}
