using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Andromeda.Entity;
using GregsCameraClass;

namespace Andromeda.Screen
{
    abstract class Screen
    {
        protected Game Game { get; private set; }

        protected List<RenderableModel> models;

        public Screen( Game game )
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

        //------------------------------------------------------------------abstract methods

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

        /**
         * ABSTRACT
         * returns the ITargetable item which the camera should focus on while this screen is active
         */
        public abstract ITargetable GetCameraFocus();
    }
}
