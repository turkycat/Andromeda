using System;

namespace Andromeda
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (AndromedaMain game = new AndromedaMain())
            {
                game.Run();
            }
        }
    }
#endif
}

