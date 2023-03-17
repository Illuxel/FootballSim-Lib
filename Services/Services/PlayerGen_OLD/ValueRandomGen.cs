namespace FootBalLife.Services.PlayerStatsGen
{
    public class ValueRandomGen
    {
        // отталкиваемся
        private readonly double _mean;
        // разброс
        private readonly double _stdDev;

        private readonly double _minVal;
        private readonly double _maxVal;

        public ValueRandomGen(long mean, long stdDev, long minVal = 1, long maxVal = 100)
        {
            _mean = mean;
            _stdDev = stdDev;
            _minVal = minVal;
            _maxVal = maxVal;
        }

        public long Next()
        {
            return Next(_mean, _stdDev, _minVal, _maxVal);
        }
        public static long Next(double mean, double stdDev, double minVal = 1, double maxVal = 100)
        {
            Random random = new();

            double u1 = random.NextDouble();
            double u2 = random.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return (long)(Math.Max(Math.Min(mean + stdDev * randStdNormal, maxVal), minVal));
        }
    }
}