using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Umbrella.Libraries;
using Umbrella.MainSystem.Console;

namespace Umbrella.MainSystem
{
    internal class Methods
    {
        /// <summary>
        /// The command that creates the directory in the current directory
        /// </summary>
        public static class MkdirCommand
        {
            public static bool Execute(List<string> arguments)
            {
                try
                {
                    if (!arguments[0].Contains('.') && !arguments[0].Contains(@"\") && !arguments[0].Contains("/") &&
                        !arguments[0].Contains(':') && !arguments[0].Contains('*') && !arguments[0].Contains('?') &&
                        !arguments[0].Contains('<') && !arguments[0].Contains('>') && !arguments[0].Contains('|') &&
                        !arguments[0].Contains('+'))
                    {
                        if (Directory.Exists(arguments[0]))
                        {
                            System.Console.WriteLine("That path exists already.");
                        }

                        if (!Directory.Exists(arguments[0]))
                        {
                            Directory.CreateDirectory(@$"{Kernel.CurrentDirectory}{arguments[0]}");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine(
                            "The following characters are prohibited in the names of folders and files:\n" +
                            @"'-' '.' '\' '/' ':' '*' '?' '<' '>' '|' '+'");
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Command to configure the system
        /// </summary>
        public static class StCommand
        {
            public static bool Execute(List<string> arguments)
            {
                try
                {
                    if (arguments.Count > 0 && arguments[0] is "set")
                    {
                        if (arguments.Count > 1 && arguments[1] is "showCompletionStatus")
                        {
                            if (arguments.Count > 2 && arguments[1] is "true")
                            {
                                if (arguments.Count > 2 && arguments[2] is "default")
                                {
                                    if (arguments.Count is 3)
                                    {
                                        MethodsInfo.ConsoleWarning("Enter the full path to the file where you want to save this value, with `0:\\`");
                                    }

                                    if (arguments.Count is 4)
                                    {
                                        File.WriteAllText(arguments[3], "1");
                                    }
                                }
                                Config.ShowCompletionStatus = true;
                            }
                            else if (arguments.Count > 2 && arguments[1] is "false")
                            {
                                Config.ShowCompletionStatus = false;
                            }
                            else if (arguments.Count > 2 && arguments[1] is "-av")
                            {
                                SnowHelp("st set showCompletionStatus [STATUS]",
                                    "This command sets the output of messages informing the user about the execution status of the command, \n" +
                                    "if disabled the execution status messages will not be displayed in the console",
                                    "true, false");
                            }
                            else if (arguments.Count is 2)
                            {
                                WriteMethods.WriteLine("Use 'st set showCompletionStatus -av' \n" +
                                                       "to display possible settings for this area");
                            }
                            
                        }

                        if (arguments.Count > 1 && arguments[1] is "console")
                        {
                            if (arguments.Count > 2 && arguments[3] is "mode")
                            {
                                if (arguments.Count > 3 && arguments[4] is "cli")
                                {
                                    if (arguments.Count > 4 && arguments[5] is "default")
                                    {
                                        if (arguments.Count is 5)
                                        {
                                            MethodsInfo.ConsoleWarning("Enter the full path to the file where you want to save this value, with `0:\\`");
                                        }
                                        else if (arguments.Count is 6 && (Directory.Exists(Path.GetDirectoryName(arguments[6])) && File.Exists(arguments[6])))
                                        {
                                            File.WriteAllText(arguments[6], "CLIMode");
                                        }
                                    }
                                    else if (arguments.Count > 4 && arguments[5] is "-av")
                                    {
                                        SnowHelp("st set console mode cli [OPTION]", "This command sets the Command Line Interface console mode", "default");
                                    }
                                    else
                                    {
                                        Kernel.SelectedConsoleMode = Kernel.ConsoleMode[0];
                                    }

                                }
                                if (arguments.Count > 3 && arguments[4] is "cui")
                                {
                                    if (arguments.Count > 4 && arguments[5] is "default")
                                    {
                                        if (arguments.Count is 5)
                                        {
                                            MethodsInfo.ConsoleWarning("Enter the full path to the file where you want to save this value, with `0:\\`");
                                        }
                                        else if (arguments.Count is 6)
                                        {
                                            if (Directory.Exists(Path.GetDirectoryName(arguments[6])) && File.Exists(Kernel.CurrentDirectory + arguments[6]))
                                            {
                                                File.WriteAllText(arguments[6], "CUIMode");
                                            }
                                        }
                                    } 
                                    else if (arguments.Count > 4 && arguments[5] is "-av")
                                    {
                                        SnowHelp("st set console mode cui [OPTION]", "This command sets the Character User Interface console mode", "default");
                                    }
                                    else
                                    {
                                        Kernel.SelectedConsoleMode = Kernel.ConsoleMode[1];
                                    }
                                }
                                if (arguments.Count > 3 && arguments[4] is "-av")
                                {
                                    SnowHelp("st set console mode [OPTION]", "This area is for setting one of the two console modes: CLI or CUI", "cui, cli");
                                }
                                if (arguments.Count is 3)
                                {
                                    WriteMethods.WriteLine("Use 'st set console mode -av' to display possible settings for this area");
                                }
                            }

                            if (arguments.Count > 2 && arguments[3] is "test")
                            {
                                WriteMethods.WriteLine("Its work");
                            }
                            if (arguments.Count > 2 && arguments[3] is "-av")
                            {
                                SnowHelp("st set console [OPTION]", "This area has console settings", "mode, test");
                            }
                            if (arguments.Count is 2)
                            {
                                WriteMethods.WriteLine("Use 'set mode -av' to display possible settings for this area");
                            }

                            #region Not use

                            /*if (arguments.Count > 2 && arguments[3] is "backcolor")
                            {
                                if (arguments.Count > 4 && arguments[5] is "default")
                                {
                                    switch (arguments.Count)
                                    {
                                        case 5:
                                            MethodsInfo.ConsoleWarning("Type the name of the color for the background.");
                                            goto input;
                                        case 6:
                                        {
    
    
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    var color = arguments[5];
                                    System.Console.BackgroundColor = (ConsoleColor)color;
                                }
                            }*/

                            #endregion
                        }

                        if (arguments.Count > 1 && arguments[1] is "-av")
                        {
                            SnowHelp("st set [OPTION]", 
                                "This option sets the settings for different objects, \n" + 
                                "to get help for it, type `st set [OPTION] -av`", 
                                "showCompletionStatus, console");
                        }

                        if (arguments.Count is 1)
                        {
                            WriteMethods.WriteLine("Use 'st set -av' to display possible settings for this area");
                        }
                    }
                    return true;
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
            }
        }

        public static class LoginMethods
        {
            public static bool CliLogin(string[] loginInUbcfgSplit)
            {
                try
                {
                    login:
                    MethodsInfo.ConsoleInfo("LOGGING IN\n");
                    WriteMethods.Write("Username: ");
                    var inputUsername = System.Console.ReadLine();
                    WriteMethods.Write("Password: ");
                    var inputPassword = System.Console.ReadLine();
                    if (!string.IsNullOrEmpty(inputUsername) && !string.IsNullOrEmpty(inputUsername))
                    {
                        if (inputUsername != loginInUbcfgSplit[0] || inputPassword != loginInUbcfgSplit[1])
                        {
                            WriteMethods.WriteLine("Username or password is incorrect, try again");
                            goto login;
                        }

                        Kernel.CurrentUser = inputUsername;
                    }
                    else
                    {
                        WriteMethods.WriteLine("Enter login and password, I do not need blank lines");
                        goto login;
                    }

                    return true;
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
            }

            public static bool CuiLogin(string[] loginInUbcfgSplit)
            {
                try
                {
                    Screen.ClearScreen(ConsoleColor.Black, ConsoleColor.White);
                    Box.Login(loginInUbcfgSplit[0], loginInUbcfgSplit[1], x: 20, y: 10); // Libraries/CAL/Box.Login (Input form output)
                    return true;
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
            }
        }

        public static bool CreateUser()
        {
            try
            {
                WriteMethods.WriteLine("\n\n---------------------------");
                WriteMethods.WriteLine("\nUser creation\n", ConsoleColor.Yellow);
                
                WriteMethods.WriteLine("Username: ");
                var newLogin = System.Console.ReadLine();
                
                WriteMethods.WriteLine($"Password for {newLogin}: ");
                var newPassword = System.Console.ReadLine();
                
                WriteMethods.WriteLine($"Username is {newLogin} and the password for it {newPassword}?\n", ConsoleColor.Yellow);
                WriteMethods.Write("y/N: ");
                var confirmation = System.Console.ReadLine();

                if (confirmation.ToLower() is "y" or "yes")
                {
                    File.WriteAllText(@"0:\system\login.ubcfg", $"{newLogin} {newPassword}");
                    Kernel.CurrentUser = newLogin;
                    MethodsInfo.ConsoleOk("User created!");
                }
                else if (confirmation.ToLower() is "n" or "no")
                {
                    MethodsInfo.ConsoleWarning("You cancel creation user");
                }
                else
                {
                    MethodsInfo.ConsoleWarning("User creation is cancelled by default");
                }

                return true;
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

        }

        /// <summary>
        /// This function outputs help for using the command
        /// </summary>
        /// <param name="usage"></param>
        /// <param name="description"></param>
        /// <param name="options"></param>
        public static void SnowHelp(string usage, string description, string options)
        {
            WriteMethods.WriteLine("  USAGE:", ConsoleColor.Blue);
            WriteMethods.WriteLine(usage);
            WriteMethods.WriteLine("  DESCRIPTION:", ConsoleColor.Blue);
            WriteMethods.WriteLine(description);
            WriteMethods.WriteLine("  OPTIONS:", ConsoleColor.Blue);
            WriteMethods.WriteLine(options);
        }

        /// <summary>
        /// This method parses arguments from cmd line
        /// </summary>
        /// <param name="cmdLine"></param>
        /// <returns>Array with all arguments, counting the command name</returns>
        public static List<string> ParseCommandLine(string cmdLine)
        {
            var args = new List<string>();
            if (string.IsNullOrWhiteSpace(cmdLine)) return args;

            var currentArg = new StringBuilder();
            var inQuotedArg = false;

            foreach (var t in cmdLine)
            {
                switch (t)
                {
                    case '"' when inQuotedArg:
                        args.Add(currentArg.ToString());
                        currentArg = new StringBuilder();
                        inQuotedArg = false;
                        break;
                    case '"':
                        inQuotedArg = true;
                        break;
                    case ' ' when inQuotedArg:
                        currentArg.Append(t);
                        break;
                    case ' ':
                    {
                        if (currentArg.Length > 0)
                        {
                            args.Add(currentArg.ToString());
                            currentArg = new StringBuilder();
                        }

                        break;
                    }
                    default:
                        currentArg.Append(t);
                        break;
                }
            }

            if (currentArg.Length > 0) args.Add(currentArg.ToString());

            return args;
        }
    }
}
