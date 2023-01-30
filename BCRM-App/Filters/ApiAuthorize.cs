using BCRM.Common.Configs;
using BCRM.Common.Factory;
using BCRM.Common.Factory.Entities.Brand;
using BCRM.Common.Filters.Authorize;
using BCRM_App.Configs;
using BCRM_App.Constants;
using BCRM_App.Extentions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Filters
{
    public class ApiAuthorize : BCRM_Authorize
    {
        public string brandScope { get; private set; }

        public string[] Roles { get; private set; }

        public ApiAuthorize(string brandScope, params string[] roles) : base(String.Empty, "")
        {
            this.brandScope = brandScope;
            Roles = roles;
        }

        public ApiAuthorize(string brandRef, string app_id, string scopes) : base(brandRef, app_id, scopes)
        {
        }

        public override void ValidateExtraPayloads(AuthorizationFilterContext filterContext, JwtSecurityToken jwt_RawToken, Dictionary<string, object> dictExtraPayloads)
        {
            try
            {
                if (brandScope != AppConstants.Authentication.Scope.ThirdPartyApi)
                {
                    if (brandScope != (string)dictExtraPayloads[AppConstants.RouteData.Scope]) throw _bcrm_Ex_Factory.Build(1000401, "invalid token scope");

                    try
                    {
                        string LineId = (string)dictExtraPayloads[AppConstants.RouteData.Line.LineId];
                        AddRouteData(filterContext, AppConstants.RouteData.Line.LineId, LineId);
                    }
                    catch { }

                    try
                    {
                        string LineName = (string)dictExtraPayloads[AppConstants.RouteData.Line.Linename];
                        AddRouteData(filterContext, AppConstants.RouteData.Line.Linename, LineName);
                    }

                    catch { }
                    try
                    {
                        string CustomerId = (string)dictExtraPayloads[AppConstants.RouteData.CustomerId];
                        AddRouteData(filterContext, AppConstants.RouteData.CustomerId, CustomerId);
                    }
                    catch { }
                    try
                    {

                        string Identity_SRef = (string)dictExtraPayloads[AppConstants.RouteData.Identity_SRef];
                        AddRouteData(filterContext, AppConstants.RouteData.Identity_SRef, Identity_SRef);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public class ApiKeyFilter : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "X-API-KEY";
        private const string TokenHeader = "Authorization";

        private readonly List<string> api_key_localtions;
        private IBCRM_Exception_Factory _ccExFactory;

        public ApiKeyFilter(string[] api_key_localtions)
        {
            this.api_key_localtions = api_key_localtions.ToList();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            _ccExFactory = (IBCRM_Exception_Factory)context.HttpContext.RequestServices.GetService(typeof(IBCRM_Exception_Factory));

            if (!context.HttpContext.Request.Headers.TryGetValue(TokenHeader, out var extractedApiKey)) throw new Exception("Token invalid.");

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var _api_key_localtions = new List<string>(api_key_localtions);

            if (api_key_localtions != null)
            {
                var apiKey = appSettings.GetSection(_api_key_localtions.FirstOrDefault())._BuildSectionSetting(_api_key_localtions).Value;
                if (context.HttpContext.Request.Headers.TryGetValue(TokenHeader, out var extractedApiKey1))
                {
                    var apikey = extractedApiKey1.FirstOrDefault().Replace("bearer ", "");
                    if (apiKey != apikey ) throw new Exception("Token invalid.");
                }
            }

            await next();
        }
    }
}
