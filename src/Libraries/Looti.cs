using System;
using System.Globalization;
using System.IO;
using Cosmos.System;
using Console = System.Console;
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace Umbrella.Libraries
{
    public class Looti
    {
        public static bool Beep = true;
        public static bool Ide;
        public static bool Hello = true;
        public static int Xint;
        public static bool Click;
        public static bool Lockf;
        public static bool Wrap = true;
        private static string _tosav;
        private static string _cursor = " ";
        private static readonly double Ver = 1.1;

        public static string Run(string[] argv)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Editor(argv);
            Console.CursorVisible = true;
            Console.CursorSize = 16;
            Console.Clear();
            return null;
        }

        public static string Editor(string[] argv)
        {
            Console.CursorVisible = false;
            var path = argv[0];
            if (!File.Exists(@"0:\looti.scf"))
            {
                File.Create(@"0:\looti.scf");
                File.AppendAllText("looti.scf", "beep=yes\nhello=yes\nwraplines=yes\nIDE=no");
                Beep = true;
                Hello = true;
            }
            else
            {
                var scf = File.ReadAllLines(@"0:\looti.scf");
                for (var i = 0; i < scf.Length; i++)
                {
                    if (scf[i] == "beep=yes") Beep = true;
                    if (scf[i] == "beep=no") Beep = false;
                    if (scf[i] == "hello=yes") Hello = true;
                    if (scf[i] == "hello=no") Hello = false;
                    if (scf[i] == "click=yes") Click = true;
                    if (scf[i] == "click=no") Click = false;
                    if (scf[i] == "wraplines=yes") Wrap = true;
                    if (scf[i] == "wraplines=no") Wrap = false;
                    if (scf[i] == "IDE=yes") Ide = true;
                    if (scf[i] == "IDE=no") Ide = false;
                    if (scf[i].StartsWith("cursor="))
                    {
                        scf[i] = scf[i].Remove(0, 7);
                        if (scf[i].Length == 1) _cursor = scf[i];
                    }
                }
            }

            path = path.ToLower();
            Console.Clear();
            for (var i = 1; i < argv.Length; i++)
                if (argv[i] == "--help")
                {
                    Console.WriteLine("Looti Perfect " + Ver + ", MIT license.\nhttps://github.com/luftkatzeBASIC/\nLooti " +
                                      Ver + ", 2021 Luftkatze\n");
                    Console.CursorVisible = true;
                    return null;
                }
                else if (argv[i] == "--lock")
                {
                    Lockf = true;
                }
                else if (argv[i] == "--ide")
                {
                    Ide = true;
                }
                else
                {
                    Console.Write("\nUnknown argument - ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(argv[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.CursorVisible = true;
                    return null;
                }

            if (path == "conf") path = @"0:\Looti.scf";
            if (!File.Exists(path)) File.Create(path);
            if (Hello) DrawLogo();
            Console.BackgroundColor = ConsoleColor.Blue;
            var arrow = "";
            var old = File.ReadAllText(Directory.GetCurrentDirectory() + path);
            _tosav = File.ReadAllText(Directory.GetCurrentDirectory() + path);
            for (;;)
            {
                DrawBar(path, _tosav, arrow, old);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(_tosav);
                Xint = Console.CursorLeft;
                var yint = Console.CursorTop;
                Console.Write(arrow);
                Console.SetCursorPosition(Xint, yint);
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(_cursor);
                Xint = Console.CursorLeft;
                yint = Console.CursorTop;
                Console.SetCursorPosition(Xint, yint);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Gray;
                var input = new ConsoleKeyInfo();
                if (Lockf == false)
                {
                    input = Console.ReadKey(true);
                }
                else if (Lockf)
                {
                    Console.SetCursorPosition(0, 24);
                    Console.Write("[Document locked] - Press INSERT to exit");
                    for (;;)
                    {
                        var lockv = Console.ReadKey().Key;
                        if (lockv == ConsoleKey.Insert)
                        {
                            Console.CursorVisible = true;
                            return null;
                        }

                        Console.CursorLeft--;
                    }
                }

                if (Click) Console.Beep(900, 35);
                if (_tosav.Contains("%DATE%"))
                {
                    var dt = DateTime.Now;
                    var d = dt.ToShortDateString();
                    _tosav = _tosav.Replace("%DATE%", d);
                }
                else if (_tosav.Contains("%TIME%"))
                {
                    var dt = DateTime.Now;
                    var t = dt.ToShortTimeString();
                    _tosav = _tosav.Replace("%TIME%", t);
                }
                else if (_tosav.Contains("%NOW%"))
                {
                    var dt = DateTime.Now;
                    _tosav = _tosav.Replace("%NOW%", dt.ToString(CultureInfo.InvariantCulture));
                }

                if (input.Key == ConsoleKey.Enter)
                {
                    _tosav += "\n";
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.S) != 0)
                    {
                        File.Delete(path);
                        File.AppendAllText(path, _tosav + arrow);
                        old = _tosav + arrow;
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.O) != 0)
                    {
                        var open = Open();
                        if (open != null && File.Exists(open))
                        {
                            _tosav = File.ReadAllText(open);
                            arrow = "";
                            path = open;
                        }
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.N) != 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.WriteLine("[New file name]");
                        Console.SetCursorPosition(20, 14);
                        var newflnam = LootiTerminal();
                        if (newflnam != null)
                        {
                            _tosav = File.ReadAllText(newflnam);
                            arrow = "";
                            path = newflnam;
                        }
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.Q) != 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("[Save as...]");
                        Console.SetCursorPosition(20, 14);
                        var savas = LootiTerminal();
                        if (savas != null)
                        {
                            File.AppendAllText(savas, old);
                            path = savas;
                        }
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.H) != 0) Replace(_tosav, old, arrow, path);
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.F) != 0) Find(_tosav, old, arrow, path);
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.X) != 0)
                    {
                        Exit(_tosav, old, arrow, path);
                        Console.CursorVisible = true;
                        return null;
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.K) != 0)
                    {
                        if (path.EndsWith(".TXT")) path = path.Replace(".TXT", ".LTI");
                        else if (path.EndsWith(".LTI")) path = path.Replace(".LTI", ".TXT");
                        else if (path.EndsWith(".DOC")) path = path.Replace(".DOC", ".LTI");
                    }
                }
                else if (input.Key == ConsoleKey.F1)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(0, 2);
                    Console.Write("| File            \n");
                    Console.Write("| F1  [SAVE]     S\n");
                    Console.Write("| F2  [OPEN]     O\n");
                    Console.Write("| F3  [NEW]      N\n");
                    Console.Write("| F4  [SAVE AS]  Q\n");
                    Console.Write("| F12 [EXIT]     X");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.F1)
                    {
                        File.Delete(path);
                        File.AppendAllText(path, _tosav + arrow);
                        old = _tosav + arrow;
                    }
                    else if (input.Key == ConsoleKey.F2)
                    {
                        var open = Open();
                        if (open != null && File.Exists(open))
                        {
                            _tosav = File.ReadAllText(open);
                            arrow = "";
                            path = open;
                        }
                    }
                    else if (input.Key == ConsoleKey.F3)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.WriteLine("[New file name]");
                        Console.SetCursorPosition(20, 14);
                        var newflnam = LootiTerminal();
                        if (newflnam != null)
                        {
                            _tosav = File.ReadAllText(newflnam);
                            arrow = "";
                            path = newflnam;
                        }
                    }
                    else if (input.Key == ConsoleKey.F4)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("[Save as...]");
                        Console.SetCursorPosition(20, 14);
                        var savas = LootiTerminal();
                        if (savas != null)
                        {
                            File.AppendAllText(savas, old);
                            path = savas;
                        }
                    }
                    else if (input.Key == ConsoleKey.F12)
                    {
                        Exit(_tosav, old, arrow, path);
                        Console.CursorVisible = true;
                        return null;
                    }
                    else
                    {
                        DrawBar(path, _tosav, arrow, old);
                    }
                }
                else if (input.Key == ConsoleKey.F2)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(8, 2);
                    Console.Write("| Edit           \n");
                    Console.SetCursorPosition(8, 3);
                    Console.Write("| F1  [REPLACE] H\n");
                    Console.SetCursorPosition(8, 4);
                    Console.Write("| F2  [CONVERT] K\n");
                    Console.SetCursorPosition(8, 5);
                    Console.Write("| F3  [FIND]    F\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.F2)
                    {
                        if (path.EndsWith(".TXT")) path = path.Replace(".TXT", ".LTI");
                        else if (path.EndsWith(".LTI")) path = path.Replace(".LTI", ".TXT");
                        else if (path.EndsWith(".DOC")) path = path.Replace(".DOC", ".LTI");
                    }
                    else if (input.Key == ConsoleKey.F1)
                    {
                        Replace(_tosav, old, arrow, path);
                    }
                    else if (input.Key == ConsoleKey.F3)
                    {
                        Find(_tosav, old, arrow, path);
                    }
                    else
                    {
                        DrawBar(path, _tosav, arrow, old);
                    }
                }
                else if (input.Key == ConsoleKey.F3)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(19, 2);
                    Console.Write("| Other          \n");
                    Console.SetCursorPosition(19, 3);
                    Console.Write("| F1   [CREATE]   \n");
                    Console.SetCursorPosition(19, 4);
                    Console.Write("| F2   [ABOUT]   \n");
                    Console.SetCursorPosition(19, 5);
                    Console.Write("| F3   [PATH]    \n");
                    Console.SetCursorPosition(19, 6);
                    Console.Write("| F4   [CONF]    \n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.F1) return null;
                    if (input.Key == ConsoleKey.F2)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.WriteLine("Looti text editor" + "\nVersion" + Ver + "\nCreated by:\n" +
                                          "Programming: Luftkatze\n" + "Name: BaseMax\n" + "Few ideas: Ilobilo\n" +
                                          "2021, Luftkatze\n\n\n\n" + "Default Looti file extension: * .LTI\n");
                        Console.ReadKey();
                    }
                    else if (input.Key == ConsoleKey.F3)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(20, 14);
                        Console.WriteLine("[New path...]");
                        var np = LootiTerminal();
                        if (np != null)
                        {
                            File.AppendAllText(np, old);
                            path = np;
                        }
                    }
                    else if (input.Key == ConsoleKey.F4)
                    {
                        if (Exit(_tosav, old, arrow, path) != null)
                        {
                            _tosav = File.ReadAllText(@"0:\looti.scf");
                            old = _tosav;
                            arrow = "";
                        }
                    }
                    else
                    {
                        DrawBar(path, _tosav, arrow, old);
                    }
                }
                else if (input.Key == ConsoleKey.F4)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(29, 2);
                    Console.Write("| Insert          \n");
                    Console.SetCursorPosition(29, 3);
                    Console.Write("| F1  [NOW]       \n");
                    Console.SetCursorPosition(29, 4);
                    Console.Write("| F2  [TIME]      \n");
                    Console.SetCursorPosition(29, 5);
                    Console.Write("| F3  [DATE]      \n");
                    Console.SetCursorPosition(29, 6);
                    Console.Write("| F4  [FILE]      \n");
                    Console.SetCursorPosition(29, 7);
                    Console.Write("| F5  [ASCII]     \n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.F1)
                    {
                        var dt = DateTime.Now;
                        _tosav += dt.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (input.Key == ConsoleKey.F2)
                    {
                        var dt = DateTime.Now;
                        var t = dt.ToShortTimeString();
                        _tosav += t;
                    }
                    else if (input.Key == ConsoleKey.F3)
                    {
                        var dt = DateTime.Now;
                        var d = dt.ToShortDateString();
                        _tosav += d;
                    }
                    else if (input.Key == ConsoleKey.F4)
                    {
                        var open = Open();
                        if (open != null)
                        {
                            if (File.Exists(open))
                            {
                                var toconcat = File.ReadAllText(open);
                                _tosav += toconcat;
                            }
                            else
                            {
                                DrawBar(path, _tosav, arrow, old);
                                Console.SetCursorPosition(20, 13);
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("CANNOT FIND FILE!");
                                Console.SetCursorPosition(27, 13);
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.ReadKey();
                                DrawBar(path, _tosav, arrow, old);
                            }
                        }
                    }
                    else if (input.Key == ConsoleKey.F5)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        DrawBar(path, _tosav, arrow, old);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(_tosav);
                        Console.Write(arrow);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(20, 13);
                        Console.Write("CHAR [                              ]");
                        Console.SetCursorPosition(26, 13);
                        var chr = LootiTerminal();
                        int.TryParse(chr, out var ascii);
                        _tosav += (char) ascii;
                        DrawBar(path, _tosav, arrow, old);
                    }
                    else
                    {
                        DrawBar(path, _tosav, arrow, old);
                    }
                }
                else if (input.Key == ConsoleKey.Home)
                {
                    Console.ResetColor();
                    Console.Clear();
                    Console.CursorVisible = true;
                    return null;
                }
                else if (input.Key == ConsoleKey.Escape)
                {
                    Console.CursorLeft--;
                }
                else if (input.Key == ConsoleKey.Tab)
                {
                    _tosav += "\t";
                }
                else if (input.Key == ConsoleKey.Spacebar)
                {
                    _tosav += " ";
                }
                else if (input.KeyChar == '{')
                {
                    _tosav += "{";
                    if (Ide) _tosav += "\n";
                    arrow = "\n}" + arrow;
                }
                else if (input.KeyChar == '(')
                {
                    _tosav += "(";
                    if (Ide) arrow = ")" + arrow;
                }
                else if (input.KeyChar == '<')
                {
                    _tosav += "<";
                    if (Ide) arrow = ">" + arrow;
                }
                else if (input.KeyChar == '"')
                {
                    _tosav += "\"";
                    if (Ide) arrow = "\"" + arrow;
                }
                else if (input.KeyChar == '\'')
                {
                    _tosav += "'";
                    if (Ide) arrow = "'" + arrow;
                }
                else if (input.KeyChar == '[')
                {
                    _tosav += "[";
                    if (Ide) arrow = "]" + arrow;
                }
                else if (input.Key == ConsoleKey.Backspace || input.Key == ConsoleKey.Delete)
                {
                    var back = _tosav;
                    if (back.Length != 0)
                    {
                        var del = back.Length;
                        back = back.Remove(del - 1, 1);
                        _tosav = back;
                    }
                }
                else if (input.Key == ConsoleKey.Insert)
                {
                    Console.SetCursorPosition(0, 24);
                    Console.Write("[Document locked]");
                    for (;;)
                    {
                        var lockv = Console.ReadKey().Key;
                        if (lockv == ConsoleKey.Insert) break;
                        Console.CursorLeft--;
                    }
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {
                    if (_tosav.Length > 0)
                    {
                        arrow = _tosav[^1] + arrow;
                        _tosav = _tosav.Remove(_tosav.Length - 1, 1);
                    }
                    else if (_tosav.EndsWith("\n"))
                    {
                        arrow = "\n" + arrow;
                        _tosav = _tosav.Remove(_tosav.Length - 1, 1);
                    }
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length > 0)
                {
                    _tosav += arrow[0];
                    arrow = arrow.Remove(0, 1);
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length == 0)
                {
                    Console.CursorLeft--;
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    if (_tosav.Length != 0 && Console.CursorTop != 2)
                        if (_tosav.Contains("\n"))
                            for (;;)
                            {
                                arrow = _tosav[^1] + arrow;
                                _tosav = _tosav.Remove(_tosav.Length - 1, 1);
                                if (arrow.StartsWith("\n")) break;
                            }
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    if (arrow.Contains("\n"))
                        for (;;)
                        {
                            _tosav += arrow[0];
                            arrow = arrow.Remove(0, 1);
                            if (_tosav.EndsWith("\n")) break;
                        }
                }
                else if (input.Key == ConsoleKey.PageUp)
                {
                    if (_tosav.Length != 0)
                    {
                        arrow = _tosav + arrow;
                        _tosav = "";
                    }
                }
                else if (input.Key == ConsoleKey.PageDown)
                {
                    if (arrow.Length != 0)
                    {
                        _tosav += arrow;
                        arrow = "";
                    }
                }
                else if (input.Key == ConsoleKey.Escape && Console.CursorLeft != 0)
                {
                    Console.CursorLeft--;
                }
                else if (input.Key == ConsoleKey.F12)
                {
                    File.Delete(path);
                    File.AppendAllText(path, _tosav + arrow);
                    old = _tosav + arrow;
                }
                else
                {
                    _tosav += input.KeyChar.ToString();
                    if (Wrap)
                        if (Console.CursorLeft == 79 && _tosav.Contains(" "))
                        {
                            var lastIndex = 0;
                            var lio = _tosav;
                            for (var i = 0; i < lio.Length; i++)
                                if (lio[i] == ' ')
                                    lastIndex = i;
                            _tosav = _tosav.Remove(lastIndex, " ".Length).Insert(lastIndex, "\n");
                        }
                }
            }
        }

        public static void DrawBar(string path, string tosav, string arrow, string old)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 0);
            for (var i = 0; i < 80; i++) Console.Write(" ");
            Console.SetCursorPosition(0, 0);
            Console.Write("Looti Perfect - " + path);
            if (old != tosav + arrow) Console.Write("*");
            Console.Write("\n");
            Console.SetCursorPosition(35, 0);
            var tolenght = tosav.Replace("\n", "") + arrow.Replace("\n", "");
            var lines = tosav.Split('\n').Length + arrow.Split('\n').Length;
            lines--;
            var tarr = tosav.Split('\n');
            var t = tarr[^1];
            var pos = tosav.Split('\n').Length - 1 + "," + t.Length;
            Console.Write("Pos: [" + pos + "]  |  Lines: [" + lines + "] Chars: [" + tolenght.Length + "]");
            Console.SetCursorPosition(0, 1);
            for (var i = 0; i < 80; i++) Console.Write(" ");
            Console.SetCursorPosition(0, 1);
            Console.Write("F1 FILE | F2 EDIT | F3 OTHER | F4 INSERT");
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void Replace(string tosav, string old, string arrow, string path)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            DrawBar(path, tosav, arrow, old);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(tosav);
            Console.Write(arrow);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(20, 13);
            Console.Write("Old [                          ]");
            Console.SetCursorPosition(20, 14);
            Console.Write("New [                          ]");
            Console.SetCursorPosition(25, 13);
            var oldrep = LootiTerminal();
            if (oldrep != null)
            {
                Console.SetCursorPosition(25, 14);
                var newrep = LootiTerminal();
                if (newrep != null)
                {
                    if (tosav.Contains(oldrep)) tosav = tosav.Replace(oldrep, newrep);
                    if (arrow.Contains(oldrep))
                    {
                        arrow = arrow.Replace(oldrep, newrep);
                    }
                    else
                    {
                        DrawBar(path, tosav, arrow, old);
                        Console.SetCursorPosition(25, 8);
                        Console.WriteLine("CANNOT FIND WORD TO REPLACE!");
                        Console.ReadKey();
                    }
                }
            }

            DrawBar(path, tosav, arrow, old);
        }

        private static string Exit(string tosav, string old, string arrow, string path)
        {
            if (tosav != old)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.SetCursorPosition(20, 13);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("[Do you want to save changes? Y/N ]");
                Console.SetCursorPosition(20, 14);
                var answer = Console.ReadKey().KeyChar.ToString();
                var shouldSave = answer.ToLower() == "y";
                if (shouldSave == false)
                {
                    File.Delete(path);
                    File.AppendAllText(path, old);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ResetColor();
                    Console.Clear();
                    Console.CursorVisible = true;
                    return null;
                }

                File.Delete(path);
                File.AppendAllText(path, tosav + arrow);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = true;
                return null;
            }
            else
            {
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = true;
                return null;
            }
        }

        public static void DrawLogo()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\n\n\n\n\n");
            Console.WriteLine("        &@@@@,                                                          (@@@");
            Console.WriteLine("        @@@@@                                               @@@@@       &@@@");
            Console.WriteLine("       @@@@@.                                              ,@@@@(           ");
            Console.WriteLine("      .@@@@@           (@@@@@@@@@@%      *@@@@@@@@@@&    @@@@@@@@@@@  #@@@@.");
            Console.WriteLine("      @@@@@.         #@@@@@   .@@@@@   *@@@@@    @@@@@    *@@@@/      @@@@@");
            Console.WriteLine("     .@@@@&         #@@@@/      #@@@@.*@@@@%     /@@@@*   @@@@@      &@@@@.");
            Console.WriteLine("     @@@@@          @@@@@       @@@@@ &@@@@      @@@@@   *@@@@*      @@@@&");
            Console.WriteLine("    .@@@@%          @@@@@/   *@@@@@   #@@@@%   .@@@@@    @@@@@      #@@@@,");
            Console.WriteLine("    @@@@@@@@@@@@@.   *@@@@@@@@@@@      .@@@@@@@@@@@.     %@@@@@@@   ,@@@@@@&");
            if (Beep) PCSpeaker.Beep(500, 50);
        }

        private static void Find(string tosav, string old, string arrow, string path)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            DrawBar(path, tosav, arrow, old);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(tosav);
            Console.Write(arrow);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(20, 13);
            Console.Write("FIND [                              ]");
            Console.SetCursorPosition(26, 13);
            var find = LootiTerminal();
            if (find != null)
            {
                if (tosav.Contains(find) || arrow.Contains(find))
                {
                    var tofind = "";
                    if (tosav.Contains(find))
                        for (var i = 0; i < find.Length; i++)
                            tofind += tosav.Replace(find, "!");
                    if (arrow.Contains(find))
                        for (var i = 0; i < find.Length; i++)
                            tofind += arrow.Replace(find, "!");
                    Console.BackgroundColor = ConsoleColor.Blue;
                    DrawBar(path, tofind, arrow, old);
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(tofind);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadKey();
                    DrawBar(path, tofind, arrow, old);
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                DrawBar(path, tosav, arrow, old);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(tosav);
                Console.Write(arrow);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(20, 13);
                Console.Write("CANNOT FIND WORD!");
                Console.SetCursorPosition(27, 13);
                Console.ReadKey();
                DrawBar(path, tosav, arrow, old);
            }
        }

        private static string Open()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.SetCursorPosition(20, 0);
            Console.Write("Files:");
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());
            var numoffiles = files.Length;
            for (var i = 0; i < numoffiles; i++)
            {
                Console.SetCursorPosition(20, i + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(files[i]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("]\n");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("[File to open]");
            Console.SetCursorPosition(0, 1);
            var open = LootiTerminal();
            return open;
        }

        public static string LootiTerminal()
        {
            var toreturn = "";
            for (;;)
            {
                var arrow = "";
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Enter) return toreturn + arrow;
                if (input.Key == ConsoleKey.Backspace)
                {
                    if (toreturn.Length != 0)
                    {
                        Console.CursorLeft--;
                        toreturn = toreturn.Remove(toreturn.Length - 1, 1);
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                    else
                    {
                        Console.CursorLeft++;
                    }
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {
                    if (toreturn.Length > 0)
                    {
                        toreturn = toreturn.Remove(toreturn.Length - 1, 1);
                    }
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length > 0)
                {
                    toreturn += arrow[0];
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length == 0)
                {
                    Console.CursorLeft--;
                }
                else if (input.Key == ConsoleKey.Escape)
                {
                    return null;
                }
                else
                {
                    toreturn += input.KeyChar.ToString();
                }
            }
        }
    }
}