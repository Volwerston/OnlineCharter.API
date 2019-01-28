using System;

namespace Utils
{
    public static class SystemDateTime
    {
        private static Func<DateTime> _dateTimeFactory = () => DateTime.UtcNow;

        public static void Init(Func<DateTime> dateTimeFactory) => _dateTimeFactory = dateTimeFactory;

        public static DateTime Now => _dateTimeFactory();
    }
}
