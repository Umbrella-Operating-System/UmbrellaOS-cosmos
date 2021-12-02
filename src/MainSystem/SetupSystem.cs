/*
 * ----> CONTENT:      The main installation file, 
                    which authorizes, sets the system mode 
                    and creates the system folder.
 * ----> DATE:         11/29/2021
 * ----> PROGRAMMERS:  Doonort3 < t.me/doonort3 >
 * * ----> TESTERS:      Discord: Pλweł#2343
*/

using System;
using System.IO;
using Cosmos.System.FileSystem.VFS;
using Umbrella.Libraries;
using Umbrella.MainSystem.Console;
using static Umbrella.MainSystem.Console.WriteMethods;

namespace Umbrella.MainSystem
{
    public class Config
    {
        public static bool ShowCompletionStatus;
    }
    /// <summary>
    ///     Installing and checking system components
    /// </summary>
    internal class SetupSystem
    {
        public static bool LoginFuncStatus;
        public static bool CreateSystemDirsStatus;
        public static bool SelectConsoleModeStatus;
        public static bool InstallationStatus;

        public static void Initialization()
        {
            // Add "super design"
            System.Console.Clear();
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Clear();

            try
            {
                #region Primary, necessary initializations

                static bool ContainsVolumes()
                {
                    return Kernel.Vfs.GetVolumes().Count != 0;
                }

                VFSManager.RegisterVFS(Kernel.Vfs); // Reg vfs
                if (ContainsVolumes())
                {
                    Kernel.VfsRunning = true;
                    System.Console.Clear();
                    MethodsInfo.ConsoleOk("FileSystem Registration");
                }
                else
                {
                    Kernel.VfsRunning = false;
                    System.Console.Clear();
                    MethodsInfo.ConsoleError("FileSystem Registration");
                }

                if (!Kernel.VfsRunning)
                {
                    MethodsInfo.ConsoleError("VFS not initialized");
                    LoginFuncStatus = false;
                    Crush.CrushScreenWithoutException();
                }
                else if (!CreateSystemDirectories())
                {
                    CreateSystemDirsStatus = false;
                    MethodsInfo.ConsoleError("System dirs");
                    Crush.CrushScreenWithoutException();
                }
                else if (CreateSystemDirectories())
                {
                    MethodsInfo.ConsoleOk("System dirs");
                    CreateSystemDirsStatus = true;
                }

                #endregion necessary INIT END
            }
            catch (Exception e)
            {
                Crush.CrushScreen(e);
                throw;
            }


            #region Reading and checking the file, which is responsible for the installation status of the system

            if (File.Exists(@"0:\system\installst.ubst"))
            {
                var installStatusInFile = File.ReadAllText(@"0:\system\installst.ubst");
                if (!string.IsNullOrEmpty(installStatusInFile))
                {
                    if (installStatusInFile.Trim(' ') is "1")
                    {
                        MethodsInfo.ConsoleInfo("The system was installed earlier, \n" +
                                                "if you want to enter recovery mode, after starting enter 'st recovery'.");
                        InstallationStatus = true;
                        MethodsInfo.ConsoleOk("System successfully started!");
                        MethodsInfo.ConsoleInfo($"Current console mode: {Kernel.SelectedConsoleMode}");
                        Screen.ClearScreen(ConsoleColor.DarkBlue,
                            ConsoleColor.White); // Clear the screen, fill the background with dark blue and the text with white
                        CLI.Commands();
                    }
                }
                else
                {
                    MethodsInfo.ConsoleInfo("The configuration file exists, but it is empty, \n" +
                                            "so we can't tell if the system has been installed before. \n" +
                                            "By default, the system will offer to install it again.");
                    Kernel.StableRunning = false;
                }
            }
            else
            {
                MethodsInfo.ConsoleWarning("Failed to get an installation status, reset to reinstall.");
            }

            #endregion END

            #region How to install system

            System.Console.Clear();
            MethodsInfo.ConsoleInfo("Select the installation method: ");
            WriteLine("\n[ 1 ] Easy installation\n[ 2 ] Manual installation\n");
            Write("> ");

            var key = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(key))
            {
                if (!Kernel.VfsRunning)
                {
                    Kernel.VfsRunning = false;
                    MethodsInfo.ConsoleError("VFS not initialized");
                    LoginFuncStatus = false;
                    Crush.CrushScreenWithoutException();
                }
                else
                {
                    if (key.Trim(' ') is "1") EasyInstall();
                    else if (key.Trim(' ') is "2") ManualInstall();
                }
            }
            else
            {
                WriteLine("Easy installation select by default");
                EasyInstall();
            }

