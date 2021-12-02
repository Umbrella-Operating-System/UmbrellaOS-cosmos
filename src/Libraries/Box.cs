using System;
using System.IO;

namespace Umbrella.Libraries
{
    public class Box
    {
        public static void ErrorBox(string title, string[] message, int w, int y = 6, int x = 25,
            ConsoleColor back = ConsoleColor.White, ConsoleColor text = ConsoleColor.Red, ConsoleColor bar = ConsoleColor.Red,
            ConsoleColor barText = ConsoleColor.White)
        {
            w += 2;
            var oldback = Console.BackgroundColor;
            var oldfore = Console.ForegroundColor;
            Console.BackgroundColor = bar;
            Console.ForegroundColor = barText;
            Console.SetCursorPosition(x, y);
            for (var i = 0; i < w; i++) Console.Write(" ");
            Console.SetCursorPosition(x + 1, y);
            Console.Write("[" + title + "]");
            Console.BackgroundColor = back;
            Console.ForegroundColor = text;
            var okc = 0;
            for (var i = 0; i < message.Length + 3; i++)
            {
                Console.SetCursorPosition(x, y + 1 + i);
                for (var it = 0; it < w; it++) Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
                Console.BackgroundColor = back;
                okc = Console.CursorTop;
            }

            var yi = y + 2;
            for (var i = 0; i < message.Length; i++)
            {
                Console.ForegroundColor = text;
                Console.SetCursorPosition(x + 1, yi + i);
                if (message[i].Length <= w - 2) Console.Write(message[i]);
                else Console.Write("Msg too long");
            }

            Console.SetCursorPosition(x, okc);
            Menu.WriteMenuOption("OK", text);
            Console.SetCursorPosition(x + 5, okc);
            Console.SetCursorPosition(x + 1, okc + 1);
            Console.BackgroundColor = ConsoleColor.Black;
            for (var it = 0; it < w; it++) Console.Write(" ");
            Console.BackgroundColor = oldback;
            Console.ForegroundColor = oldfore;
            Console.SetCursorPosition(0, Console.CursorTop++);
            Console.Beep(300, 30);
        }

        public static bool TrueFalseBox(string title, string[] message, int w, int y = 6, int x = 25,
            ConsoleColor back = ConsoleColor.Gray, ConsoleColor text = ConsoleColor.Black, ConsoleColor bar = ConsoleColor.Blue,
            ConsoleColor barText = ConsoleColor.White)
        {
            for (;;)
            {
                w += 2;
                var oldback = Console.BackgroundColor;
                var oldfore = Console.ForegroundColor;
                for (;;)
                {
                    Console.BackgroundColor = bar;
                    Console.ForegroundColor = barText;
                    Console.SetCursorPosition(x, y);
                    for (var i = 0; i < w; i++) Console.Write(" ");
                    Console.SetCursorPosition(x + 1, y);
                    Console.Write("[" + title + "]");
                    Console.BackgroundColor = back;
                    Console.ForegroundColor = text;
                    var okc = 0;
                    for (var i = 0; i < message.Length + 3; i++)
                    {
                        Console.SetCursorPosition(x, y + 1 + i);
                        for (var it = 0; it < w; it++) Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                        Console.BackgroundColor = back;
                        okc = Console.CursorTop;
                    }

                    var yi = y + 2;
                    for (var i = 0; i < message.Length; i++)
                    {
                        Console.SetCursorPosition(x + 1, yi + i);
                        if (message[i].Length <= w - 2) Console.Write(message[i]);
                    }

                    Console.SetCursorPosition(x + 5, okc);
                    Menu.WriteMenuOption("Yes", text);
                    Console.SetCursorPosition(x + 12, okc);
                    Menu.WriteMenuOption("No", text);
                    Console.SetCursorPosition(x + 1, okc + 1);
                    Console.BackgroundColor = ConsoleColor.Black;
                    for (var it = 0; it < w; it++) Console.Write(" ");
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.Y)
                    {
                        Console.BackgroundColor = oldback;
                        Console.ForegroundColor = oldfore;
                        Console.SetCursorPosition(0, Console.CursorTop++);
                        return true;
                    }

                    if (input.Key == ConsoleKey.N)
                    {
                        Console.BackgroundColor = oldback;
                        Console.ForegroundColor = oldfore;
                        Console.SetCursorPosition(0, Console.CursorTop++);
                        return false;
                    }

                    Console.Beep(300, 30);
                }
            }
        }

