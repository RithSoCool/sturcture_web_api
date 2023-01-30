using System;
using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class Show_Coupon_Code_Req
    {
        [Required]
        public Guid? RedemptionRef { get; set; }
        [Required]
        public string CouponCodeRef { get; set; }
    }
}