            #endregion How to install system END
        }

        #region System installation methods

        /// <summary>
        /// An automatic setup that uses methods,
        /// and if they are executed without errors, sets their execution status to true
        /// </summary>
        private static void EasyInstall()
        {
            #region Login execute check

            // Check Login func complete
            if (!Kernel.VfsRunning)
            {
                Kernel.VfsRunning = false;
                MethodsInfo.ConsoleError("VFS not initialized");
                LoginFuncStatus = false;
                Crush.CrushScreenWithoutException();
            }
            else if (Login())
            {
                MethodsInfo.ConsoleOk("Login"); // Output the execution message without errors
                Kernel.Logged = true; // Setting the execution status (login completed)
            }
            else
            {
                MethodsInfo.ConsoleError("Login"); // Output the execution message without errors
                Kernel.Logged = false; // Setting the execution status (login completed)
                Crush.CrushScreenWithoutException();
            }

            #endregion Login execute check END

            #region Console mode execute check

            if (!Kernel.VfsRunning)
            {
                Kernel.VfsRunning = false;
                MethodsInfo.ConsoleError("VFS not initialized");
                LoginFuncStatus = false;
            }
            else if (SelectMode())
            {
                MethodsInfo.ConsoleOk("Console mode");
                SelectConsoleModeStatus = true;
            }
            else
            {
                MethodsInfo.ConsoleError("Console mode");
                SelectConsoleModeStatus = false;
            }

            #endregion Console mode execute check END

            #region Final check all statuses

            if (CreateSystemDirsStatus && CreateSystemDirsStatus && SelectConsoleModeStatus)
            {
                if (File.Exists(@"0:\system\installst.ubst"))
                {
                    File.WriteAllText(@"0:\system\installst.ubst", "1");
                    InstallationStatus = true;
                }
                else if (!File.Exists(@"0:\system\installst.ubst"))
                {
                    File.Create(@"0:\system\installst.ubst"); // I know that ЦriteAllText creates a file if it does not exist
                    File.WriteAllText(@"0:\system\installst.ubst", "1");
                    InstallationStatus = true;
                }
            }
            else
            {
                MethodsInfo.ConsoleError("Not all steps of the installation are completed correctly, \n" +
                                         "please check the disk, otherwise write to the developer.");
                InstallationStatus = false;

                if (!Kernel.VfsRunning) return;
                File.Create(@"0:\system\installst.ubst");
                File.WriteAllText(@"0:\system\installst.ubst", "0");
            }

            #endregion #region Final checkEND
        }

