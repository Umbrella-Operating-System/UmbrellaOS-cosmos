using System;

namespace Umbrella.Libraries
{
    public class Screen
    {
        public static void ClearScreen(ConsoleColor back, ConsoleColor fore)
        {
            Console.BackgroundColor = back;
            Console.Clear();
            Console.BackgroundColor = back;
            Console.SetCursorPosition(0, 0);
        }

        public static void Fillscreen(ConsoleColor back, ConsoleColor fore)
        {
            Console.BackgroundColor = back;
            Console.BackgroundColor = back;
            Console.SetCursorPosition(0, 0);
        }
    }
}