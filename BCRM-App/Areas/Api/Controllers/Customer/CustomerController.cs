using BCRM.Common.Factory;
using BCRM.Common.Filters.Action;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static BCRM.Common.Constants.BCRM_Core_Const.Api.Filter;
using System.Collections.Generic;
using BCRM_App.Constants;
using BCRM_App.Filters;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM.Common.Services.RemoteInternal.IAM;
using BCRM.Common.Services.RemoteInternal.IAM.Model;
using BCRM_App.Configs;
using System.Text;
using BCRM.Common.Services.Wallet;
using BCRM_App.Areas.Api.Services.Customer.Models;

namespace BCRM_App.Areas.Api.Controllers.Customer
{
    [Area("Api")]
    [ApiVersion("1.0")]
    public partial class CustomerController : API_BCRM_Controller
    {
        private readonly ICustomer_Service customerService;
        private readonly IIAM_Client_Service iamService;
        private readonly IBCRM_Wallet_Service walletService;

        public CustomerController(ILogger<CustomerController> logger,
                                  ICustomer_Service customerService,
                                  IBCRM_Exception_Factory bcrm_Ex_Factory,
                                  IIAM_Client_Service iamService,
                                  IBCRM_Wallet_Service walletService,
                                  IHttpContextAccessor httpContext_Accessor) : base(logger, bcrm_Ex_Factory, httpContext_Accessor)
        {
            this.customerService = customerService;
            this.iamService = iamService;
            this.walletService = walletService;
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.Register)]
        [BCRM_Api_Logging(Log_Header: true, Log_Req: true, Log_Resp: true, Req_Keys: new string[] { "req" })]
        public async Task<IActionResult> Register([FromBody] RegisterModel req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                string lineId = _ctrl_Util.GetRouteData<string>(AppConstants.RouteData.Line.LineId);
                req.Line_UserId = lineId;

                var appIdentityContext = VerifyAppIdentityContext();

                customerService.SetIdentityContext(apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var registerResponse = await customerService.Register(customerData: req);

                Dictionary<string, string> bcrm_payload = new Dictionary<string, string>();
                bcrm_payload[AppConstants.RouteData.Scope] = AppConstants.Token.Scope.API;
                bcrm_payload[AppConstants.RouteData.TokenId] = Guid.NewGuid().ToString();
                bcrm_payload[AppConstants.RouteData.Line.LineId] = registerResponse.LineInfo.Line_UserId;
                bcrm_payload[AppConstants.RouteData.Line.Linename] = registerResponse.LineInfo.Name;
                bcrm_payload[AppConstants.RouteData.Identity_SRef] = registerResponse.CRM_CustomerInfo.Identity_SRef;
                bcrm_payload[AppConstants.RouteData.CustomerId] = registerResponse.CRM_CustomerInfo.CRM_CustomerId.ToString();


                var exchangeTokenResponse = await iamService.TokenExchangeAsync(access_token: AccessToken,
                                                                   brand_ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                                                   scope: "",
                                                                   payload: bcrm_payload);
                if (exchangeTokenResponse.Success)
                {
                    IAM_Token_Exchange_Resp tokenExchange = (IAM_Token_Exchange_Resp)exchangeTokenResponse.ResponseSuccess;

                    Data = new 
                    {
                        tokenExchange.Access_Token,
                        tokenExchange.Access_Token_Exp_time,
                        registerResponse.WelcomePoints
                    };

                    Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
                }
                else
                {
                    string errorMessageTh = exchangeTokenResponse.ResponseError.RawData["error"]["ErrorMessageTh"].ToString();

                    string errorMessageEn = exchangeTokenResponse.ResponseError.RawData["error"]["ErrorMessageEn"].ToString();

                    throw new Exception(!string.IsNullOrEmpty(errorMessageTh) ? errorMessageTh : errorMessageEn);
                }
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult CustomerInfo()
        {
            try
            {
                string identity_SRef = _ctrl_Util.GetRouteData<string>(AppConstants.RouteData.Identity_SRef);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                customerService.SetIdentityContext(apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var response = customerService.GetCustomerInfo(customerId: customerId, identity_SRef: identity_SRef);

                Data = response;

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
        public async Task<IActionResult> EditCustomerInfo([FromForm] EditProfileReq req)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var customerInfo = await customerService.EditCustomerInfo(customerId: customerId, profile: req);

                Data = customerInfo;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult GetAddress(int? addressId, int addressType)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var address = customerService.GetAddress(addressId: addressId, addressType: addressType, customerId: customerId);

                Data = address;

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
        public IActionResult AddAddress([FromBody] AddAddressModel req)
        {
            try
            {
                int customerId;

                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");
                req.CRM_CustomerId = customerId;
                var address = customerService.AddAddress(address: req, addressType: req.AddressType);

                Data = address;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult GetAddressList(int addressType)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var address = customerService.GetAddressList(addressType: addressType, customerId: customerId);

                Data = address;

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
        public IActionResult UpdateAddress([FromBody] AddressModel req)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                req.CRM_CustomerId = customerId;

                var address = customerService.UpdateAddress(addressType: req.AddressType, address: req);

                Data = address;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Delete)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult DeleteAddress(int? addressId)
        {
            try
            {
                int customerId;

                if (addressId == null) throw new Exception("AddressId has required.");

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var address = customerService.DeleteAddress(addressId: addressId.Value, customerId: customerId);

                Data = address;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult GetPointBalance(string brandName)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var pointBalance = customerService.GetPointBalance(customerId: customerId);

                Data = pointBalance;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }


        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult GetCustomerTags()
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var appIdentityContext = VerifyAppIdentityContext();

                customerService.SetIdentityContext( apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var tiers = customerService.GetCustomerTags(customerId: customerId);

                Data = tiers;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }


        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult GetPointHistory(DateTime? start = null, DateTime? end = null)
        {
            try
            {
                int customerId;

                string identity_SRef = _ctrl_Util.GetRouteData<string>(AppConstants.RouteData.Identity_SRef);

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var pointHistory = customerService.GetPointHistory(customerId: customerId, identity_SRef: identity_SRef, start: start, end: end);

                Data = pointHistory;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult GetConsent()
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");

                var consent = customerService.GetConsent(customerId: customerId);

                Data = consent;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }


        [BCRM_AcceptVerb(BCRM_HttpMethods.Put)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult UpdateConsent([FromBody] GetConsent_Req consent_Req)
        {
            try
            {
                int customerId;

                if (!int.TryParse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId), out customerId)) throw new Exception("Invalid request.");
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                var consent = customerService.UpdateConsent(customerId: customerId, consent_Req: consent_Req);

                Data = consent;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }


        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.Register)]
        public IActionResult GetMemberLastCampaign([FromBody] GetMemberLastCampaign_Req req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                var memberFromLastCampaign = customerService.GetMemberLastCampaign(req.MobileNo);
                Data = memberFromLastCampaign;
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