        /// <summary>
        /// Manual installation that does not use methods
        /// and is done by the user
        /// </summary>
        private static void ManualInstall()
        {
            input:
            System.Console.Clear();
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Clear();

            // Before input
            Write($"\n{Kernel.CurrentDirectory}", ConsoleColor.White);
            Write("@", ConsoleColor.Gray);
            Write($"{Kernel.CurrentUser}", ConsoleColor.DarkBlue);
            Write("> ", ConsoleColor.White);
            // Get input
            var input = System.Console.ReadLine();
            var arguments = Methods.ParseCommandLine(input); // Parse arguments
            var commandName = arguments[0]; // Command name
            if (arguments.Count > 0) arguments.RemoveAt(0); // Leave only the arguments

            if (!Kernel.VfsRunning)
            {
                Kernel.VfsRunning = false;
                MethodsInfo.ConsoleError("VFS not initialized");
                LoginFuncStatus = false;
                Crush.CrushScreenWithoutException();
            }
            else
            {
                if (commandName.Trim(' ').ToLower() is "st")
                {
                    if (arguments.Count > 0 && arguments[0].ToLower() is "-h" or "--help")
                    {
                        Methods.SnowHelp("st [1ARG] [2ARG [3ARG] ...", "This command allows you to set user settings. Write 'st list' to get the list", "-h, --help)");
                        goto input;
                    }

                    if (arguments.Count > 0 && arguments[0].ToLower() is "list")
                    {
                        WriteLine("set" +
                                  "\n\tshowCompletionStatus" +
                                  "\n\tconsole" +
                                  "\n\t\tmode" +
                                  "\n\t\ttest");
                        goto input;
                    }

                    if (arguments.Count > 0 && arguments[0].ToLower() is not "-h" or "--help" or "list" && !arguments[0].Contains('-'))
                    {
                        if (Methods.StCommand.Execute(arguments) is false)
                        {
                            WriteLine("An error has occurred", ConsoleColor.Red);
                        }
                        else
                        {
                            if (Config.ShowCompletionStatus)
                            {
                                MethodsInfo.ConsoleOk("st -> OK");
                            }
                        }
                    }
                }

                if (commandName.Trim(' ').ToLower() is "mkdir")
                {
                    if (arguments.Count > 0 && arguments[0].ToLower() is "-h" or "--help")
                    {
                        Methods.SnowHelp("mkdir [DIR]", "This command create empty folder.", "-h, --help)");
                        goto input;
                    }

                    if (arguments.Count > 0 && arguments[0] is not "-h" or "--help" && !arguments[0].Contains('-'))
                    {
                        if (Methods.MkdirCommand.Execute(arguments) is false) 
                        {
                            WriteLine("An error has occurred", ConsoleColor.Red);
                        }
                        else
                        {
                            if (Config.ShowCompletionStatus)
                            {
                                MethodsInfo.ConsoleOk("mkdir > OK");
                            } 
                        }
                    }



                    #region old

                    /*if (commandName.Trim(' ').ToLower() is "mkdir")
                    {
                        #region Вывод помощи
                        if (arguments.Count > 0 && arguments[0].ToLower() is "-h" or "--help")
                        {
                            CLI.SnowHelp("mkdir [ARG IF NEED '-h' ] [DIR]", "This command create empty folder.", "-h, --help)");
                            goto input;
                        }
                        #endregion Вывод помощи END
    
                        #region Без опций
                        if (arguments.Count > 0 && arguments[0] is not "-h" or "--help" && !arguments[0].Contains('-'))
                        {
                            if (!arguments[0].Contains('.') && !arguments[0].Contains(@"\") && !arguments[0].Contains("/") &&
                                !arguments[0].Contains(':') && !arguments[0].Contains('*') && !arguments[0].Contains('?') &&
                                !arguments[0].Contains('<') && !arguments[0].Contains('>') && !arguments[0].Contains('|') &&
                                !arguments[0].Contains('+'))
                            {
                                #region Основной код
    
                                if (Directory.Exists(arguments[0])) // Если путь уже существует
                                {
                                    System.Console.WriteLine("That path exists already.");
                                    goto input;
                                }
    
                                if (!Directory.Exists(arguments[0])) // Если путь не существует - создаём
                                {
                                    Directory.CreateDirectory(@$"{Kernel.CurrentDirectory}{arguments[0]}");
                                    System.Console.WriteLine("Directory created!");
                                    goto input;
                                }
    
                                #endregion Основной код END
                            }
                            #region Если запрещённые символы есть
                            else
                            {
                                System.Console.WriteLine(
                                    "The following characters are prohibited in the names of folders and files:\n" +
                                    @"'-' '.' '\' '/' ':' '*' '?' '<' '>' '|' '+'");
                                goto input;
                            }
                            #endregion Если запрещённые символы есть END
                        }
    
                        #endregion Если аргументы есть, но они не являются '-h' или '--help' END
    
                        #region Если добавлены лишние аргументы к опции
    
                        if (arguments.Count > 1 &&
                            arguments[0].ToLower() is "-h"
                                or "--help") // Если аргументов больше одного, и один из них это существующая опция (человек пытает написать лишний аргумент к опции, типа $ ls -al CurrentDir"
                        {
                            System.Console.WriteLine(
                                "The option is used without additional arguments. \nmkdir [ARG '-h' or '--help' if need] [DIRNAME]");
                            goto input;
                        }
    
                        #endregion Если добавлены лишние аргументы END
    
                        #region Если опции не существует
    
                        if (arguments.Count > 0 && arguments[0].ToLower().Contains('-') &&
                            arguments[0].ToLower() is not "-h" or "--help")
                        {
                            System.Console.WriteLine("Invalid option");
                            goto input;
                        }
    
                        #endregion Если опции не существует END
                    }
    
                    if (commandName.Trim(' ').ToLower() is "rmdir")
                    {
    
    
                        if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                        {
                            CLI.SnowHelp("rmdir [ARG IF NEED] [FILE]", "This command deletes existing, specified directories",
                                "-h, --help, -r");
                            goto input;
                        }
    
                        if (arguments.Count > 1 && (arguments[0].ToLower() is "-r"))
                        {
                            string[] folders = Directory.GetDirectories(Kernel.CurrentDirectory + @"\" + arguments[1]);
                            foreach (string folder in folders)
                            {
                                string[] files = Directory.GetFiles(folder);
                                foreach (string file in files)
                                {
                                    File.Delete(file);
                                }
    
                                Directory.Delete(folder);
                            }
    
                            WriteLine("[ OK ] Directory delete.");
                            goto input;
                        }
    
                        if (arguments.Count is 1 && (arguments[0].ToLower() is "-r"))
                        {
                            WriteLine($"Use \"{commandName} -r [DIR]\" to delete the directory with all subdirectories and files.");
                            goto input;
                        }
    
                        if (arguments.Count > 0 && (arguments[0].ToLower() is not "-r"))
                        {
                            /*if (Directory.EnumerateFiles(@$"{Kernel.CurrentDirectory}\{arguments[0]}\", "*",
                                    SearchOption.AllDirectories).Any())
                            {
                                WriteLine("[ ERROR ] directory is not empty.");
                                goto input;
                            }
    
    
    
    
    
                            //string[] dirs = Directory.GetFiles(Kernel.CurrentDirectory, "*");
                            try
                            {
                                Directory.Delete(arguments[0]);
                                WriteLine("[ OK ] directory deleting.");
                                goto input;
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e);
                                goto input;
                            }
    
                            
    
                            if (foreachFiles.Length is 0 || foreachDirectories.Length is 0)
                            {
                                Directory.Delete(arguments[0]);
                                Console.WriteLine("[ OK ] directory deleting.");
                                goto input;
                            }*/

                    /*if (foreachFiles.Length is not 0 || foreachDirectories.Length is not 0)
                    {
                        Console.WriteLine("[ ERROR ] directory is not empty.");
                        goto input;
                    }*/

                    /*
                }
    
                if (arguments.Count is 0)
                {
                    WriteLine($"Use \"{commandName} -h\" to display all available actions.");
                    goto input;
                }
            }
    
            if (commandName.Trim(' ').ToLower() is "ctfile")
            {
                if (arguments.Count > 0 && !File.Exists(Kernel.CurrentDirectory + arguments[0]))
                {
                    File.Create(arguments[0]);
                    goto input;
                }
    
                if (arguments.Count > 0 && File.Exists(Kernel.CurrentDirectory + arguments[0]))
                {
                    WriteLine("File already exist");
                    goto input;
                }
            }
    
            if (commandName.Trim(' ').ToLower() is "rmfile")
            {
                if (arguments.Count > 0 && File.Exists(Kernel.CurrentDirectory + arguments[0]))
                {
                    File.Delete(arguments[0]);
                    goto input;
                }
    
                if (arguments.Count > 0 && !File.Exists(Kernel.CurrentDirectory + arguments[0]))
                {
                    WriteLine("File is not exist");
                    goto input;
                }
            }
    
            if (commandName.TrimEnd(' ').ToLower() is "cd")
            {
                if (arguments.Count > 0 && (arguments[0] is "-h" or "--help"))
                {
                    CLI.SnowHelp("cd [DIR]", "This command allows you to change the working directory.", "-h --help, ..");
                    goto input;
                }
                    */


                    /*if (arguments.Count > 0 && (arguments[0] is ".."))
                    {
                        Kernel.CurrentDirectory = Kernel.CurrentDirectory + "\\";
                        var pos = Kernel.CurrentDirectory.LastIndexOf("\\", StringComparison.Ordinal);
                        Kernel.CurrentDirectory = Kernel.CurrentDirectory.Remove(pos, Kernel.CurrentDirectory.Length - pos);
                        Directory.SetCurrentDirectory(Kernel.CurrentDirectory);
                    }*/
                    /*
    
                    if (arguments.Count > 0 && (arguments[0] is not "-h" or "--help" or ".."))
                    {
                        if (!Directory.Exists(Kernel.CurrentDirectory + arguments[0]))
                        {
                            WriteLine("Directory is not exist");
    
                            goto input;
                        }
    
                        if (Directory.Exists(Kernel.CurrentDirectory + arguments[0]))
                        {
                            Kernel.CurrentDirectory = Kernel.CurrentDirectory + arguments[0];
                            Directory.SetCurrentDirectory(Kernel.CurrentDirectory);
    
                            goto input;
                        }
                    }
    
                    if (arguments.Count is 0) // Home
                    {
                        Directory.SetCurrentDirectory(@"0:\");
    
                        goto input;
                    }
                }
    
                if (commandName.TrimEnd(' ').ToLower() is "pwd")
                {
                    if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                    {
                        CLI.SnowHelp("pwd", "This command display current working directory.", "-h --help");
                        goto input;
                    }
    
                    if (arguments.Count is 0)
                    {
                        WriteLine(Directory.GetCurrentDirectory());
                        goto input;
                    }
                }
    
                if (commandName.Trim(' ').ToLower() is "cat")
                {
                    if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                    {
                        CLI.SnowHelp("cat [ARG IF NEED] [FILE]", "This command outputs the content of the file, creates and joins", "-h, --help, --new, -j");
                        goto input;
                    }
    
                    #region '--new' argument
                    if (arguments.Count is 1 && (arguments[0].ToLower() is "--new"))
                    {
                        WriteLine($"Use \"{commandName} --new [FILENAME]\" to create a new clean file.");
                        goto input;
                    }
                    if (arguments.Count > 1 && (arguments[0].ToLower() is "--new"))
                    {
                        try
                        {
                            File.Create($"{arguments[1]}");
                            WriteLine($"New file created - {arguments[1]}");
                            goto input;
    
                        }
                        catch (Exception e)
                        {
                            WriteLine($"File not created. \nError: {e}");
                            goto input;
                        }
    
                    }
                    #endregion
    
                    #region '-j' argument
    
                    if (arguments.Count > 0 && (arguments[0].ToLower() is "-j"))
                    {
                        WriteLine($"Use \"{commandName} -j [FILENAME1] [FILENAME2] [FILENAMEFORNEWFILE]\" to join the content of the files into one new.");
                        goto input;
                    }
    
                    if (arguments.Count > 0 && (arguments[0].ToLower() is "-j"))
                    {
                        try
                        {
                            if (File.Exists(Kernel.CurrentDirectory + arguments[1]) && File.Exists(Kernel.CurrentDirectory + arguments[2]))
                            {
                                var contentInFirstFile = File.ReadAllText(arguments[1]);
                                var contentInSecondFile = File.ReadAllText(arguments[2]);
    
                                File.WriteAllText($"{arguments[3]}", $"{contentInFirstFile}\n{contentInSecondFile}");
                                if (string.IsNullOrEmpty(contentInFirstFile) || string.IsNullOrEmpty(contentInSecondFile))
                                {
                                    WriteLine("Warning! \nOne or more join files are empty.");
                                }
                                WriteLine($"Files joined in {arguments[3]}");
                                goto input;
                            }
    
                            if (!File.Exists(Kernel.CurrentDirectory + arguments[1]) || !File.Exists(Kernel.CurrentDirectory + arguments[2]))
                            {
                                WriteLine("One or more files are not found.");
                                goto input;
                            }
                        }
                        catch (Exception e)
                        {
                            WriteLine($"The command did not execute and the files are not joined. \nError: {e}");
                            goto input;
                        }
    
                        WriteLine($"Use \"{commandName} -j [FILENAME1] [FILENAME2] [FILENAMEFORNEWFILE]\" to join the content of the files into one new.");
                        goto input;
                    }
    
                    #endregion
    
                    if (arguments.Count > 0)
                    {
    
                        if (File.Exists(Kernel.CurrentDirectory + arguments[0]))
                        {
                            try
                            {
                                var contentInFile = File.ReadAllText(Kernel.CurrentDirectory + arguments[0]);
    
                                WriteLine(contentInFile);
                                goto input;
                            }
                            catch (Exception e)
                            {
                                WriteLine($"The command did not execute and the file are not read. \nError: {e}");
                                goto input;
                            }
                        }
    
                        if (!File.Exists(Kernel.CurrentDirectory + arguments[0]))
                        {
                            WriteLine("File not found.");
                            goto input;
                        }
                    }
    
                    if (arguments.Count is 1)
                    {
                        WriteLine($"Use \"{commandName} -h\" to display all available actions.");
                        goto input;
                    }
                }
    
                if (commandName.Trim(' ').ToLower() is "writetext")
                {
    
                    if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                    {
                        CLI.SnowHelp("writetext [ARG IF NEED] [FILE]", "This command writes and clears the file", "-h, --help, --clr");
                        goto input;
                    }
    
                    if (arguments.Count > 0 && arguments[0].ToLower() is "--clr")
                    {
                        if (arguments.Count > 1 && File.Exists(Kernel.CurrentDirectory + arguments[1]))
                        {
                            File.WriteAllText(Kernel.CurrentDirectory + arguments[1], string.Empty);
                            goto input;
                        }
    
                        if (arguments.Count > 1 && !File.Exists(Kernel.CurrentDirectory + arguments[1]))
                        {
                            WriteLine("File not exist");
                            goto input;
                        }
    
                        if (arguments.Count is 1)
                        {
                            WriteLine("Use 'writetext --clr [FILENAME]'");
                            goto input;
                        }
                    }
    
                    if (arguments.Count > 0 && arguments[0].ToLower() is not "--clr")
                    {
                        if (arguments.Count > 1 && File.Exists(Kernel.CurrentDirectory + arguments[0]))
                        {
                            File.WriteAllText(Kernel.CurrentDirectory + arguments[0], arguments[1]);
                            goto input;
                        }
    
                        if (arguments.Count > 1 && !File.Exists(Kernel.CurrentDirectory + arguments[0]))
                        {
                            WriteLine("File not exist");
                            goto input;
                        }
    
                        if (arguments.Count is 1)
                        {
                            WriteLine($"Use \"{commandName} -h\" to display all available actions.");
                            goto input;
                        }
                    }
                }
    
                if (commandName.Trim(' ').ToLower() is "cls" or "clear")
                {
                    if (arguments.Count > 0 && (arguments[0].ToLower() is "-h" or "--help"))
                    {
                        CLI.SnowHelp("cls (or clear)", "This command clear console", "-h, --help");
                        goto input;
                    }
    
                    if (arguments.Count is 0)
                    {
                        System.Console.Clear();
                        goto input;
                    }
                }*/

                    #endregion old END
                }

                if (commandName.Trim(' ').ToLower() is "rmdir")
                {
                    if (arguments.Count > 0 && arguments[0].ToLower() is "-h" or "--help")
                    {
                        Methods.SnowHelp("rmdir [ARG IF NEED] [DIR]", "This command create empty folder.", "-h, --help)");
                        goto input;
                    }

                    if (arguments.Count > 0 && arguments[0] is not "-h" or "--help" && !arguments[0].Contains('-'))
                    {

                    }
                }
            }
        }

