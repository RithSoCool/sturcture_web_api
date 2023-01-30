using BCRM.Common.Models.DBModel.Privilege;
using BCRM.Common.Services.Privilege;
using System;
using System.Collections.Generic;

namespace BCRM_App.Areas.Api.Services.Privilege
{
    public class GetPrivilegies_Resp
    {
        public Privilege_List_Response Privilegies_Resp { get; set; }
        //public List<CRM_Privilege> Privilegies { get; set; }
        public string BrandName { get; set; }
    }

    public class GetPrivilegiesWithTier_Resp
    {
        public GetPrivilegiesWithTier_Resp()
        {
            Privileges = new List<CRM_Privilege_Resp>();
        }

        public int Total_Record { get; set; }
        public int Filtered_Record { get; set; }
        public int Total_Page { get; set; }
        public int Current_Page { get; set; }
        public List<CRM_Privilege_Resp> Privileges { get; set; }
        public string BrandName { get; set; }
        public int RedeemCount { get; set; }
        public decimal? Point_Balance { get; set; }

    }

    public class GetPrivilegiesDetailsWithTier_Resp
    {
        public GetPrivilegiesDetailsWithTier_Resp()
        {
            PrivilegeDetails = new CRM_Privilege_Resp();
        }

        public CRM_Privilege_Resp PrivilegeDetails { get; set; }
        public ErrorResp Error { get; set; }
        public string BrandName { get; set; }
        public int ParentCategoryId { get; set; }

        public int RedeemCount { get; set; }
        public decimal? Point_Balance { get; set; }

    }

    public class ErrorResp
    {
        public bool IsError { get; set; }
        public string Titile_TH { get; set; }
        public string Titile_EN { get; set; }
        public string Message_TH { get; set; }
        public string Message_EN { get; set; }
        public string Remark { get; set; }
    }

    public partial class CRM_Privilege_Resp
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
        public string Privilege_Sub_Image_Url { get; set; }
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
        public string TierName { get; set; }
        public int? TierValue { get; set; }
        public bool IsOutOfStock
        {
            get
            {
                if (StockRemaining > 0) return false;
                return true;
            }
        }
        public decimal StockRemaining { get; set; }
    }
}
