using System;

namespace Umbrella.Libraries
{
    public class Desktop
    {
        public static void Draw(ConsoleColor back, ConsoleColor fore)
        {
            Console.BackgroundColor = back;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = fore;
            Console.Write("Umbrella Desktop");
            Console.WriteLine("\n----------------------------");
        }
    }
}