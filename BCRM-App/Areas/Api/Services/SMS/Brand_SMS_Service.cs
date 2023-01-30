using BCRM.Common.Factory.Entities.Brand;
using BCRM.Common.Services.RemoteInternal.IAM;
using BCRM.Common.Services.RemoteInternal.IAM.Model;
using BCRM_App.Areas.Api.Models.Sms;
using BCRM_App.Areas.Api.Services.SMS.Models;
using BCRM_App.Configs;
using BCRM_App.Constants;
using BCRM_App.Services.RemoteInternal.SMS;
using BCRM_App.Services.RemoteInternal.SMS.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Areas.Api.Services.SMS
{
    public class Brand_SMS_Service : SMS_Internal_Service_Base, ISMS_Internal_Service
    {
        public Brand_SMS_Service(ISMS_Client_Service sms_Service,
                                     IIAM_Client_Service iamService) : base(sms_Service, iamService)
        {
            this.BrandRef = App_Setting.Brands.Main.Config.Brand_Ref;
            this.AppId = App_Setting.Brands.Main.Config.App_Id;
            this.SenderName = App_Setting.SMS.SenderName;
        }

        public override async Task<OTP_Verify_Resp> VerifySMS(SMSOTP_Verify req)
        {

            try
            {
                dynamic tokenInfo;

                using (DuchmillModel.BCRM_36_Entities DutchmillContext = await new BCRM_Brand_Entities_Factory<DuchmillModel.BCRM_36_Entities>().CreateAsync(App_Setting.Brands.Main.Config.Brand_Ref))
                {
                    var customer = (from crm in DutchmillContext.CRM_Customers
                                    where crm.MobileNo == req.TokenPayload.MobileNo
                                    select new { crm }).FirstOrDefault();

                    if (customer != null) throw new Exception("User has already exist in system.");

                    var _tokenInfo = new
                    {
                        AccessToken = req.AccessToken,
                        LineId = req.TokenPayload.LineId,
                        LineName = req.TokenPayload.LineName,
                        MobileNo = req.TokenPayload.MobileNo,
                        Brand_Ref = App_Setting.Brands.Main.Config.Brand_Ref,
                    };

                    tokenInfo = _tokenInfo;
                }

                var response = await base.VerifySMS(req);

                var token = await GenerateToken(response.VerifyResponse, tokenInfo);

                response.Access_Token = token.Access_Token;
                response.Access_Token_Exp_time = token.Access_Token_Exp_time;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task<IAM_Token_Exchange_Resp> GenerateToken(dynamic req, dynamic tokenInfo)
        {
            try
            {
                var lineInfo = tokenInfo;

                Dictionary<string, string> bcrm_payload = new Dictionary<string, string>();
                bcrm_payload[AppConstants.RouteData.Scope] = AppConstants.Token.Scope.Register;
                bcrm_payload[AppConstants.RouteData.TokenId] = Guid.NewGuid().ToString();
                bcrm_payload[AppConstants.RouteData.Line.LineId] = lineInfo.LineId;
                bcrm_payload[AppConstants.RouteData.Line.Linename] = lineInfo.LineName;
                bcrm_payload[AppConstants.RouteData.Customer_MobileNo] = req.MobileNo;

                var response = await iamService.TokenExchangeAsync(lineInfo.AccessToken, lineInfo.Brand_Ref, "", bcrm_payload);

                if (response.Success)
                {
                    IAM_Token_Exchange_Resp tokenExchange = (IAM_Token_Exchange_Resp)response.ResponseSuccess;

                    return tokenExchange;
                }
                else
                {
                    string errorMessageTh = response.ResponseError.RawData["error"]["ErrorMessageTh"].ToString();

                    string errorMessageEn = response.ResponseError.RawData["error"]["ErrorMessageEn"].ToString();

                    throw new Exception(!string.IsNullOrEmpty(errorMessageTh) ? errorMessageTh : errorMessageEn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
