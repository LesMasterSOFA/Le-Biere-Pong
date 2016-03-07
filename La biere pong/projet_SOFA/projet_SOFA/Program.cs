using System;

namespace AtelierXNA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Atelier game = new Atelier())
            {
                //Cache la console étant donné que l'application est de type console
                ConsoleWindow.HideConsoleWindow();
                
                game.Run();
            }
        }
    }
#endif
}

