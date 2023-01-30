namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models
{
    public class EarnPointMessage
    {
        public string Line_Name { get; set; }
        public string Line_UserId { get; set; }
        public int Point { get; set; }
        public string Remark { get; set; }
    }

    public class CumulativeSpendingMessage
    {
        public string Line_Name { get; set; }
        public string Line_UserId { get; set; }
        public int Point { get; set; }
        public string Remark { get; set; }
    }

    public class RejectMessage
    {
        public string Line_Name { get; set; }
        public string Line_UserId { get; set; }
        public string Remark { get; set; }
    }
}
