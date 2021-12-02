using System;
using System.Collections.Generic;

namespace Umbrella.MainSystem.Console
{
    public class CommandsHistory
    {
        public static int CHIndex = 0;
        public static List<string> commands = new List<string>();

        public static void Add(string cmd)
        {
            commands.Add(cmd);
            CHIndex = commands.Count - 1;
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, currentLineCursor);
            System.Console.Write(new string(' ', System.Console.WindowWidth));
            System.Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
