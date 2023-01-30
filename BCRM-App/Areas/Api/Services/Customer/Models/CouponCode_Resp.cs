using System;

namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class CouponCode_Resp
    {
        public string CouponCode { get; set; }
        public DateTime? ExpiredTime { get; set; }
        public double? ExpiredWithinSec { get; set; }
    }
}
