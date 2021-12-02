using System;

namespace Umbrella.Libraries
{
    public class Menu
    {
        public static void WriteMenuOption(string text, ConsoleColor fore = ConsoleColor.Black)
        {
            Console.ForegroundColor = fore;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(text[0]);
            Console.ForegroundColor = fore;
            Console.Write(text.Remove(0, 1));
            Console.Write("]");
        }

        public static void Bar(string[] text, int x = 0, int y = 0)
        {
            var old = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(x, y);
            for (var i = 0; i < 80; i++) Console.Write(" ");
            Console.SetCursorPosition(x, y);
            foreach (var t in text)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(t[0]);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(t.Remove(0, 1));
                Console.Write(" | ");
            }

            Console.BackgroundColor = old;
        }

        public static char StripMenu(string[] text, string[] shorts, int x, string name)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.White;
            var y = 1;
            Console.SetCursorPosition(x, y);
            for (var it = 0; it != 11; it++) Console.Write(" ");
            Console.SetCursorPosition(x, y);
            Console.Write(name);
            for (var i = 0; i != text.Length; i++)
            {
                y++;
                Console.SetCursorPosition(x, y);
                for (var it = 0; it != 10; it++) Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(x, y);
                Console.Write(text[i]);
                Console.SetCursorPosition(x + 10, y);
                Console.Write(shorts[i]);
            }

            return Console.ReadKey().KeyChar;
        }
    }
}