namespace BCRM_App.Areas.Api.Services.Point
{
    public class SpendingInfo
    {
        public decimal Spending { get; set; }
        public int? TransactionId { get; set; }
        public string TransactionRef { get; set; }
        public bool RecalculatePoint { get; set; } = false;
        public string Remark { get; set; }
    }

    public class PointInfo
    {
        public int Point { get; set; }
        public int? TransactionId { get; set; }
        public string TransactionRef { get; set; }
    }


    public class VoidPointInfo
    {
        public int? TransactionId { get; set; }
        public string TransactionRef { get; set; }
    }
}