        public static bool MsgBox(string title, string message, bool cancel, int w, int y = 12, int x = 24,
            ConsoleColor back = ConsoleColor.Gray, ConsoleColor text = ConsoleColor.Black, ConsoleColor bar = ConsoleColor.Blue,
            ConsoleColor barText = ConsoleColor.White)
        {
            var Message = Convert.ToStringArr(message);
            w += 2;
            var oldback = Console.BackgroundColor;
            var oldfore = Console.ForegroundColor;
            for (;;)
            {
                Console.BackgroundColor = bar;
                Console.ForegroundColor = barText;
                Console.SetCursorPosition(x, y);
                for (var i = 0; i < w; i++) Console.Write(" ");
                Console.SetCursorPosition(x + 1, y);
                Console.Write("[" + title + "]");
                Console.BackgroundColor = back;
                Console.ForegroundColor = text;
                var okc = 0;
                for (var i = 0; i < Message.Length + 3; i++)
                {
                    Console.SetCursorPosition(x, y + 1 + i);
                    for (var it = 0; it < w; it++) Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                    Console.BackgroundColor = back;
                    okc = Console.CursorTop;
                }

                var yi = y + 2;
                for (var i = 0; i < Message.Length; i++)
                {
                    Console.SetCursorPosition(x + 1, yi + i);
                    if (Message[i].Length <= w - 2) Console.Write(Message[i]);
                }

                Console.SetCursorPosition(x + 6, okc);
                Menu.WriteMenuOption("OK", text);
                Console.SetCursorPosition(x + 13, okc);
                if (cancel) Menu.WriteMenuOption("CANCEL", text);
                Console.SetCursorPosition(x + 1, okc + 1);
                Console.BackgroundColor = ConsoleColor.Black;
                for (var it = 0; it < w; it++) Console.Write(" ");
                var input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.O)
                {
                    Console.BackgroundColor = oldback;
                    Console.ForegroundColor = oldfore;
                    Console.SetCursorPosition(0, Console.CursorTop++);
                    return true;
                }

                if (input.Key == ConsoleKey.C && cancel)
                {
                    Console.BackgroundColor = oldback;
                    Console.ForegroundColor = oldfore;
                    Console.SetCursorPosition(0, Console.CursorTop++);
                    return false;
                }

                Console.Beep(300, 30);
            }
        }

        public static bool ArrBox(string title, string[] message, bool cancel, int w, int y = 6, int x = 25,
            ConsoleColor back = ConsoleColor.Gray, ConsoleColor text = ConsoleColor.Black, ConsoleColor bar = ConsoleColor.Blue,
            ConsoleColor barText = ConsoleColor.White)
        {
            w += 2;
            var oldback = Console.BackgroundColor;
            var oldfore = Console.ForegroundColor;
            for (;;)
            {
                Console.BackgroundColor = bar;
                Console.ForegroundColor = barText;
                Console.SetCursorPosition(x, y);
                for (var i = 0; i < w; i++) Console.Write(" ");
                Console.SetCursorPosition(x + 1, y);
                Console.Write("[" + title + "]");
                Console.BackgroundColor = back;
                Console.ForegroundColor = text;
                var okc = 0;
                for (var i = 0; i < message.Length + 3; i++)
                {
                    Console.SetCursorPosition(x, y + 1 + i);
                    for (var it = 0; it < w; it++) Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                    Console.BackgroundColor = back;
                    okc = Console.CursorTop;
                }

                var yi = y + 2;
                for (var i = 0; i < message.Length; i++)
                {
                    Console.SetCursorPosition(x + 1, yi + i);
                    if (message[i].Length <= w - 2) Console.Write(message[i]);
                }

                Console.SetCursorPosition(x + 6, okc);
                Menu.WriteMenuOption("OK", text);
                Console.SetCursorPosition(x + 13, okc);
                if (cancel) Menu.WriteMenuOption("CANCEL", text);
                Console.SetCursorPosition(x + 1, okc + 1);
                Console.BackgroundColor = ConsoleColor.Black;
                for (var it = 0; it < w; it++) Console.Write(" ");
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.O)
                {
                    Console.BackgroundColor = oldback;
                    Console.ForegroundColor = oldfore;
                    Console.SetCursorPosition(0, Console.CursorTop++);
                    return true;
                }

                if (input.Key == ConsoleKey.C && cancel)
                {
                    Console.BackgroundColor = oldback;
                    Console.ForegroundColor = oldfore;
                    Console.SetCursorPosition(0, Console.CursorTop++);
                    return false;
                }

                Console.Beep(300, 30);
            }
        }

        public static string OpenBox(string path, string title = "Open", string message = "File to open", int w = 32, int y = 6,
            int x = 25, ConsoleColor back = ConsoleColor.Gray, ConsoleColor text = ConsoleColor.Black,
            ConsoleColor bar = ConsoleColor.Blue, ConsoleColor barText = ConsoleColor.White)
        {
            string[] files = {" ", " "};
            if (Directory.Exists(path)) files = Directory.GetFiles(path);
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = bar;
            Console.ForegroundColor = barText;
            for (var i = 0; i < w; i++) Console.Write(" ");
            Console.SetCursorPosition(x + 1, y);
            Console.Write("[" + title + "]");
            var okc = 0;
            for (var i = 0; i < files.Length + 5; i++)
            {
                Console.BackgroundColor = back;
                Console.SetCursorPosition(x, y + 1 + i);
                for (var it = 0; it < w; it++) Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
                Console.BackgroundColor = back;
                okc = Console.CursorTop;
            }

            Console.SetCursorPosition(x + 1, okc + 1);
            Console.BackgroundColor = ConsoleColor.Black;
            for (var it = 0; it < w; it++) Console.Write(" ");
            Console.SetCursorPosition(x + 1, y + 2);
            Console.BackgroundColor = back;
            Console.ForegroundColor = text;
            Console.Write("[" + message + "]");
            for (var i = 0; i < files.Length; i++)
            {
                Console.SetCursorPosition(x + 1, y + 3 + i);
                Console.Write(files[i]);
                Console.SetCursorPosition(x + 1, y + 4 + i);
            }

            Console.CursorTop++;
            Console.Write(title + ": [");
            var h = Console.CursorLeft;
            Console.CursorLeft += w - 10;
            var mxcurpos = Console.CursorLeft;
            Console.Write("]");
            Console.CursorLeft = h;
            var opn = Read.LootiTerminal(mxcurpos, ']');
            return opn;
        }

        public static string InputBox(string title, string print, int y = 6, int x = 25, ConsoleColor back = ConsoleColor.Gray,
            ConsoleColor text = ConsoleColor.Black, ConsoleColor bar = ConsoleColor.Blue,
            ConsoleColor barText = ConsoleColor.White)
        {
            Console.BackgroundColor = bar;
            Console.ForegroundColor = barText;
            Console.SetCursorPosition(x, y);
            for (var i = 0; i < 35; i++) Console.Write(" ");
            Console.SetCursorPosition(x, y);
            Console.Write(" [" + title + "]");
            Console.ForegroundColor = text;
            Console.BackgroundColor = back;
            Console.SetCursorPosition(x, y + 1);
            for (var i = 0; i < 35; i++) Console.Write(" ");
            Console.SetCursorPosition(x, y + 2);
            for (var i = 0; i < 35; i++) Console.Write(" ");
            Console.SetCursorPosition(x + 1, y + 2);
            Console.Write(print + ": [");
            var x2 = Console.CursorLeft;
            Console.SetCursorPosition(x + 33, y + 2);
            Console.Write("]");
            var x3 = Console.CursorLeft + 1;
            Console.SetCursorPosition(x, y + 3);
            for (var i = 0; i < 35; i++) Console.Write(" ");
            Console.SetCursorPosition(x + 1, y + 4);
            Console.BackgroundColor = ConsoleColor.Black;
            for (var i = 0; i < 35; i++) Console.Write(" ");
            Console.SetCursorPosition(x3, y + 1);
            Console.Write(" ");
            Console.SetCursorPosition(x3, y + 2);
            Console.Write(" ");
            Console.SetCursorPosition(x3, y + 3);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(x2, y + 2);
            var toret = Read.LootiTerminal(x + 33, ']');
            return toret;
        }

        public static string[] Login(string login, string password, int y = 6, int x = 25,
            ConsoleColor back = ConsoleColor.Gray, ConsoleColor text = ConsoleColor.Black, ConsoleColor bar = ConsoleColor.Blue,
            ConsoleColor barText = ConsoleColor.White)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            var logInput = InputBox("Login", "Login", y, x, back, text, bar, barText);
            var passInput = InputBox("Login", "Password", y, x, back, text, bar, barText);
            var toret = new string[2];
            toret[0] = logInput;
            toret[1] = passInput;
            if (toret[0] != login || toret[1] != password)
            {
                MsgBox("Login", "Incorrect login or password,\ntry again", false, 30);
                Login(login, password, x: 20, y: 10);
            }

            return toret;
        }
    }
}