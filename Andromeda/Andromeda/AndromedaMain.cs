using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Andromeda
{
    using Screen;
    using BEPUphysics.Threading;
    using BEPUphysics;
    using GregsCameraClass;
    using Andromeda.Entity;

    public class AndromedaMain : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //private Space physicsEngine;
        //private ScreenManager screenManager;

        public AndromedaMain()
        {
            graphics = new GraphicsDeviceManager( this );
            Content.RootDirectory = "Content";


            //build a thread for each processor core and initialize the physics engine with the task executor.
            ParallelLooper looper = new ParallelLooper();
            for ( int i = 0; i < Environment.ProcessorCount; i++ )
            {
                looper.AddThread();
            }

            Space physicsEngine = new Space( looper );
            Services.AddService( typeof( Space ), physicsEngine );
            GameState.Instance.SetPhysicsEngine( physicsEngine );


            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
        }

        


        /**
         * initialization logic for the game.
         */
        protected override void Initialize()
        {
            //initialize a screen manager and provide it to the GameState
            ScreenManager screenManager = new Screen.ScreenManager( this );
            GameState.Instance.SetScreenManager( screenManager );
            Components.Add( screenManager );

            base.Initialize();
        }




        /**
         * Load all of content.
         */
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );
        }






        /**
         * Unload all content.
         */
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }





        /**
         * Game Loop Element: Update the game logic based on elapsed time
         */
        protected override void Update( GameTime gameTime )
        {
            // Allows the game to exit
            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed )
                this.Exit();

            GameState.Instance.Update( gameTime );

            base.Update( gameTime );
        }



        /**
         * Game Loop Element: Called when the game should draw itself.
         */
        protected override void Draw( GameTime gameTime )
        {
            //I like pie
            base.Draw( gameTime );
        }
    }
}
