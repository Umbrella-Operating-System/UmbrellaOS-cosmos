using System;
using System.IO;

namespace Umbrella.Libraries
{
    public class Sound
    {
        public static int RunFromFile(string path)
        {
            if (!File.Exists(path)) return 0;
            var file = File.ReadAllText(path);
            if (!file.Contains(" ")) return 0;
            var ti = file.Split(' ');
            var data = new int[10240];
            for (var i = 0; i < ti.Length; i++) int.TryParse(ti[0], out data[i]);
            PlaySound(data);

            return 0;
        }

        public static void PlaySound(int[] data)
        {
            for (var i = 1; i < data.Length; i++) Console.Beep(data[i], data[0]);
        }
    }
}