namespace FootBalLife.Services.PlayerStatsGen
{
    public class ValueRandomGen
    {
        // отталкиваемся
        private readonly long _mean;
        // разброс
        private readonly long _stdDev;

        private readonly long _minVal;
        private readonly long _maxVal;

        public ValueRandomGen(long mean, long stdDev, long minVal = 1, long maxVal = 100)
        {
            _mean = mean;
            _stdDev = stdDev;
            _minVal = minVal;
            _maxVal = maxVal;
        }

        public double Next()
        {
            return Next(_mean, _stdDev, _minVal, _maxVal);
        }
        public static long Next(long mean, long stdDev, long minVal = 1, long maxVal = 100)
        {
            Random random = new();

            long u1 = random.NextInt64();
            long u2 = random.NextInt64();

            long randStdNormal = (long)(Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2));

            return Math.Max(Math.Min(mean + stdDev * randStdNormal, maxVal), minVal);
        }
    }
}