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


            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
        }

        


        /**
         * initialization logic for the game.
         */
        protected override void Initialize()
        {
            base.Initialize();
        }




        /**
         * Load all of content.
         */
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch( GraphicsDevice );

            Resources.Instance.AddModel( "universe", Content.Load<Model>( @"Models/universe" ) );
            Resources.Instance.AddModel( "asteroid_large1", Content.Load<Model>( @"Models/asteroid_large1" ) );
            Resources.Instance.AddModel( "asteroid_large2", Content.Load<Model>( @"Models/asteroid_large2" ) );
            Resources.Instance.AddModel( "asteroid_medium1", Content.Load<Model>( @"Models/asteroid_medium1" ) );
            Resources.Instance.AddModel( "asteroid_medium2", Content.Load<Model>( @"Models/asteroid_medium2" ) );
            Resources.Instance.AddModel( "asteroid_small1", Content.Load<Model>( @"Models/asteroid_small1" ) );
            Resources.Instance.AddModel( "asteroid_small2", Content.Load<Model>( @"Models/asteroid_small2" ) );

            GameState.Instance.Initialize( this );
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
