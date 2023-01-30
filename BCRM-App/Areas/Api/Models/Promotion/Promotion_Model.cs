using BCRM_App.Models.DBModels.Duchmill;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Models
{
    public class Req_Promotion_ValidateEmployee
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string BrandId { get; set; }

        [Required]
        public string BranchId { get; set; }

        [Required]
        public string EmployeeQR { get; set; }
    }

    public class Resp_Promotion_ValidateEmployee
    {
        public string UserId;
        public string BrandId;
        public string BranchId;
        public string EmployeeQR;
        public EmployeeDiscount EmployeeDiscount;
    }

    public class EmployeeDiscount
    {
        public string SerialCode;
        public string BrandId;
        public string Status;
        public string NameTh;
        public string NameEn;
        public int Value;
        public string ValueType;
        public string CouponExp;
        public string CouponDiscountType;
    }

    public class Req_Promotion_Serial
    {
        [Required]
        public string SerialCode { get; set; }

        [Required]
        public string Brandid { get; set; }
    }

    public class Promotion_Serial_Information
    {
        public BCRM_Customer_Coupon_Code CouponCode;
        public BCRM.Common.Models.DBModel.Privilege.CRM_Privilege Privilege;
    }

    public class Resp_Promotion_Serial
    {
        public string SerialCode;
        public string BrandId;
        public string Status;
        public string NameTh;
        public string NameEn;
        public decimal Value;
        public string ValueType;
        public string CouponExp;
        public string CouponDiscountType;
    }
}
