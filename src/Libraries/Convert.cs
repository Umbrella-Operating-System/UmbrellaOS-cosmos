namespace Umbrella.Libraries
{
    public static class Convert
    {
        public static string[] ToStringArr(this string tsa)
        {
            if (tsa.Contains("\n")) return tsa.Split('\n');
            string[] els = {tsa};
            return els;
        }
    }
}