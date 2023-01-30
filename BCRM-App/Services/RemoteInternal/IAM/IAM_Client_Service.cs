using BCRM.Common.Configs;
using BCRM.Common.Services;
using BCRM.Portable.Services.RemoteInternal.IAM.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM.Portable.Services.RemoteInternal.IAM
{
    public interface IIAM_Client_Service
    {
        Task<IAM_Response> AccountCreateAsync(string provider_ref, string identity_ref, string username, string password, string email, string country_code, string mobile_no);
        Task<IAM_Response> AccountLoginAsync(string provider_ref, string username, string password, dynamic payload);
        Task<IAM_Response> AccountLogoutAsync(string access_token);

        /// <summary>
        /// OauthRequestAsync(), OauthRequestGoogleAsync(), OauthRequestLineAsync() 
        /// ทำงานเหมือนกันแต่มี overload function ไว้เพื่อให้อ่านทำความเข้าใจ code ง่ายๆ เท่านั้น
        /// </summary>
        Task<IAM_Response> OauthRequestAsync(string provider_ref, string state, string callbackUrl);

        /// <summary>
        /// OauthRequestAsync(), OauthRequestGoogleAsync(), OauthRequestLineAsync() 
        /// ทำงานเหมือนกันแต่มี overload function ไว้เพื่อให้อ่านทำความเข้าใจ code ง่ายๆ เท่านั้น
        /// </summary>
        /// 
        Task<IAM_Response> OauthRequestGoogleAsync(string provider_ref, string state, string callbackUrl);
        /// <summary>
        /// OauthRequestAsync(), OauthRequestGoogleAsync(), OauthRequestLineAsync() 
        /// ทำงานเหมือนกันแต่มี overload function ไว้เพื่อให้อ่านทำความเข้าใจ code ง่ายๆ เท่านั้น
        /// </summary>
        Task<IAM_Response> OauthRequestLineAsync(string provider_ref, string state, string callbackUrl);
        Task<IAM_Response> TokenRefreshAsync(string access_token, string refresh_token);
        Task<IAM_Response> TokenExchangeAsync(string access_token, string brand_ref, string scope, Dictionary<string, string> payload);
    }



    public class IAM_Client_Service : BCRM_Client, IIAM_Client_Service
    {
        private readonly ILogger<IAM_Client_Service> _logger;

        public IAM_Client_Service(IHttpClientFactory clientFactory, ILogger<IAM_Client_Service> logger) : base(clientFactory)
        {
            _logger = logger;
        }

        public async Task<IAM_Response> AccountCreateAsync(string provider_ref, string identity_ref, string username, string password, string email, string country_code, string mobile_no)
        {
            var reqParams = new
            {
                provider_ref = provider_ref,
                identity_ref = identity_ref,
                username = username,
                password = password,
                email = email,
                country_code = country_code,
                mobile_no = mobile_no
            };


            IAM_Response result = new IAM_Response();

            result.Success = await PostAsync(
                IAM_SC_Constant.Service.Api.Action.Account_Create
                , IAM_SC_Constant.Service.Api.Path.v1.Account_Create
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<IAM_Response> AccountLoginAsync(string provider_ref, string username, string password, dynamic payload)
        {
            var reqParams = new
            {
                provider_ref = provider_ref,
                app_id = BCRM_Config.Platform.App.App_Id,
                username = username,
                password = password,
                payload = payload
            };

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("bcrm-app-secret", BCRM_Config.Platform.App.App_Secret);

            IAM_Response result = new IAM_Response();

            result.Success = await RequestAsync(
                IAM_SC_Constant.Service.Api.Action.Account_Login
                , IAM_SC_Constant.Service.Api.Path.v1.Account_Login
                , reqParams
                , headers
                , HttpMethod.Post
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<IAM_Response> AccountLogoutAsync(string access_token)
        {
            var reqParams = new
            {
                app_id = BCRM_Config.Platform.App.App_Id,
                access_token = access_token
            };

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("bcrm-app-secret", BCRM_Config.Platform.App.App_Secret);

            IAM_Response result = new IAM_Response();

            result.Success = await RequestAsync(
                IAM_SC_Constant.Service.Api.Action.Account_Logout
                , IAM_SC_Constant.Service.Api.Path.v1.Account_Logout
                , reqParams
                , headers
                , HttpMethod.Post
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<IAM_Response> OauthRequestAsync(string provider_ref, string state, string callbackUrl)
        {
            var reqParams = new
            {
                provider_ref = provider_ref,
                app_id = BCRM_Config.Platform.App.App_Id,
                state = state,
                callback_url = callbackUrl
            };

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("bcrm-app-secret", BCRM_Config.Platform.App.App_Secret);

            IAM_Response result = new IAM_Response();

            result.Success = await RequestAsync(
                IAM_SC_Constant.Service.Api.Action.Oauth_Request
                , IAM_SC_Constant.Service.Api.Path.v1.Oauth_Request
                , reqParams
                , headers
                , HttpMethod.Post
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<IAM_Response> OauthRequestGoogleAsync(string provider_ref, string state, string callbackUrl)
        {
            var reqParams = new
            {
                provider_ref = provider_ref,
                app_id = BCRM_Config.Platform.App.App_Id,
                state = state,
                callback_url = callbackUrl
            };

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("bcrm-app-secret", BCRM_Config.Platform.App.App_Secret);

            IAM_Response result = new IAM_Response();

            result.Success = await RequestAsync(
                IAM_SC_Constant.Service.Api.Action.Oauth_RequestGoogle
                , IAM_SC_Constant.Service.Api.Path.v1.Oauth_RequestGoogle
                , reqParams
                , headers
                , HttpMethod.Post
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<IAM_Response> OauthRequestLineAsync(string provider_ref, string state, string callbackUrl)
        {
            var reqParams = new
            {
                provider_ref = provider_ref,
                app_id = BCRM_Config.Platform.App.App_Id,
                state = state,
                callback_url = callbackUrl
            };

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("bcrm-app-secret", BCRM_Config.Platform.App.App_Secret);

            IAM_Response result = new IAM_Response();

            result.Success = await RequestAsync(
                IAM_SC_Constant.Service.Api.Action.Oauth_RequestLine
                , IAM_SC_Constant.Service.Api.Path.v1.Oauth_RequestLine
                , reqParams
                , headers
                , HttpMethod.Post
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<IAM_Response> TokenExchangeAsync(string access_token, string brand_ref, string scope, Dictionary<string, string> payload)
        {
            var reqParams = new
            {
                token = access_token,
                brand_ref = brand_ref,
                scope = scope,
                payload = payload
            };

            IAM_Response result = new IAM_Response();

            result.Success = await PostAsync(
                IAM_SC_Constant.Service.Api.Action.Token_Exchange
                , IAM_SC_Constant.Service.Api.Path.v1.Token_Exchange
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<IAM_Response> TokenRefreshAsync(string access_token, string refresh_token)
        {
            var reqParams = new
            {
                access_token = access_token,
                refresh_token = refresh_token,
                app_id = BCRM_Config.Platform.App.App_Id
            };

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("bcrm-app-secret", BCRM_Config.Platform.App.App_Secret);


            IAM_Response result = new IAM_Response();

            result.Success = await RequestAsync(
                IAM_SC_Constant.Service.Api.Action.Token_RefreshToken
                , IAM_SC_Constant.Service.Api.Path.v1.Token_RefreshToken
                , reqParams
                , headers
                , HttpMethod.Post
                , (resp) => { result.ResponseSuccess = resp;  }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }
    }
}
