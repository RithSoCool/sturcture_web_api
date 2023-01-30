namespace BCRM_App.Areas.Api.Services.Point
{
    public class Calculate_Point_Resp
    {
        public bool IsEarnPoint { get; set; }
        public decimal? Pre_Spending { get; set; }
        public decimal? Spending { get; set; }
        public decimal? Total_Spending { get; set; }
        public int PrePointBalance { get; set; }
        public int EarnPoint { get; set; }
        public int PointBalance { get; set; }
        public int CustomerId { get; set; }
        public string WalletId { get; set; }
        public int BrandId { get; set; }
        public string Brand { get; set; }
    }
}