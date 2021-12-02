using System;

namespace Umbrella.Libraries
{
    public class Read
    {
        public static string TextBox(int maxlenght, bool showBox = true, char boxopen = '[', char boxclose = ']',
            ConsoleColor back = ConsoleColor.Gray, ConsoleColor text = ConsoleColor.Black)
        {
            if (Console.CursorLeft - 80 > maxlenght + 2)
            {
                Console.CursorLeft = 0;
                Console.CursorTop++;
            }

            Console.BackgroundColor = back;
            Console.ForegroundColor = text;
            if (showBox) Console.Write(boxopen);
            var x = Console.CursorLeft;
            for (var i = 0; i <= maxlenght; i++) Console.Write(" ");
            if (showBox) Console.Write(boxclose);
            Console.SetCursorPosition(x, Console.CursorTop);
            var toreturn = "";
            if (showBox) toreturn = LootiTerminal(Console.CursorLeft + maxlenght + 1, ']');
            if (showBox == false) toreturn = LootiTerminal(Console.CursorLeft + maxlenght);
            return toreturn;
        }

        public static string LootiTerminal(int mxcurpos, char bck = ' ')
        {
            var toreturn = "";
            for (;;)
            {
                var arrow = "";
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Enter) return toreturn + arrow;
                if (input.Key == ConsoleKey.Backspace)
                {
                    var x = Console.CursorLeft;
                    if (toreturn.Length != 0)
                    {
                        Console.CursorLeft--;
                        toreturn = toreturn.Remove(toreturn.Length - 1, 1);
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                    else
                    {
                        Console.CursorLeft = x;
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
                else
                {
                    if (Console.CursorLeft <= mxcurpos)
                    {
                        toreturn += input.KeyChar.ToString();
                    }
                    else
                    {
                        Console.CursorLeft--;
                        Console.Write(bck);
                        Console.CursorLeft--;
                    }
                }
            }
        }
    }
}