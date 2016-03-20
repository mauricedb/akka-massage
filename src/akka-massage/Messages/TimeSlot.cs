using System;

namespace akka_massage.Messages
{
    public class TimeSlot
    {
        public int Hour { get; }
        public int Minute { get; }

        public TimeSlot(int hour, int minute)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hour));
            }
            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute));
            }

            Hour = hour;
            Minute = minute;
        }

        public override string ToString()
        {
            return $"{Hour}:{Minute}";
        }

        public int TotalMinutes()
        {
            return 60 * Hour + Minute;
        }
    }

    public class Print
    {
    }
}