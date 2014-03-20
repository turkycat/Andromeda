using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Andromeda.Entity;
using GregsCameraClass;

namespace Andromeda.Screen
{
    abstract class ScreenBase
    {
        //reference to the game
        protected Game Game
        {
            get;
            private set;
        }

        //a reference point for the camera class, any screen can modify this property to allow the screen to choose a point of view
        protected ITargetable ActiveTarget
        {
            get;
            set;
        }

        //a list of models which can be used to store the screen's contents
        protected List<RenderableModel> models;

        //basic constructor
        public ScreenBase( Game game )
        {
            this.Game = game;
            this.models = new List<RenderableModel>();
        }

        /**
         * returns a list of Entities used by this Screen. Should correspond to items which should exist in the physics environment
         */
        public List<BEPUphysics.Entities.Entity> GetEntities()
        {
            List<BEPUphysics.Entities.Entity> toReturn = new List<BEPUphysics.Entities.Entity>();

            foreach ( RenderableModel model in models )
                toReturn.Add( model.Entity );

            return toReturn;
        }

        //------------------------------------------------------------virtual methods, optional implementation for base classes


        /**
         * 
         * returns the ITargetable item which the camera should focus on while this screen is active
         */
        public virtual ITargetable GetCameraFocus()
        {
            return ActiveTarget;
        }




        //------------------------------------------------------------------abstract methods, required implementation for base classes

        /**
         * ABSTRACT
         * calls upon the screen object to update itself and/or it's entities
         */
        public abstract void Update( GameTime gameTime );

        /**
         * ABSTRACT
         * calls upon the screen object to draw itself and/or it's entities
         */
        public abstract void Draw( GameTime gameTime, Camera camera );
    }
}
