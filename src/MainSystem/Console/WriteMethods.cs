using System;

namespace Umbrella.MainSystem.Console
{
    /// <summary>
    ///     Functions for reduced convenience
    /// </summary>
    public static class WriteMethods
    {
        public static ConsoleColor BackColor = ConsoleColor.Black;
        public static ConsoleColor ForeColor = ConsoleColor.White;

        public static void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }

        public static void Write(string text)
        {
            System.Console.Write(text);
        }

        public static void Write(string text, ConsoleColor fg)
        {
            System.Console.ForegroundColor = fg;
            System.Console.Write(text);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteLine(string text, ConsoleColor fg)
        {
            System.Console.ForegroundColor = fg;
            System.Console.WriteLine(text);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}