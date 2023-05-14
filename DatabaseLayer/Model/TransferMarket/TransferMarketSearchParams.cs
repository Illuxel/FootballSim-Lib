using DatabaseLayer.Repositories;
using DatabaseLayer;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Model
{
    public class TransferMarketSearchParams
    {
        public int? Ratting { get; set; }
        public List<string> PositionParams { get; set; }
        public string? PositionParam { get; set; }
        public int? AgeLowerBound { get; set; }
        public int? AgeUpperBound { get; set; }
        public decimal? SumLowerBound { get; set; }
        public decimal? SumUpperBound { get; set; }

        public TransferMarketSearchParams()
        {
            var pos = new PositionRepository();
            var position = PositionParam == null ? pos.Retrieve() : new List<Position> { pos.Retrieve(PositionParam) };

            Ratting = (Ratting == null ? 0 : Ratting);
            AgeLowerBound = (AgeLowerBound == null ? 0 : AgeLowerBound);
            AgeUpperBound = (AgeUpperBound == null ? 100 : AgeUpperBound);
            SumLowerBound = (SumLowerBound == null ? 0 : SumLowerBound);
            SumUpperBound = (SumUpperBound == null ? decimal.MaxValue : SumUpperBound);
            PositionParams = position.Select(s => s.Code).ToList();
        }
    }
}
