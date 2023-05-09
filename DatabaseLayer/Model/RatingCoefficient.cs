using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.Model
{
    internal class RatingCoefficient
    {
        public string ID { get; set; }
        public string IDRating { get; set; }
        public string Season { get; set; }
        public double Coeff { get; set; }
    }
}