        #endregion System installation END

        #region Functions for Easy install

        #region Log in function

        /// <summary>
        ///     Logging in or creating a user
        /// </summary>
        /// <returns>Status of function execution</returns>
        private static bool Login()
        {
            try
            {
                if (File.Exists(@"0:\system\login.ubcfg")) // Actions If a config with user data exists
                {
                    var loginInUbcfg = File.ReadAllText(@"0:\system\login.ubcfg");
                    var loginInUbcfgSplit = loginInUbcfg.Split(' ');

                    if (Kernel.SelectedConsoleMode == Kernel.ConsoleMode[0])
                    {
                        if (string.IsNullOrEmpty(loginInUbcfg))
                        {
                            Methods.LoginMethods.CliLogin(loginInUbcfgSplit);
                        }
                    }
                    else if (Kernel.SelectedConsoleMode == Kernel.ConsoleMode[1])
                    {
                        if (string.IsNullOrEmpty(loginInUbcfg))
                        {
                            if (Methods.LoginMethods.CuiLogin(loginInUbcfgSplit) is false)
                            {
                                WriteLine("An error has occurred", ConsoleColor.Red);
                            }
                            else
                            {
                                WriteLine("login > OK");
                            }


                        }
                    }
                    else
                    {
                        MethodsInfo.ConsoleInfo("The configuration file exists, \n" +
                                                "but it was not possible to read or match its contents. \n" +
                                                "Logged is false by default");
                    }
                }
                else if (!File.Exists(@"0:\system\login.ubcfg")) // Actions if the config with user data was not generated or would be deleted
                {
                    Methods.CreateUser();
                }
            }
            catch (Exception e)
            {
                /*if (!String.IsNullOrEmpty(e.Message))
                {
                    var error = e;
                    // TODO
                    // * write in log file
                }*/
                return false;
            }

            return true;
        }

