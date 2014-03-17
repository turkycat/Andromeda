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

    /**
     * a singleton class which provides global state information to the game
     */
    public class GameState
    {
        //the instance
        private static GameState instance;

        //the screen manager controls what the user sees
        private ScreenManager screenManager;
        private Space physicsEngine;



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
         * Set the ScreenManager object so that the state class can control it.
         */
        public void SetScreenManager( ScreenManager provided )
        {
            if ( screenManager == null )
            {
                screenManager = provided;
            }
        }


        /**
         * Set the physics engine object so that the state class can control it.
         */
        public void SetPhysicsEngine( Space provided )
        {
            if ( physicsEngine == null )
            {
                physicsEngine = provided;
            }
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
