using System;

namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public partial class RewardRedemption
    {
        //public long TransactionId { get; set; }
        public Guid RedemptionRef { get; set; }
        public Guid? CouponCodeRef { get; set; }
        public string Name_Th { get; set; }
        public string Name_En { get; set; }
        public string SubName_Th { get; set; }
        public string SubName_En { get; set; }
        public int Type { get; set; }
        public bool Code_IsExpired { get; set; }
        public int PrivilegeId { get; set; }
        public string Privilege_Image_Url { get; set; }
        public string Privilege_Sub_Image_Url { get; set; }
        public decimal Point_Ref { get; set; }
        public int Amount { get; set; }
        public decimal PointUsed { get; set; }
        public string ITF_Ref { get; set; }
        public string ITF_Ref_2 { get; set; }
        public string Remark { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
