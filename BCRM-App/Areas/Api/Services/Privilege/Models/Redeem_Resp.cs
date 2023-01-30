using BCRM.Common.Models.DBModel.Privilege;

namespace BCRM_App.Areas.Api.Services.Privilege
{
    public class Redeem_Resp
    {
        public Reward_Redemption_Resp PrivilegeRedemption { get; set; }
        public string BrandName { get; set; }
    }
}
