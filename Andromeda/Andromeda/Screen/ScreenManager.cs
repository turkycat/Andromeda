using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GregsCameraClass;

namespace Andromeda.Screen
{
    public class ScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //a reference to the currently active screen
        private ScreenBase activeScreen;

        //a reference to the camera object which will control the focus element of our screens.
        private Camera camera;

        //a dictionary of references to the various screens used within the game
        private Dictionary<string, ScreenBase> screens;


        public ScreenManager( Game game ) : base( game )
        {
            this.screens = new Dictionary<string,ScreenBase>();
            camera = new Camera( game.GraphicsDevice );
        }


        /**
         * initializes the ScreenManager and it's various screens
         *  - note: requires content to be loaded so that various screens can initialize themselves properly
         */
        public void Init()
        {
            screens.Add( "main", new MainScreen( this.Game ) );
            screens.Add( "game", new GameScreen( this.Game ) );
            screens.Add( "pause", new PauseScreen( this.Game ) );
            SetActiveScreen( "game" );
        }


        /**
         * controls the swapping of active screens
         */
        public void SetActiveScreen( string key )
        {
            if ( key == null || !screens.ContainsKey( key ) ) return;
            ScreenBase requested = screens[key];

            //remove all entities from the current screen, if there is one, from the physics manager
            if ( activeScreen != null )
            {
                List<BEPUphysics.Entities.Entity> currentEntities = activeScreen.GetEntities();
                foreach ( BEPUphysics.Entities.Entity activeEntity in currentEntities )
                {
                    GameState.Instance.RemoveEntity( activeEntity );
                }
            }

            //swap the active screen and add it's entities to the physics manager
            activeScreen = requested;
            List<BEPUphysics.Entities.Entity> newEntities = activeScreen.GetEntities();
            foreach ( BEPUphysics.Entities.Entity newEntity in newEntities )
            {
                GameState.Instance.AddEntity( newEntity );
            }

            SetCameraFocusPoint();
        }

        
        /**
         * sets or resets the active focus object for the camera
         */
        public void SetCameraFocusPoint()
        {
            camera.Target = activeScreen.GetCameraFocus();
        }


        public override void Update( GameTime gameTime )
        {
            activeScreen.Update( gameTime );
            camera.Update( gameTime );
            base.Update( gameTime );
        }


        

        /**
         * ScreenManager is a DrawableGameComponent, this method is automatically invoked during the proper Draw time
         * this method will defer the drawing operation to the active screen
         */
        public override void Draw( GameTime gameTime )
        {
            activeScreen.Draw( gameTime, camera );
            base.Draw( gameTime );
        }
    }
}
