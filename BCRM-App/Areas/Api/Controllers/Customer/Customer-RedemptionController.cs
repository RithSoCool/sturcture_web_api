using BCRM.Common.Filters.Action;
using Microsoft.AspNetCore.Mvc;
using System;
using static BCRM.Common.Constants.BCRM_Core_Const.Api.Filter;
using BCRM_App.Constants;
using BCRM_App.Filters;
using Pv_Tx_Ref = BCRM_App.Constants.AppConstants.Privilege.RewardRedemptionHistory.ActionType;
using BCRM_App.Areas.Api.Services.Customer.Models;

namespace BCRM_App.Areas.Api.Controllers.Customer
{
    public partial class CustomerController : API_BCRM_Controller
    {
        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult GetRewardRedemptionHistory(string brandName = "all", int? rewardType = Pv_Tx_Ref.All, DateTime? start = null, DateTime? end = null)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var redemptionHistory = customerService.GetRedemptionHistory(  customerId: customerId, rewardType: rewardType, start: start, end: end);

                Data = redemptionHistory;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        [BCRM_Api_Logging(Log_Header: true, Log_Req: true, Log_Resp: true, Req_Keys: new string[] { "req" })]
        public IActionResult ShowCouponCode([FromBody] Show_Coupon_Code_Req req)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var codeInfo = customerService.ShowCouponCode(codeRef: req, customerId: customerId);

                Data = codeInfo;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }
    }
}
