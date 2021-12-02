using System;
using static Umbrella.MainSystem.Console.WriteMethods;

namespace Umbrella.MainSystem.Console
{
    internal class MethodsInfo
    {
        /// <summary>
        ///     Creating functions to quickly output information to the OS and the debugger
        ///     ( OK, ERROR, INFO, WARNING and custom solution )
        /// </summary>
        /// <param name="text"></param>
        public static void ConsoleOk(string text)
        {
            Write("\n[ OK ] ", ConsoleColor.Green);
            Write(text, ConsoleColor.White);
        }

        public static void ConsoleError(string text)
        {
            Write("\n[ ERROR ] ", ConsoleColor.Red);
            Write(text, ConsoleColor.White);
        }

        public static void ConsoleInfo(string text)
        {
            Write("\n[ INFO ] ", ConsoleColor.Cyan);
            Write(text, ConsoleColor.White);
        }

        public static void ConsoleWarning(string text)
        {
            Write("\n[ WARNING ] ", ConsoleColor.Yellow);
            Write(text, ConsoleColor.White);
        }

        public static void ConsoleCustom(ConsoleColor customColorTitle, ConsoleColor customColorText, string text, string customText)
        {
            Write($"\n[ {customText} ] ", customColorTitle);
            Write(text, customColorText);
        }
    }
}