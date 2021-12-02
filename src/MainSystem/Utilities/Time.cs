using Cosmos.HAL;

namespace Umbrella.MainSystem.Utilities
{
    public static class Time
    {
        /// <summary>
        ///     Variables with time
        /// </summary>
        private static int Hour()
        {
            return RTC.Hour;
        }

        private static int Minute()
        {
            return RTC.Minute;
        }

        private static int Second()
        {
            return RTC.Second;
        }

        private static int Year()
        {
            return RTC.Year;
        }

        private static int Month()
        {
            return RTC.Month;
        }

        private static int DayOfMonth()
        {
            return RTC.DayOfTheMonth;
        }

        /// <summary>
        ///     Time format
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns>Time in 12-hour time format</returns>
        private static string Time12(bool hour, bool min, bool sec)
        {
            var trueTime = "";
            if (hour)
            {
                if (Hour() > 12) trueTime += Hour() - 12;
                else trueTime += Hour();
            }

            if (min)
            {
                if (Minute().ToString().Length == 1)
                {
                    trueTime += ":";
                    trueTime += "0" + Minute();
                }
                else
                {
                    trueTime += ":";
                    trueTime += Minute().ToString();
                }
            }

            if (sec)
            {
                if (Second().ToString().Length == 1)
                {
                    trueTime += ":";
                    trueTime += "0" + Second();
                }
                else
                {
                    trueTime += ":";
                    trueTime += Second().ToString();
                }
            }

            if (hour)
            {
                if (Hour() > 12) trueTime += " PM";
                else trueTime += " AM";
            }

            return trueTime;
        }

        /// <summary>
        ///     Time output in different formats for different languages (so far only English)
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns>Time in 12-hour format</returns>
        public static string TimeString(bool hour, bool min, bool sec)
        {
            return Time12(hour, min, sec);
        }

        /// <summary>
        ///     The current year in the string
        /// </summary>
        /// <returns>Year string</returns>
        public static string YearString()
        {
            var intYear = Year();
            var stringYear = intYear.ToString();
            if (stringYear.Length == 2) stringYear = $"20{stringYear}";
            else System.Console.WriteLine("lol, what?))");
            return stringYear;
        }

        /// <summary>
        ///     The current month in the string
        /// </summary>
        /// <returns>Year month</returns>
        public static string MonthString()
        {
            var intMonth = Month();
            var stringMonth = intMonth.ToString();
            if (stringMonth.Length == 2) stringMonth = $"{stringMonth}";
            return stringMonth;
        }

        /// <summary>
        ///     The current day in the string
        /// </summary>
        /// <returns>Day string</returns>
        public static string DayString()
        {
            var intDay = DayOfMonth();
            var stringDay = intDay.ToString();
            if (stringDay.Length == 1) stringDay = $"0{stringDay}";
            return stringDay;
        }
    }
}