using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Privilege
    {
        public int PrivilegeId { get; set; }
        public int BrandId { get; set; }
        public Guid Reference { get; set; }
        public string Alt_Reference { get; set; }
        public int Progress_Status { get; set; }
        public string Progress_Status_Desc { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public int Seq { get; set; }
        public int Type { get; set; }
        public int CPC_Type { get; set; }
        public int CPC_Issue_Mode { get; set; }
        public int CPC_Qty_Per_Issue { get; set; }
        public string CPC_Pattern { get; set; }
        public int Redeem_Mode { get; set; }
        public string Name_Th { get; set; }
        public string Name_En { get; set; }
        public string SubName_Th { get; set; }
        public string SubName_En { get; set; }
        public string Desc_Th { get; set; }
        public string Desc_En { get; set; }
        public string TermsAndCondsTh { get; set; }
        public string TermsAndCondsEn { get; set; }
        public string Privilege_Image_Url { get; set; }
        public string Privilege_BCI_Ref { get; set; }
        public int Point_Ref { get; set; }
        public decimal Value_Ref { get; set; }
        public int Reserve_Timeout { get; set; }
        public int? TP_Carrier_Flag { get; set; }
        public int? TP_Topup_Value { get; set; }
        public int? PV_LimitId { get; set; }
        public int Total_Issue_TX { get; set; }
        public int Total_Redeem_TX { get; set; }
        public int Total_Issue { get; set; }
        public int Total_Redeem { get; set; }
        public int Exp_Mode { get; set; }
        public string Exp_Mode_Desc { get; set; }
        public int Exp_Rounding_Mode { get; set; }
        public int Exp_Period_Year { get; set; }
        public int Exp_Period_Month { get; set; }
        public int Exp_Period_Day { get; set; }
        public int Exp_Period_Hour { get; set; }
        public int Exp_Period_Minute { get; set; }
        public DateTime? Exp_Period_FixedDate { get; set; }
        public DateTime Valid_From { get; set; }
        public DateTime Valid_Through { get; set; }
        public string Extra_Ref { get; set; }
        public string Extra_Ref_2 { get; set; }
        public bool Product_Mapping { get; set; }
        public string Remark { get; set; }
        public string Extra_Payload { get; set; }
        public int? Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? EXC_SchemeId { get; set; }
    }
}
