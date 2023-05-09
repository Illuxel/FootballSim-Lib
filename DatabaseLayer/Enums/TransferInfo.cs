using System.ComponentModel;

namespace DatabaseLayer
{
    public enum TransferStatus
    {
        [Description("DONE")]
        Done = 1,
        [Description("CANCEL")]
        Cancel = 2
    }
    public enum TransferType
    {
        [Description("RENT")]
        Rent = 1,
        [Description("FREEAGENT")]
        FreeAgent = 2,
        [Description("BUY")]
        Buy = 3,
        [Description("OFFER")]
        Offer = 4
    }
}
