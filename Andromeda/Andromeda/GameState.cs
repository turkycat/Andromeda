using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Andromeda
{
    using Screen;
    using BEPUphysics;
    using BEPUphysics.Threading;
    using BEPUphysics.CollisionRuleManagement;

    /**
     * a singleton class which provides global state information to the game
     */
    public class GameState
    {
        //the instance
        private static GameState instance;

        //the screen manager controls what the user sees
        private ScreenManager screenManager;

        //the physics engine manages the physics
        private Space physicsEngine;

        //reference to the game
        private Game game;


        /**
         * singleton instance getter
         */
        public static GameState Instance
        {
            get
            {
                if( instance == null )
                {
                    instance = new GameState();
                }
                return instance;
            }
        }

        /**
         * a hidden constructor
         */
        private GameState()
        {
        }


        //-----------------------------------public methods

        /**
         * initializes the GameState, guaranteeing encapsulation of the physics engine and screen manager
         */
        public void Initialize( Game game )
        {
            if ( game == null ) throw new InvalidOperationException();
            this.game = game;

            //initialize the physics engine with an optimum number of threads
            ParallelLooper looper = new ParallelLooper();
            for ( int i = 0; i < Environment.ProcessorCount; i++ )
            {
                looper.AddThread();
            }

            physicsEngine = new Space( looper );
            game.Services.AddService( typeof( Space ), physicsEngine );

            //initialize a screen manager and provide it to the GameState
            screenManager = new Screen.ScreenManager( game );
            game.Components.Add( screenManager );
            screenManager.Init();
        }



        /**
         * adds a mesh to the physics engine
         */
        public void AddEntity( BEPUphysics.Entities.Entity entity )
        {
            physicsEngine.Add( entity );
        }



        /**
         * removes a mesh from the physics engine
         */
        public void RemoveEntity( BEPUphysics.Entities.Entity entity )
        {
            physicsEngine.Remove( entity );
        }



        /**
         * allows the GameState to handle updates on a global scale for the entire game
         */
        public void Update( GameTime gameTime )
        {
            physicsEngine.Update();
            screenManager.Update( gameTime );
        }
    }
}
