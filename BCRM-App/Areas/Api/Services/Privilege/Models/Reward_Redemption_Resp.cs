using BCRM_App.Areas.Api.Services.Customer.Models;
using System.Collections.Generic;

namespace BCRM_App.Areas.Api.Services.Privilege
{
    public class Reward_Redemption_Resp
    {
        public string RewardRedemptionRef { get; set; }
        public string CouponCodeRef { get; set; }
        public List<string> CouponCodeRefs { get; set; }
        public int Type { get; set; }
        public int TypeDesc { get; set; }
        public string Privilege_Image_Url { get; set; }
        public int PointUsed { get; set; }
        public int Balance { get; set; }
        public CouponCode_Resp CouponCodeInfo { get; set; }
        public List<CouponCode_Resp> CouponCodeInfos { get; set; }
    }
}
