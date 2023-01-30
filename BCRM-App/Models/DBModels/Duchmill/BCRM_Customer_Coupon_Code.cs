using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_Customer_Coupon_Code
    {
        public int CouponCodeId { get; set; }
        public int CRM_CustomerId { get; set; }
        public string Identity_SRef { get; set; }
        public Guid? Reference { get; set; }
        public string CouponCode { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public bool? ForPOS { get; set; }
        public string Privilege_Name { get; set; }
        public int? Privilege_Id { get; set; }
        public int? Privilege_Tx_Id { get; set; }
        public Guid? Privilege_Tx_Ref { get; set; }
        public DateTime? Use_DT { get; set; }
        public int? Exp_Time_Within_Sec { get; set; }
        public DateTime? Absolute_Exp_DT { get; set; }
        public DateTime? Expired_DT { get; set; }
        public int Type { get; set; }
        public string Type_Desc { get; set; }
        public DateTime Create_DT { get; set; }
        public DateTime? Update_DT { get; set; }
        public int? BrandId { get; set; }
        public string Brand { get; set; }
        public string Remark { get; set; }
    }
}
