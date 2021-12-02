using System;
using System.IO;
using Umbrella.MainSystem;

namespace Umbrella.Libraries
{
    public class CALUtilities
    {
        public static void FileExplorer(string path)
        {
            if (!path.EndsWith(@"\")) path += @"\";
            for (; ; )
            {
                if (!Directory.Exists(path)) continue;
                var x = 3;
                var f = 0;
                var y = 7;
                var fx = 0;
                var fy = 0;

                Window.DrawWindow("Explorer", 60, 20, 2, 1);

                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(x, y - 3);
                Console.Write("[" + path + "]:");
                Console.SetCursorPosition(x, y - 1);

                var files = Directory.GetFiles(path);
                var dirs = Directory.GetDirectories(path);

                for (var index = 0; index < dirs.Length; index++)
                {
                    var t = dirs[index];
                    var actual = t.Replace(path, "");
                    if (actual.Length > 12)
                    {
                        do
                        {
                            actual = actual.Remove(actual.Length - 1, 1);
                        } while (actual.Length > 10);

                        f++;
                        actual += "~" + f;
                    }

                    actual += @"\";
                    Console.Write(actual);
                    Console.SetCursorPosition(x, y);
                    y++;
                    if (y == 21)
                    {
                        y = 7;
                        x += 16;
                    }

                    fy = Console.CursorTop;
                    fx = Console.CursorLeft;
                }

                for (var index = 0; index < files.Length; index++)
                {
                    var t = files[index];
                    var actual = t.Replace(path, "");
                    if (actual.Length > 12)
                    {
                        var ext = actual.Substring(actual.Length - 4);
                        do
                        {
                            actual = actual.Remove(actual.Length - 1, 1);
                        } while (actual.Length > 8);

                        f++;
                        actual += "~" + f + ext;
                    }

                    Console.Write(actual);
                    Console.SetCursorPosition(fx, fy);
                    fy++;
                    if (fy != 21) continue;
                    fy = 7;
                    fx += 16;
                }

                Console.SetCursorPosition(3, 3);
                var npath = Read.TextBox(50);

                if (npath == "exit")
                {
                    
                    Screen.ClearScreen(ConsoleColor.Black, ConsoleColor.White);
                    CLI.Commands();
                }

                if (!npath.EndsWith(@"\"))
                {
                    npath += @"\";
                }

                if (Directory.Exists(path + npath))
                {
                    path += npath;
                }
                else if (File.Exists(path + npath))
                {
                    Screen.ClearScreen(ConsoleColor.Black, ConsoleColor.White);
                    Looti.Run(npath.ToStringArr());
                }
                else if (npath.Length < 4 && Directory.Exists(npath))
                {
                    path = npath;
                }
                else
                {
                    Box.MsgBox("Err", "Cannot find directory:\n" + path, false, path.Length + 25);
                }
            }
        }

        /*public static void NewFileExplorer(string path)
        {
            if (!path.EndsWith(@"\")) path += @"\";
            else
            {
                #region Draw window
                Window.DrawWindow("Explorer", 40, 20, 2, 1);
                #endregion

                #region Variables
                // Func for get file and get dirs
                var getFiles = Directory.GetFiles(path);
                var getFolders = Directory.GetDirectories(path);

                // idk
                var x = 3;
                var f = 0;
                var y = 7;
                var fx = 0;
                var fy = 0;
                #endregion

                #region Set cursor position and write block for path txt
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(x, y - 3);
                Console.Write("[" + path + "]:");
                Console.SetCursorPosition(x, y - 1);
                Console.SetCursorPosition(3, 3);
                #endregion

                #region Get list of dirs
                foreach (var t in getFolders)
                {
                    var actual = t.Replace(path, "");
                    if (actual.Length > 12)
                    {
                        do
                        {
                            actual = actual.Remove(actual.Length - 1, 1);
                        } while (actual.Length > 10);

                        f++;
                        actual += "~" + f;
                    }

                    actual += @"\";
                    Console.Write(actual);
                    Console.SetCursorPosition(x, y);
                    y++;
                    if (y == 21)
                    {
                        y = 7;
                        x += 16;
                    }

                    fy = Console.CursorTop;
                    fx = Console.CursorLeft;
                }
                #endregion

                #region Get list of file
                foreach (var t in getFiles)
                {
                    var actual = t.Replace(path, "");
                    if (actual.Length > 12)
                    {
                        var ext = actual.Substring(actual.Length - 4);
                        do
                        {
                            actual = actual.Remove(actual.Length - 1, 1);
                        } while (actual.Length > 8);

                        f++;
                        actual += "~" + f + ext;
                    }


                    #region Write actual path
                    Console.Write(actual);
                    Console.SetCursorPosition(fx, fy);
                    fy++;
                    if (fy != 21) continue;
                    fy = 7;
                    fx += 16;
                    #endregion
                }
                #endregion

                #region Path entry and check
                var inputPath = Read.TextBox(65);
                var finallyPath = path + inputPath;

                if (!inputPath.EndsWith(@"\")) inputPath += @"\";
                if (Directory.Exists(finallyPath))
                {
                    path += inputPath;
                } 
                else if (File.Exists(finallyPath))
                {
                    Looti.Run(finallyPath.ToStringArr());
                }
                else
                {
                    Box.MsgBox("Error", "Cannot find directory:\n" + path, false, path.Length + 25);
                }
                #endregion
            }
        }*/
    }
}