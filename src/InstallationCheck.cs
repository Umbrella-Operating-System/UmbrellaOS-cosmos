/*
----> CONTENT:      
----> DATE:         11/29/2021
----> PROGRAMMERS:  Doonort3 < t.me/doonort3 >
----> TESTERS:      Discord: Pλweł#2343
*/

using System;
using System.IO;
using Umbrella.MainSystem;
using Umbrella.MainSystem.Console;

namespace Umbrella
{
    internal class InstallationCheck
    {
        public static void CheckStFile()
        {
            if (!Directory.Exists(@"0:\system"))
            {
                Directory.CreateDirectory(@"0:\system");
            }

            #region If there is no installation status file
            if (!File.Exists(@"0:\system\install.ubst"))
            {
                try
                {
                    SetupSystem.Initialization();
                    if (SetupSystem.InstallationStatus)
                    {
                        Kernel.Installed = true;
                        File.WriteAllText(@"0:\system\install.ubst", "Install performed previously");
                        MethodsInfo.ConsoleOk("Checking the installation of the system components is complete without errors.");
                    }
                    else
                    {
                        Kernel.StableRunning = false;
                        MethodsInfo.ConsoleWarning("One or more installation steps are not completed!" +
                                                   "\nThe system may not work correctly");
                    }
                }
                catch 
                {
                    Crush.CrushScreenWithoutException();
                    throw;
                }
                
            }
            #endregion If there is NO installation status file END

            #region If there is a file with installation status
            else if (File.Exists(@"0:\system\install.ubst"))
            {
                try
                {
                    var installStatus = File.ReadAllText(@"0:\system\install.st"); // Reading installation status from a file
                    if (installStatus is "Install performed previously") // If the text you read is the same as the text indicating that the system was installed earlier
                    {
                        MethodsInfo.ConsoleWarning("The installation has been done on this computer before. " +
                                                   "\nPress any key to skip the system check or press F5 to check the system correctness." +
                                                    // TODO
                                                   "\nYou can disable this choice by defaulting to anything with the command: " +
                                                   "\n'st checking true' or 'st checking false'");
                                                    // TODO END
                    }

                    var key = Console.ReadKey(false).Key;
                    if (key != ConsoleKey.F5) return; // If the button pressed is not F5 - skip
                    SetupSystem.Initialization();
                    if (SetupSystem.InstallationStatus) // Checking the completion of the installation program steps
                    {
                        Kernel.Installed = true;
                        MethodsInfo.ConsoleOk("Checking the installation of the system components is complete without errors.");
                    }
                    else
                    {
                        Kernel.StableRunning = false;
                        MethodsInfo.ConsoleWarning("One or more installation steps are not completed!" +
                                                   "\nThe system may not work correctly");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            #endregion If there is a file with installation status END
        }
    }
}
