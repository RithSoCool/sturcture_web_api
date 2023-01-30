using BCRM.Common.Api.Response;
using BCRM.Common.Helpers;
using BCRM.Common.Services;
using BCRM.Common.Services.RemoteInternal.IAM.Model;
using BCRM_App.Areas.Api.Models;
using BCRM_App.Configs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;
using BCRM.Common.Services.RemoteInternal.IAM;
using BCRM_App.Services.RemoteInternal.Authentication.Models;
using Microsoft.Extensions.Logging;

namespace BCRM_App.Services.RemoteInternal.Authentication
{
    public interface IAuthentication_Internal_Service
    {
        public string AppId { get; set; }
        public string BrandRef { get; set; }
        public Task<LoginResponse> Login(string brandName, string redirect);
        public Task<CallbackResponse> Callback(LineCallbackInfo callbackInfo, CallbackCustomerInfo customerInfo, Dictionary<string, string> payload);
    }

    public partial class Authentication_Internal_Service_Base : IAuthentication_Internal_Service
    {
        private readonly IIAM_Client_Service iamService;
        private readonly IBCRM_Client_Builder _client_Builder;

        public Authentication_Internal_Service_Base(IBCRM_Client_Builder client_Builder,
                                  IIAM_Client_Service iamService,
                                  ILogger<Authentication_Internal_Service_Base> logger)
        {
            _client_Builder = client_Builder;
            this.iamService = iamService;
            Logger = logger;
            TxTimeStamp = DateTime.Now;
        }

        public ILogger Logger { get; private set; }
        public DateTime TxTimeStamp { get; private set; }
        public string BrandRef { get; set; }
        public string AppId { get; set; }
        public string IAMToken { get; set; }

        public virtual async Task<LoginResponse> Login(string brandName, string redirect)
        {
            try
            {
                var config = App_Setting.Brands.Configs.FirstOrDefault(it => it.Name == brandName);

                if (config == null) throw new Exception("Request has invalid signature");

                String lineState = StringHelper.Instance.RandomString(16);

                lineState = $"{lineState}_{config.Name}";

                if (!string.IsNullOrEmpty(redirect)) lineState = $"{lineState}_{redirect}";

                IBCRM_Client client = _client_Builder.Set_Service(BCRM_Client_Const.Service.IAM)
                                      .Set_Token(config.App_Token)
                                      .Client();

                client.Set_Header("bcrm-app-secret", config.App_Secret);

                var payload = new
                {
                    App_Id = config.App_Id,
                    Provider_Ref = config.Provider_Ref,
                    State = lineState,
                    Callback_Url = $"{config.Backend_Endpoint}/Api/v1.0/Authentication/Callback"
                };

                LoginResponse loginResponse = new LoginResponse();

                client.Request_v2(null, BCRM.IAM.Constants.BCRM_IAM_Const.Api.Url.OAuth.Request, payload, HttpMethod.Post,
                (respData) =>
                {
                    var resp_Wrp = respData as BCRM_Client_Resp;
                    BCRM_Api_Response resp = resp_Wrp.Response;

                    if (resp.Status == BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success)
                    {
                        JObject data = (JObject)resp.Data;
                        string redirectUrl = (String)data["url"];

                        loginResponse.LineStage = lineState;
                        loginResponse.Res = resp;
                        loginResponse.RedirectUrl = redirectUrl;

                        return true;
                    }

                    return false;
                },
                 (respData) =>
                {
                    return false;
                });

                return loginResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<CallbackResponse> Callback(LineCallbackInfo callbackInfo, CallbackCustomerInfo customerInfo, Dictionary<string, string> payload)
        {
            try
            {
                if (callbackInfo == null) throw new Exception("Invalid callback info");

                if (callbackInfo.Status != BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success) throw new Exception("invalid callback info");

                var config = App_Setting.Brands.Configs.FirstOrDefault(it => it.App_Id == callbackInfo.App_Id);

                if (config == null) throw new Exception("Invalid callback info");

                var IAM_Response = await iamService.TokenExchangeAsync(callbackInfo.Access_Token, config.Brand_Ref, "", payload);

                if (!IAM_Response.Success) throw IAM_Response.ResponseError.Exception;

                IAM_Token_Exchange_Resp tokenExchange = (IAM_Token_Exchange_Resp)IAM_Response.ResponseSuccess;

                CallbackResponse callbackResponse = new CallbackResponse()
                {
                    RedirectUrl = "",
                    Token = tokenExchange,
                };

                return callbackResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
