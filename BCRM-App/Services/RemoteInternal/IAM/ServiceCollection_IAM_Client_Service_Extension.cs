using BCRM.Common.Configs;
using BCRM.Common.Helpers;
using BCRM.Common.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCRM.Portable.Services.RemoteInternal.IAM
{
    public static class ServiceCollection_IAM_Client_Service_Extension
    {
        public static void Add_IAM_Client_Service(this IServiceCollection services)
        {
            services.AddTransient<IIAM_Client_Service, IAM_Client_Service>(serviceProvider =>
            {
                IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                ILogger<IAM_Client_Service> logger = serviceProvider.GetRequiredService<ILogger<IAM_Client_Service>>();
                IAM_Client_Service iamClientService = new IAM_Client_Service(httpClientFactory, logger);

                // Setting object
                IWebHostEnvironment env = serviceProvider.GetRequiredService<IWebHostEnvironment>();

                BCRM_Client_Settings settings = new BCRM_Client_Settings();
                settings.Http_Client_Name = IAM_SC_Constant.Http_Client_Name;
                settings.Set_Api_Token(BCRM_Config.Platform.App.App_Token); // Read from appSettings.{EnvironmentName}.json
                settings.Set_Client_Environment(env);
                settings.Set_Request_MaxRetry(3);
                
                settings.Validate();

                // Setting to IAM_Client_Service
                iamClientService.Set_Settings(settings);
                //iamClientService.Set_Service(BCRM_Client_Const.Service.IAM); // Endpoint URL จะถูกเลือกตาม environment

                // In IAM case Endpoint URL is set from IAM_Client_Service.Set_Service();\
                if (EnvironmentUtil.IsLocal())
                {
                    //iamClientService.Set_Endpoint("https://localhost:44314");
                    iamClientService.Set_Endpoint(BCRM_Client_Const.Service.Endpoint.Development.IAM);
                }
                else if (EnvironmentUtil.IsProduction())
                {
                    iamClientService.Set_Endpoint(BCRM_Client_Const.Service.Endpoint.Production.IAM);
                }
                else
                {
                    iamClientService.Set_Endpoint(BCRM_Client_Const.Service.Endpoint.Development.IAM);
                }

                iamClientService.Set_Token(BCRM_Config.Platform.App.App_Token);

                // Config response converter (if any)
                iamClientService.Set_RespConverter(IAM_ResponseConverter.Convert); 

                return iamClientService;
            });

            // Register HttpClient Name
            services.AddHttpClient(IAM_SC_Constant.Http_Client_Name, c =>
            {
                // config HttpClient for IAM Client Service (if any)
            });
        }
    }
}
