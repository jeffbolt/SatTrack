using System;

namespace SatTrack.Service.Services
{
    /*
        A Timestamp in the Crunchbase API is Unix Time, or seconds since the epoch.
        https://data.crunchbase.com/docs/timestamp

        Epoch & Unix Timestamp Conversion Tools
        https://www.epochconverter.com/

        What is epoch time?
        The Unix epoch (or Unix time or POSIX time or Unix timestamp) is the number of seconds that have elapsed since 
        January 1, 1970 (midnight UTC/GMT), not counting leap seconds (in ISO 8601: 1970-01-01T00:00:00Z).

        See also https://stackoverflow.com/questions/1756639
    */
    public static class UnixTimeHelper
    {
        private static readonly DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(DateTime date)
        {
            return (long)(date - epoch).TotalSeconds;
        }

        public static long CurrentUnixTime()
        {
            return ToUnixTime(DateTime.UtcNow);
        }
    }
}