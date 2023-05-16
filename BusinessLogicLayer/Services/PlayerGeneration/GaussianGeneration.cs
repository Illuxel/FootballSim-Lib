using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Services.PlayerGeneration
{
    internal class GaussianGeneration
    {
        private Random _random;
        private double _mean;
        private double _stdDev;
        private int _minValue;
        private int _maxValue;

        static double NextGaussian(Random random, double mean, double stdDev)
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        public GaussianGeneration(double mean, double stdDev, int minValue, int maxValue)
        {
            _mean = mean;
            _stdDev = stdDev;
            _minValue = minValue;
            _maxValue = maxValue;
            _random = new Random();
        }

        public int Next()
        {
            double gaussian = NextGaussian();
            return (int)Math.Max(Math.Min(gaussian, _maxValue), _minValue);
        }

        private double NextGaussian()
        {
            double u1 = _random.NextDouble();
            double u2 = _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return _mean + _stdDev * randStdNormal;
        }
    }
}
