using System;

namespace Element
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ElementGame game = new ElementGame())
            {
                game.Run();
            }
        }
    }
#endif
}

