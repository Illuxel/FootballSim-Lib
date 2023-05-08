using System;

namespace DatabaseLayer
{
    public class TransferJournal
    {
        public string Id { get;  set; }
        public string OfferId { get; set; }
        public string Status { get; set; }
        public decimal SumFact { get; set; }
        public DateTime DateRelease { get; set; }
    }
}
