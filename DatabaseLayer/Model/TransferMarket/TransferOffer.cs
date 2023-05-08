namespace DatabaseLayer.Model
{
    public class TransferOffer
    {
        public string Id { get;  set; }
        public string TeamIdBuyer { get; set; }
        public decimal OfferSum { get; set; }
        public string IDMarket { get; set; }
    }
}
