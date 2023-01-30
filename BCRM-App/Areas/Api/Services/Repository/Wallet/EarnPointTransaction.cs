namespace BCRM_App.Areas.Api.Services.Repository.Wallet
{
    public partial class Wallet_Repository
    {
        public class Transaction
        {
            public int Point { get; set; }
            public string TransactionId { get; set; }
            public string Transaction_Ref { get; set; }
            public string Transaction_Extra_Ref { get; set; }
            public string Transaction_Extra_Ref_2 { get; set; }
        }
    }
}
