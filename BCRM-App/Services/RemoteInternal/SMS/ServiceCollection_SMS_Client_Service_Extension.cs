using BCRM.Common.Configs;
using BCRM.Common.Services;
using BCRM_App.Configs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCRM_App.Services.RemoteInternal.SMS
{
    public static class ServiceCollection_SMS_Client_Service_Extension
    {
        public static void Add_SMS_Client_Service(this IServiceCollection services)
        {
            services.AddTransient<ISMS_Client_Service, SMS_Client_Service>(serviceProvider =>
            {
                IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                ILogger<SMS_Client_Service> logger = serviceProvider.GetRequiredService<ILogger<SMS_Client_Service>>();
                SMS_Client_Service smsClientService = new SMS_Client_Service(httpClientFactory, logger);

                // Setting object
                IWebHostEnvironment env = serviceProvider.GetRequiredService<IWebHostEnvironment>();

                BCRM_Client_Settings settings = new BCRM_Client_Settings();
                settings.Http_Client_Name = SMS_SC_Constant.Http_Client_Name;
                
                settings.Set_Api_Token(BCRM_Config.Platform.App.App_Token); // Read from appSettings.{EnvironmentName}.json
                settings.Set_Client_Environment(env);
                settings.Set_Request_MaxRetry(3);

                settings.Validate();

                // Setting to IAM_Client_Service
                smsClientService.Set_Settings(settings);

                if (env.IsProduction())smsClientService.Set_Endpoint(SMS_SC_Constant.Service.Endpoint.Production.Url);
                else smsClientService.Set_Endpoint(SMS_SC_Constant.Service.Endpoint.Development.Url);

                smsClientService.Set_Token(App_Setting.SMS.AdHoc_Token);

                // Config response converter (if any)
                smsClientService.Set_RespConverter(SMS_ResponseConverter.Convert);

                return smsClientService;
            });

            // Register HttpClient Name
            services.AddHttpClient(SMS_SC_Constant.Http_Client_Name, c =>
            {
                // config HttpClient for IAM Client Service (if any)
            });
        }
    }
}
