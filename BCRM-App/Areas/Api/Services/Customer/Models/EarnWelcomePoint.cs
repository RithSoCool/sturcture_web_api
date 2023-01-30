namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class EarnWelcomePoint
    {
        public int PointEarn { get; set; }
        public string BrandName { get; set; }
        public string Wallet_Alt_Ref { get; set; }
        public string TransactionId { get; set; }
    }

    public class EarnWelcomePoint_Resp
    {
        public int WelcomePoint { get; set; }
        public string BrandName { get; set; }
    }
}