        #endregion

        #region Create system directories function

        /// <summary>
        ///     Creating system directories
        /// </summary>
        /// <returns>Status of function execution</returns>
        private static bool CreateSystemDirectories()
        {
            try
            {
                if (!Directory.Exists(@"0:\system"))
                {
                    Kernel.Vfs.CreateDirectory(@"0:\system");
                }
                else if (!Directory.Exists(@"0:\users"))
                {
                    Kernel.Vfs.CreateDirectory(@"0:\users");
                }
                else if (Directory.Exists(@"0:\system"))
                {
                    return true;
                }
                else if (Directory.Exists(@"0:\users"))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Crush.CrushScreen(e);
                return false;
            }

            return true;
        }

        #endregion

        #region Select console mode function

        /// <summary>
        ///     Setting console mode
        /// </summary>
        /// <returns>Status of function execution</returns>
        private static bool SelectMode()
        {
            try
            {
                if (!File.Exists(@"0:\system\mode.ubcfg"))
                {
                    MethodsInfo.ConsoleInfo("Select console mode: ");
                    WriteLine("\n[ 1 ] CLI mode\n[ 2 ] CUI mode");
                    System.Console.Write("> ");
                    var key = System.Console.ReadLine();

                    #region mode selection

                    if (!string.IsNullOrEmpty(key))
                    {
                        switch (key)
                        {
                            #region selected CLI mode

                            case "1":
                            {
                                Kernel.SelectedConsoleMode = Kernel.ConsoleMode[0];
                                WriteLine("You selected 'CLI mode', save this solution for future runs?\n");
                                Write("y/N: ");
                                var confirmation = System.Console.ReadLine();
                                if (!string.IsNullOrEmpty(confirmation))
                                {
                                    if (confirmation.ToLower() is "y" or "yes")
                                    {
                                        File.WriteAllText(@"0:\system\mode.ubcfg", "CLIMode");
                                    }
                                }
                                else
                                {
                                    WriteLine("'No' is selected by default");
                                }

                                break;
                            }

                            #endregion selected CLI mode

                            #region selected CUI mode

                            case "2":
                            {
                                Kernel.SelectedConsoleMode = Kernel.ConsoleMode[1];
                                WriteLine("You selected 'CUI mode', save this solution for future runs?\n");
                                Write("y/N: ");
                                var confirmation = System.Console.ReadLine();
                                if (!string.IsNullOrEmpty(confirmation))
                                    switch (confirmation.ToLower())
                                    {
                                        case "y":
                                        case "yes":
                                            File.WriteAllText(@"0:\system\mode.ubcfg", "CUIMode");
                                            break;
                                    }
                                else WriteLine("'No' is selected by default");

                                break;
                            }

                            #endregion selected CUI mode
                        }
                    }
                    else // If a person has not entered anything
                    {
                        Kernel.SelectedConsoleMode = Kernel.ConsoleMode[0];
                        WriteLine("'CLI mode' is selected by default");
                    }

                    #endregion
                }
                else
                {
                    var consoleModeInUbcfg =
                        File.ReadAllText(@"0:\system\mode.ubcfg"); // Reading console mode strings from a file

                    #region reading a mode from a file

                    if (!string.IsNullOrEmpty(consoleModeInUbcfg))
                    {
                        if (consoleModeInUbcfg is "CLI")
                        {
                            // TODO
                        }
                    }
                    else // If the line you read is empty or null
                    {
                        Kernel.SelectedConsoleMode = Kernel.ConsoleMode[0];
                        MethodsInfo.ConsoleInfo("The configuration file exists, \n" +
                                                "but it was not possible to read or match its contents. \n" +
                                                "CLI mode is selected by default");
                    }

                    #endregion reading a mode from a file END
                }
            }
            catch (Exception e)
            {
                Crush.CrushScreen(e);
                return false;
            }

            return true;
        }

