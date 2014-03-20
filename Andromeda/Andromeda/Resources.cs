using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Andromeda.Screen;

namespace Andromeda
{
    /**
     * a singleton class which provides resources to the game
     */
    class Resources
    {
        //the instance
        private static Resources instance;

        //private Dictionary<string, Texture2D> textures;
        private Dictionary<string, Model> models;
        private Dictionary<string, SoundEffect> sounds;


        /**
         * a Random number generator
         */
        public Random Random { get; private set; }


        /**
         * singleton instance getter
         */
        public static Resources Instance
        {
            get
            {
                if( instance == null )
                {
                    instance = new Resources();
                }
                return instance;
            }
        }



        /**
         * Adds the given Model to reference dictionary.
         */
        public void AddModel( string id, Model model )
        {
            if ( id != null && model != null )
            {
                models.Add( id, model );
            }
        }


        /**
         * retrieves the Model for the given resource id
         */
        public Model GetModel( string id )
        {
            if ( !models.ContainsKey( id ) ) return null;
            return models[id];
        }



        /**
         * Adds the given Model to reference dictionary.
         */
        public void AddSound( string id, SoundEffect effect )
        {
            if ( id != null && effect != null )
            {
                sounds.Add( id, effect );
            }
        }


        /**
         * retrieves the Model for the given resource id
         */
        public SoundEffect GetSound( string id )
        {
            if ( !sounds.ContainsKey( id ) ) return null;
            return sounds[id];
        }



        /**
         * a hidden constructor
         */
        private Resources()
        {
            sounds = new Dictionary<string, SoundEffect>();
            models = new Dictionary<string, Model>();
            Random = new Random();
        }
    }
}
