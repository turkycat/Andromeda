using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

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
         * a hidden constructor
         */
        private Resources()
        {
            sounds = new Dictionary<string, SoundEffect>();
            Random = new Random();
        }
    }
}
