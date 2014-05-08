using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Andromeda.Screen;
using BEPUphysics.CollisionRuleManagement;

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
        private Dictionary<string, CollisionGroup> collisions;


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
         * Adds the given Model to reference dictionary.
         */
        public void AddGroup( string id, CollisionGroup group )
        {
            if ( id != null && group != null )
            {
                collisions.Add( id, group );
            }
        }


        /**
         * retrieves the Model for the given resource id
         */
        public CollisionGroup GetGroup( string id )
        {
            if ( !collisions.ContainsKey( id ) ) return null;
            return collisions[id];
        }



        /**
         * a hidden constructor
         */
        private Resources()
        {
            sounds = new Dictionary<string, SoundEffect>();
            models = new Dictionary<string, Model>();
            collisions = new Dictionary<string, CollisionGroup>();
            Random = new Random();
        }
    }
}
