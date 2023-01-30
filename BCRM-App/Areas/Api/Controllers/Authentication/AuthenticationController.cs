using BCRM.Common.Factory;
using BCRM.Common.Filters.Action;
using BCRM.Common.Services;
using BCRM_App.Areas.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static BCRM.Common.Constants.BCRM_Core_Const.Api.Filter;
using System.Collections.Generic;
using BCRM_App.Services.RemoteInternal.Authentication;
using Microsoft.AspNetCore.Authorization;
using BCRM_App.Configs;
using BCRM.Common.Constants;
using BCRM_App.Filters;
using BCRM_App.Constants;
using BCRM.Common.Services.Implement;
using System.Text;
using System.Security.Cryptography;
using BCRM_App.Models.DBModels.Duchmill;
using Microsoft.EntityFrameworkCore;
using JWT;
using System.Linq;
using Newtonsoft.Json;

namespace BCRM_App.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiVersion("1.0")]
    public partial class AuthenticationController : API_BCRM_Controller
    {
        private readonly IAuthentication_Internal_Service authentication;
        private readonly IBCRM_Client_Builder _client_Builder;
        private readonly IJWTToken_Service _jwtToken;

        public AuthenticationController(ILogger<AuthenticationController> logger,
                                        IBCRM_Exception_Factory bcrm_Ex_Factory,
                                        IAuthentication_Internal_Service authentication,
                                        IJWTToken_Service jwtToken,
                                        IHttpContextAccessor httpContext_Accessor, IBCRM_Client_Builder client_Builder) : base(logger, bcrm_Ex_Factory, httpContext_Accessor)
        {
            this.authentication = authentication;
            _client_Builder = client_Builder;
            _jwtToken = jwtToken;
        }


        [BCRM_AcceptVerb(BCRM_HttpMethods.Get | BCRM_HttpMethods.Post)]
        public async Task<IActionResult> Request(string brandName, string redirect)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                if (string.IsNullOrEmpty(brandName)) brandName = App_Setting.Brands.Main.Config.Name;

                var loginResponse = await authentication.Login(brandName, redirect);
                return Redirect(loginResponse.RedirectUrl);
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get | BCRM_HttpMethods.Post)]
        public async Task<IActionResult> Callback([FromQuery] LineCallbackInfo req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                var callbackResponse = await authentication.Callback(req, null, null);
                return Redirect(callbackResponse.RedirectUrl);
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

    }
}
