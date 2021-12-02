/*
----> CONTENT:      The kernel, which declares the standard variables,
                    starts the installation and redirects it to the input.
----> DATE:         11/28/2021
----> PROGRAMMERS:  Doonort3 < t.me/doonort3 >
----> TESTERS:      Discord: Pλweł#2343
*/

using System;
using Cosmos.System.FileSystem;
using Umbrella.MainSystem;
using Umbrella.MainSystem.Console;
using Sys = Cosmos.System;


namespace Umbrella
{
    public class Kernel : Sys.Kernel
    {        
        #region Global Variables

        // Canvas canvas;
        ///// System startup status /////
        public static bool Running;
        public static bool StableRunning;
        public static bool VfsRunning;

        ///// System versions /////
        public static string Ver = "0.0.1";
        public static string DesktopVer = "0.0.1";

        ///// Main disk volume /////
        public static CosmosVFS Vfs = new();
        public static string CurrentDirectory = @"0:\";

        ///// User logged, name and pc name variables /////
        public static bool Logged = false;
        public static string CurrentUser = "root";

        ///// System install status /////
        public static bool Installed;

        ///// Console mode /////
        public static string[] ConsoleMode = {"CLIMode", "CULMode"};
        public static string SelectedConsoleMode;

        #region Not in use yet
        //public static string PcName;
        /*public static string BootTime =
            $"{Time.DayString()}/{Time.MonthString()}/{Time.YearString()}, " +
            $"{Time.TimeString(true, true, true)}"; // Русским привет! Эм, ну, и Британцам тоже*/
        #endregion

        #endregion Global Variables END

        #region Before Run

        protected override void BeforeRun()
        {
            /*canvas = FullScreenCanvas.GetFullScreenCanvas();
            canvas.Clear();
            canvas.Display();*/
            SelectedConsoleMode = ConsoleMode[0]; // Before the user selects the preprocessing mode, the mode is CLI
            MethodsInfo.ConsoleInfo("Booting Umbrella Operating System...");

            //////////////////////////
            SetupSystem.Initialization();

            if (SetupSystem.InstallationStatus)
            {
                Installed = true;
                MethodsInfo.ConsoleOk("Checking the installation of the system components is complete without errors.");
            }
            //////////////////////////

            MethodsInfo.ConsoleOk("System successfully started!");
            MethodsInfo.ConsoleInfo($"Current console mode: {SelectedConsoleMode}");
            Libraries.Screen.ClearScreen(ConsoleColor.DarkBlue, ConsoleColor.White); // Clear the screen, fill the background with dark blue and the text with white
        }
        #endregion

        #region Run
        protected override void Run()
        {
            CLI.Commands();
        }
        #endregion Run
    }
}