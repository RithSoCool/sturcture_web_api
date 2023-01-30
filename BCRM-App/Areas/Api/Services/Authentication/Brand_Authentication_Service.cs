using BCRM.Common.Services;
using BCRM_App.Areas.Api.Models;
using BCRM_App.Configs;
using BCRM_App.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using BCRM.Common.Services.RemoteInternal.IAM;
using BCRM.Common.Factory.Entities.Brand;
using BCRM_App.Services.RemoteInternal.Authentication.Models;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BCRM_App.Services.RemoteInternal.Authentication
{
    public class Brand_Authentication_Service : Authentication_Internal_Service_Base, IAuthentication_Internal_Service
    {

        public Brand_Authentication_Service(IBCRM_Client_Builder client_Builder,
                                                IIAM_Client_Service iamService,
                                                ILogger<Brand_Authentication_Service> logger) : base(client_Builder, iamService, logger)
        {
            this.BrandRef = App_Setting.Brands.Main.Config.Brand_Ref;
            this.AppId = App_Setting.Brands.Main.Config.App_Id;
            this.IAMToken = App_Setting.Brands.Main.Config.App_Token;
        }

        public async override Task<LoginResponse> Login(string brandName, string redirect)
        {
            try
            {
                var response = await base.Login(brandName, redirect);

                var resp = response.Res;

                if (resp.Status == BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success)
                {
                    JObject data = (JObject)resp.Data;

                    using (DuchmillModel.BCRM_36_Entities DutchmillContext = await new BCRM_Brand_Entities_Factory<DuchmillModel.BCRM_36_Entities>().CreateAsync(App_Setting.Brands.Main.Config.Brand_Ref))
                    {
                        DuchmillModel.BCRM_Login_State loginState = new DuchmillModel.BCRM_Login_State()
                        {
                            Status = AppConstants.Authentication.Line.Status.Line_Request,
                            IAM_OAuth_TX_Ref = (String)data["txreference"],
                            Line_OAuth_State = response.LineStage,
                            Updated_DT = TxTimeStamp,
                            Created_DT = TxTimeStamp
                        };

                        DutchmillContext.BCRM_Login_States.Add(loginState);
                        DutchmillContext.SaveChanges();
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task<CallbackResponse> Callback(LineCallbackInfo callbackInfo, CallbackCustomerInfo customerInfo, Dictionary<string, string> payload)
        {
            try
            {
                Dictionary<string, string> bcrm_payload = new Dictionary<string, string>();
                string url_redirect = string.Empty;

                #region Line Info
                dynamic line_Data = JsonConvert.DeserializeObject(callbackInfo.Payload);
                string line_UserId = line_Data.line.user_id;
                string line_Name = line_Data.line.name;
                string line_Picture_Url = line_Data.line.picture_url;
                #endregion

                #region Extrack Params
                //string path = callbackInfo.State.Contains('_') ? callbackInfo.State.Substring(17) : string.Empty;
                string brandName = String.Empty;
                string path = String.Empty;
                string state = String.Empty;

                string[] _params = callbackInfo.State.Split('_');
                if (_params.Length > 0)
                {
                    int index = 0;
                    foreach (var param in _params)
                    {
                        if (index == 0) state = param;
                        if (index == 1) brandName = param;
                        if (index == 2) path = param;
                        index++;
                    }
                }
                #endregion

                var config = App_Setting.Brands.Configs.FirstOrDefault(it => it.Name == brandName);

                if (config == null) throw new Exception("Invalid callback info");

                using (DuchmillModel.BCRM_36_Entities DutchmillContext = await new BCRM_Brand_Entities_Factory<DuchmillModel.BCRM_36_Entities>().CreateAsync(App_Setting.Brands.Main.Config.Brand_Ref))
                {
                    DuchmillModel.BCRM_Login_State checkLineState = DutchmillContext.BCRM_Login_States.Where(o => o.IAM_OAuth_TX_Ref == callbackInfo.TX_Reference && o.Line_OAuth_State == callbackInfo.State).FirstOrDefault();

                    if (checkLineState == null) throw new Exception("รูปแบบคำของไม่ถูกต้อง");

                    checkLineState.Status = AppConstants.Authentication.Line.Status.Line_Callback;
                    DutchmillContext.SaveChanges();

                    DuchmillModel.BCRM_Line_Info user_line_Info = DutchmillContext.BCRM_Line_Infos.Where(o => o.Line_UserId == line_UserId).FirstOrDefault();
                    if (user_line_Info != null)
                    {
                        // update line info              
                        user_line_Info.Line_UserId = line_UserId;
                        user_line_Info.Name = line_Name;
                        user_line_Info.Picture_Url = line_Picture_Url;
                        user_line_Info.Access_Token = line_Data.line.access_token;
                        user_line_Info.Payload = callbackInfo.Payload;
                        user_line_Info.Identity_SRef = callbackInfo.Identity_SRef;
                        user_line_Info.Last_Line_Login_DT = TxTimeStamp;
                        user_line_Info.Updated_DT = TxTimeStamp;
                        user_line_Info.Last_Line_Login_DT = TxTimeStamp;

                        // save last state
                        user_line_Info.IAM_OAuth_TX_Ref = checkLineState.IAM_OAuth_TX_Ref;
                        user_line_Info.Line_OAuth_State = checkLineState.Line_OAuth_State;

                        DutchmillContext.SaveChanges();
                    }
                    else
                    {
                        DuchmillModel.BCRM_Line_Info newLineInfo = new DuchmillModel.BCRM_Line_Info();

                        newLineInfo.Line_UserId = line_UserId;
                        newLineInfo.Name = line_Name;
                        newLineInfo.Picture_Url = line_Picture_Url;
                        newLineInfo.Access_Token = line_Data.line.access_token;
                        newLineInfo.Payload = callbackInfo.Payload;
                        newLineInfo.Identity_SRef = callbackInfo.Identity_SRef;
                        newLineInfo.Last_Line_Login_DT = TxTimeStamp;
                        newLineInfo.Updated_DT = TxTimeStamp;

                        // save last state
                        newLineInfo.IAM_OAuth_TX_Ref = checkLineState.IAM_OAuth_TX_Ref;
                        newLineInfo.Line_OAuth_State = checkLineState.Line_OAuth_State;
                        newLineInfo.Created_DT = TxTimeStamp;

                        DutchmillContext.BCRM_Line_Infos.Add(newLineInfo);
                        DutchmillContext.SaveChanges();

                        user_line_Info = DutchmillContext.BCRM_Line_Infos.Where(o => o.Line_UserId == line_UserId).FirstOrDefault();
                    }

                    // iam - token
                    string iam_Token = callbackInfo.Access_Token;
                    string identity_SRef = callbackInfo.Identity_SRef;

                    checkLineState.Access_Token = line_Data.line.access_token;
                    checkLineState.Payload = callbackInfo.Payload;
                    checkLineState.Identity_SRef = callbackInfo.Identity_SRef;
                    checkLineState.Brand = config.Name;
                    checkLineState.BrandId = config.BrandId;
                    checkLineState.Updated_DT = TxTimeStamp;

                    DutchmillContext.SaveChanges();


                    var customer = (from crm in DutchmillContext.CRM_Customers
                                    join bcrm in DutchmillContext.BCRM_Customers on crm.CustomerId equals bcrm.CRM_CustomerId
                                    where crm.CustomerId == user_line_Info.CRM_CustomerId
                                    select new { bcrm, crm.Identity_SRef }).FirstOrDefault();

                    StringBuilder members = new StringBuilder();

                    if (customer != null)
                    {

                        bcrm_payload[AppConstants.RouteData.Scope] = AppConstants.Token.Scope.API;
                        bcrm_payload[AppConstants.RouteData.TokenId] = Guid.NewGuid().ToString();
                        bcrm_payload[AppConstants.RouteData.Line.LineId] = line_UserId;
                        bcrm_payload[AppConstants.RouteData.Line.Linename] = line_Name;
                        bcrm_payload[AppConstants.RouteData.Identity_SRef] = customer.Identity_SRef;
                        bcrm_payload[AppConstants.RouteData.CustomerId] = user_line_Info.CRM_CustomerId.ToString();
                        bcrm_payload[AppConstants.RouteData.Member] = members.ToString();

                        if (customer.bcrm.Accept_Activity_Consent == true)
                        {
                            if (string.IsNullOrEmpty(path))
                            {
                                url_redirect = $"{config.Frontend_Endpoint}/Main";
                            }
                            else
                            {
                                url_redirect = $"{config.Frontend_Endpoint}/{path}";
                            }
                        }
                        else
                        {
                            url_redirect = $"{config.Frontend_Endpoint}/AcceptCondition";
                        }
                    }
                    else
                    {
                        bcrm_payload[AppConstants.RouteData.Scope] = AppConstants.Token.Scope.OTP;
                        bcrm_payload[AppConstants.RouteData.TokenId] = Guid.NewGuid().ToString();
                        bcrm_payload[AppConstants.RouteData.Line.LineId] = line_UserId;
                        bcrm_payload[AppConstants.RouteData.Line.Linename] = line_Name;

                        url_redirect = $"{config.Frontend_Endpoint}/AcceptPrivacy";
                    }

                }

                var response = await base.Callback(callbackInfo, null, bcrm_payload);

                response.RedirectUrl = $"{url_redirect}?token={response.Token.Access_Token}";

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
