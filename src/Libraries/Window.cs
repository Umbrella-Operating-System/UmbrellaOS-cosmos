using System;

namespace Umbrella.Libraries
{
    public class Window
    {
        public static int[] DrawWindow(string title, int w = 62, int h = 12, int x = 2, int y = 2,
            bool showtitlebrackets = true, bool shadow = true, ConsoleColor back = ConsoleColor.Gray,
            ConsoleColor bar = ConsoleColor.Blue, ConsoleColor barText = ConsoleColor.White)
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = bar;
            Console.ForegroundColor = barText;
            for (var i = 0; i < w; i++) Console.Write(" ");
            Console.SetCursorPosition(x + 1, y);
            if (showtitlebrackets) Console.Write("[" + title + "]");
            else Console.Write(title);
            Console.SetCursorPosition(x, y + 1);
            var mxX = 0;
            for (var i = 0; i < h; i++)
            {
                Console.BackgroundColor = back;
                Console.SetCursorPosition(x, y + 1 + i);
                for (var ii = 0; ii < w; ii++) Console.Write(" ");
                mxX = Console.CursorLeft;
                if (shadow)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
            }

            var mxY = Console.CursorTop;
            Console.CursorLeft = x + 1;
            Console.CursorTop++;
            if (shadow)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                for (var i = 0; i < w; i++) Console.Write(" ");
            }

            Console.BackgroundColor = back;
            Console.SetCursorPosition(x + 1, y + 1);
            int[] ret = {mxX, mxY};
            return ret;
        }
    }
}