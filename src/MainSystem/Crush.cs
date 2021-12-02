using System;
using Cosmos.System;

namespace Umbrella.MainSystem
{
    internal class Crush
    {
        public static void CrushScreen(Exception e)
        {
            #region disable variables

            Kernel.VfsRunning = false;
            Kernel.Running = false;

            #endregion

            #region msg

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.DarkRed;
            System.Console.Clear();
            System.Console.WriteLine("\n========================================================================");
            System.Console.WriteLine("\nStatus: System is crashed!                       ");
            System.Console.WriteLine("\n================================         ");
            System.Console.WriteLine("  Possible causes:                       ");
            System.Console.WriteLine("  - An Kernel Error in Umbrella              ");
            System.Console.WriteLine("  - Filesystem Error                     ");
            System.Console.WriteLine("  - Other Error                        ");
            System.Console.WriteLine("================================       ");
            System.Console.WriteLine($"\nInfo: {e}, " + $"\nOS version: {Kernel.Ver}, " +
                                     $"\nDesktop version: {Kernel.DesktopVer}, " +
                                     $"\nCurrent directory: {Kernel.CurrentDirectory}");
            // System.Console.WriteLine($"\nLast knows address: {ex}");
            System.Console.WriteLine("\n========================================================================");
            System.Console.WriteLine("\n    Press R key to restart or S key to shutdown...");

            #endregion

            #region check key

            var pressKey = System.Console.ReadKey(true);
            if (pressKey.Key == ConsoleKey.R)
            {
                System.Console.WriteLine("\nRestarting...");
                Power.Reboot();
            }

            if (pressKey.Key == ConsoleKey.S)
            {
                System.Console.WriteLine("Shutting down...");
                Power.Shutdown();
            }

            #endregion
        }

        public static void CrushScreenWithoutException()
        {
            #region disable variables

            Kernel.VfsRunning = false;
            Kernel.Running = false;

            #endregion

            #region msg

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.DarkRed;
            System.Console.Clear();
            System.Console.WriteLine("\n========================================================================");
            System.Console.WriteLine("\nStatus: System is crashed!                       ");
            System.Console.WriteLine("\n================================         ");
            System.Console.WriteLine("  Possible causes:                       ");
            System.Console.WriteLine("  - An Kernel Error in Umbrella              ");
            System.Console.WriteLine("  - Filesystem Error                     ");
            System.Console.WriteLine("  - Other Error                        ");
            System.Console.WriteLine("================================       ");
            System.Console.WriteLine($"\nOS version: {Kernel.Ver}, " +
                                     $"\nDesktop version: {Kernel.DesktopVer}, " +
                                     $"\nCurrent directory: {Kernel.CurrentDirectory}");
            // System.Console.WriteLine($"\nLast knows address: {ex}");
            System.Console.WriteLine("\n========================================================================");
            System.Console.WriteLine("\n    Press R key to restart or S key to shutdown...");

            #endregion

            #region check key

            var pressKey = System.Console.ReadKey(true);
            if (pressKey.Key == ConsoleKey.R)
            {
                System.Console.WriteLine("\nRestarting...");
                Power.Reboot();
            }

            if (pressKey.Key == ConsoleKey.S)
            {
                System.Console.WriteLine("Shutting down...");
                Power.Shutdown();
            }

            #endregion
        }
    }
}