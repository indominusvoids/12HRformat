namespace _12HRFormat
{
    public class GameUtilsPatch
    {
        public static int WorldTimeToDays(ulong _worldTime) => (int)((long)(_worldTime / 24000UL) + 1L);

        public static int WorldTimeToHours12(ulong _worldTime)
        {
            int hours24 = (int)(_worldTime / 1000UL) % 24;
            int hours12 = hours24 % 12;
            return hours12 == 0 ? 12 : hours12;
        }

        public static string GetAMPM(ulong _worldTime)
        {
            int hours24 = (int)(_worldTime / 1000L) % 24;
            return hours24 >= 12 ? "PM" : "AM";
        }

        public static int WorldTimeToMinutes(ulong _worldTime)
        {
            return (int)((double)_worldTime / 1000.0 * 60.0) % 60;
        }

        public static float WorldTimeToTotalSeconds(float _worldTime) => _worldTime * 3.6f;

        public static uint WorldTimeToTotalMinutes(ulong _worldTime)
        {
            return (uint)((double)_worldTime * 0.06);
        }

        public static int WorldTimeToTotalHours(ulong _worldTime) => (int)(_worldTime / 1000UL);

        public static ulong TotalMinutesToWorldTime(uint _totalMinutes)
        {
            return (ulong)((double)_totalMinutes / 0.06);
        }

        public static (int Days, int Hours, int Minutes, string AMPM) WorldTimeToElements(ulong _worldTime)
        {
            int days = (int)((long)(_worldTime / 24000UL) + 1L);
            int hours12 = WorldTimeToHours12(_worldTime);
            int minutes = (int)((double)_worldTime * 0.06) % 60;
            string ampm = GetAMPM(_worldTime);
            return (days, hours12, minutes, ampm);
        }
    }
}
