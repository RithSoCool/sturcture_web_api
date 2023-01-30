using BCRM.Common.Api;
using BCRM.Common.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using BCRM_App.Extentions;
using BCRM_App.Configs;
using BCRM.Common.Context;
using Microsoft.EntityFrameworkCore;
using BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Constants;
using System.Linq;
using Newtonsoft.Json;

namespace BCRM_App.Areas.Api
{
    public class API_BCRM_Controller : BCRM_Controller
    {
        public API_BCRM_Controller(ILogger logger,
                                   IBCRM_Exception_Factory bcrm_Ex_Factory,
                                   IHttpContextAccessor httpContext_Accessor) : base(logger, bcrm_Ex_Factory, httpContext_Accessor)
        {
            TxTimeStamp = DateTime.Now;
        }

        public string AccessToken { get; private set; }
        public DateTime TxTimeStamp { get; private set; }

        private int LogId;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            try
            {
                AccessToken = context.HttpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(AccessToken))
                {
                    AccessToken = AccessToken.Split(' ')[1].Trim(' ');
                }
            }
            catch
            {

            }
        }

        internal IBCRM_IdentityContext VerifyAppIdentityContext()
        {
            try
            {
                IBCRM_IdentityContext appIdentityContext = new IdentityContext()
                {
                    RequestId = this.Api_RequestId != null ? this.Api_RequestId : Guid.NewGuid().ToString(),
                };

                appIdentityContext.Set_IAM_Token(App_Setting.Brands.Main.Config.App_Token);
                appIdentityContext.Verify();

                return appIdentityContext;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void Build_BCRM_Exception(Exception ex)
        {
            if (ex is Exception)
            {
                ApiException = _bcrm_Ex_Factory.Build(1000400, ex.GetErrorExceptoin().Message);
            }
            else
            {
                ApiException = ex;
            }
        }
       
    }
}
