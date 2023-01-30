using BCRM.Common.Factory;
using BCRM.Common.Filters.Action;
using BCRM_App.Areas.Api.Models.Sms;
using BCRM_App.Areas.Api.Services.SMS;
using BCRM_App.Configs;
using BCRM_App.Constants;
using BCRM_App.Filters;
using BCRM_App.Services.RemoteInternal.SMS.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace BCRM_App.Areas.Api.Controllers.SMS
{
    [Area("Api")]
    [ApiVersion("1.0")]
    public class SMSController : API_BCRM_Controller
    {
        //private readonly IEnumerable<ISMS_Internal_Service> iSMS_Internal_Services;
        private readonly ISMS_Internal_Service iSMS_Internal_Service;
        private readonly IWebHostEnvironment env;

        public SMSController(ILogger<SMSController> logger,
                             IBCRM_Exception_Factory bcrm_Ex_Factory,
                             ISMS_Internal_Service iSMS_Internal_Service,
                             IWebHostEnvironment env,
                             //IEnumerable<ISMS_Internal_Service> iSMS_Internal_Services,
                             IHttpContextAccessor httpContext_Accessor) : base(logger, bcrm_Ex_Factory, httpContext_Accessor)
        {
            //this.iSMS_Internal_Services = iSMS_Internal_Services;
            this.iSMS_Internal_Service = iSMS_Internal_Service;
            this.env = env;
        }

        // POST: api/v1/SMS/Send
        [BCRM_AcceptVerb(BCRM.Common.Constants.BCRM_Core_Const.Api.Filter.BCRM_HttpMethods.Post)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.OTP)]
        [BCRM_Api_Logging(Log_Header: true, Log_Req: true, Log_Resp: true, Req_Keys: new string[] { "req" })]
        public async Task<IActionResult> Send([FromBody] SMSOTP_Send_Req req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                var requestOTP = new SMSOTP_Send()
                {
                    SendInfo = req
                };

                var response = await iSMS_Internal_Service.SendSMS(requestOTP);

                Data = new
                {
                    TransactionId = response.TransactionId,
                    OTP_Reference = response.OTP_Reference,
                    ExpireTime = response.ExpireTime
                };

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }


        // POST: api/v1/SMS/Resend
        [BCRM_AcceptVerb(BCRM.Common.Constants.BCRM_Core_Const.Api.Filter.BCRM_HttpMethods.Post)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.OTP)]
        [BCRM_Api_Logging(Log_Header: true, Log_Req: true, Log_Resp: true, Req_Keys: new string[] { "req" })]
        public async Task<IActionResult> Resend([FromBody] SMSOTP_Resend_Req req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                var resendOTP = new SMSOTP_Resend()
                {
                    ResendInfo = req
                };

                var response = await iSMS_Internal_Service.ResendSMS(resendOTP);

                Data = new
                {
                    TransactionId = response.TransactionId,
                    OTP_Reference = response.OTP_Reference,
                    ExpireTime = response.ExpireTime
                };

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        // POST: api/v1/SMS/Verify
        [BCRM_AcceptVerb(BCRM.Common.Constants.BCRM_Core_Const.Api.Filter.BCRM_HttpMethods.Post)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.OTP)]
        [BCRM_Api_Logging(Log_Header: true, Log_Req: true, Log_Resp: true, Req_Keys: new string[] { "req" })]
        public async Task<IActionResult> Verify([FromBody] SMSOTP_Verify_Req req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                string lineId = _ctrl_Util.GetRouteData<string>(AppConstants.RouteData.Line.LineId);
                string lineName = _ctrl_Util.GetRouteData<string>(AppConstants.RouteData.Line.Linename);

                if (req.OTP == "119911") // for test script
                {
                    if (env.IsDevelopment())
                    {

                        SMSOTP_Verify verifyOTP = new SMSOTP_Verify()
                        {
                            VerifyInfo = req,
                            AccessToken = AccessToken,
                            TokenPayload = new TokenPayloadInfo()
                            {
                                MobileNo = req.MobileNo,
                                LineId = lineId,
                                LineName = lineName,
                                AccessToken = AccessToken,
                                Brand_Ref = App_Setting.Brands.Main.Config.Brand_Ref
                            }
                        };

                        var response = await iSMS_Internal_Service.GenerateToken(req: req, tokenInfo: verifyOTP.TokenPayload);

                        Data = new
                        {
                            Access_Token = response.Access_Token
                        };

                        Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
                    }
                }
                else
                {
                    SMSOTP_Verify verifyOTP = new SMSOTP_Verify()
                    {
                        VerifyInfo = req,
                        AccessToken = AccessToken,
                        TokenPayload = new TokenPayloadInfo()
                        {
                            MobileNo = req.MobileNo,
                            LineId = lineId,
                            LineName = lineName,
                        }
                    };

                    var response = await iSMS_Internal_Service.VerifySMS(verifyOTP);

                    Data = new
                    {
                        Access_Token = response.Access_Token
                    };

                    Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
                }
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }
            
            return Build_JsonResp();
        }
    }

    /*
    private void SetSMSInternalService(string brandRef = "", string appId = "")
    {
        try
        {
            if (!string.IsNullOrEmpty(brandRef))
            {
                var smsInternalService = iSMS_Internal_Services.SingleOrDefault(it => it.BrandRef == brandRef);
                iSMS_Internal_Service = smsInternalService;
            }
            else
            {
                var smsInternalService = iSMS_Internal_Services.SingleOrDefault(it => it.AppId == appId);
                iSMS_Internal_Service = smsInternalService;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    */
}