        #endregion Select console mode function END

        #endregion Functions for Easy install END
    }



    /*public static int CheckConsoleColorId(string colorIdString)
    {
        int color;
        if (colorIdString.Trim(' ') is "black")
        {
            color = 0;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 1;
            return color;
        }            
        if (colorIdString.Trim(' ') is "black")
        {
            color = 2;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 3;
            return color;
        }            
        if (colorIdString.Trim(' ') is "black")
        {
            color = 4;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 5;
            return color;
        }            if (colorIdString.Trim(' ') is "black")
        {
            color = 6;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 7;
            return color;
        }            
        if (colorIdString.Trim(' ') is "black")
        {
            color = 8;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 9;
            return color;
        }            
        if (colorIdString.Trim(' ') is "black")
        {
            color = 10;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 11;
            return color;
        }            if (colorIdString.Trim(' ') is "black")
        {
            color = 12;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 13;
            return color;
        }            
        if (colorIdString.Trim(' ') is "black")
        {
            color = 14;
            return color;
        }
        if (colorIdString.Trim(' ').ToLower() is "darkblue")
        {
            color = 15;
        }

        return color;
    }*/


    /*internal class ColsoleColorId
    {
        public static int Black = 0;
        public static int DarkBlue = 1;
        public static int DarkGreen = 2;
        public static int DarkCyan = 3;
        public static int DarkRed = 4;
        public static int DarkMagenta = 5;
        public static int DarkYellow = 6;
        public static int Gray = 7;
        public static int DarkGray = 8;
        public static int Blue = 9;
        public static int Green = 10;
        public static int Cyan = 11;
        public static int Red = 12;
        public static int Magenta = 13;
        public static int Yellow = 14;
        public static int White = 15;
    }*/
}

