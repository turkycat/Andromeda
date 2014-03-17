#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

/*
 * @author Greg Hanes
 */

namespace GregsCameraClass
{
    public interface ITargetable
    {
        Matrix World { get; }
    }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera
    {
        private float cameraArc = -30;

        public float CameraArc
        {
            get { return cameraArc; }
            set { cameraArc = value; }
        }

        private float cameraRotation = 0;

        public float CameraRotation
        {
            get { return cameraRotation; }
            set { cameraRotation = value; }
        }

        private float cameraDistance = 20;

        public float NearDistance = 1.0f;

        public float CameraDistance
        {
            get { return targetCameraDistance; }
            set { targetCameraDistance = value; }
        }
        private float targetCameraDistance = 20;
        private Matrix view;
        private Matrix projection;

        public Matrix View
        {
            get
            {

                return view;
            }
        }

        public Matrix Projection
        {
            get
            {
                return projection;
            }
        }

        public ITargetable Target
        {
            get;
            set;
        }

        private Matrix _inverseViewProjection;

        public Matrix InverseViewProjection
        {
            get { return _inverseViewProjection; }
        }

        KeyboardState currentKeyboardState = new KeyboardState();
        GamePadState currentGamePadState = new GamePadState();
        GraphicsDevice device;

        public Camera( GraphicsDevice graphics )
        {
            device = graphics;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update( GameTime gameTime )
        {

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState( PlayerIndex.One );

            if ( Keyboard.GetState().IsKeyDown( Keys.Down ) )
            {
                CameraArc += 1f;
            }
            if ( Keyboard.GetState().IsKeyDown( Keys.Up ) )
            {
                CameraArc -= 1f;
            }
            if ( Keyboard.GetState().IsKeyDown( Keys.Left ) )
            {
                CameraRotation += 1f;
            }
            if ( Keyboard.GetState().IsKeyDown( Keys.Right ) )
            {
                CameraRotation -= 1f;
            }

            if ( Keyboard.GetState().IsKeyDown( Keys.R ) )
            {
                CameraDistance -= 1f;
            }
            if ( Keyboard.GetState().IsKeyDown( Keys.F ) )
            {
                CameraDistance += 1f;
            }

            float time = (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            // Limit the arc movement.
            if ( cameraArc > 90.0f )
                cameraArc = 90.0f;
            else if ( cameraArc < -90.0f )
                cameraArc = -90.0f;

            cameraDistance += 0.05f * ( targetCameraDistance - cameraDistance );

            // Limit the arc movement.
            if ( targetCameraDistance > 11900.0f )
                targetCameraDistance = 11900.0f;
            else if ( targetCameraDistance < 1.0f )
                targetCameraDistance = 1.0f;

            if ( currentGamePadState.Buttons.RightStick == ButtonState.Pressed )
            {
                ResetCamera();
            }

            if ( Target != null )
            {
                Vector3 camOffset = Vector3.Transform( new Vector3( 0, 0, cameraDistance ),
                                    ( Matrix.CreateRotationX( MathHelper.ToRadians( cameraArc ) ) *
                                    Matrix.CreateRotationY( MathHelper.ToRadians( cameraRotation ) ) ) );
                view = Matrix.CreateLookAt( camOffset + Target.World.Translation,
                                           Target.World.Translation, Vector3.Up );
            }
            else
            {
                view = Matrix.CreateRotationY( MathHelper.ToRadians( cameraRotation ) ) *
                       Matrix.CreateRotationX( MathHelper.ToRadians( cameraArc ) ) *
                       Matrix.CreateLookAt( new Vector3( 0, 0, -cameraDistance ), Vector3.Zero, Vector3.Up );
            }



            float aspectRatio = device.Viewport.AspectRatio;
            projection = Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4,
                                                                    aspectRatio,
                                                                    1,
                                                                    12000 );

            _inverseViewProjection = Matrix.Invert( view * projection );
        }

        public void ResetCamera()
        {
            cameraArc = -30;
            cameraRotation = 0;
            cameraDistance = 100;
            targetCameraDistance = 100;
        }
    }
}


