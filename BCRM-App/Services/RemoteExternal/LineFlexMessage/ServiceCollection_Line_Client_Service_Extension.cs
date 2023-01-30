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

namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage
{
    public static class ServiceCollection_Line_Client_Service_Extension
    {
        public static void Add_Line_Client_Service(this IServiceCollection services)
        {
            services.AddTransient<ILine_FlexMessage_Client_Service, Line_FlexMessage_Client_Service>(serviceProvider =>
            {
                IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                ILogger<Line_FlexMessage_Client_Service> logger = serviceProvider.GetRequiredService<ILogger<Line_FlexMessage_Client_Service>>();
                Line_FlexMessage_Client_Service iamClientService = new Line_FlexMessage_Client_Service(httpClientFactory, logger);

                // Setting object
                IWebHostEnvironment env = serviceProvider.GetRequiredService<IWebHostEnvironment>();

                BCRM_Client_Settings settings = new BCRM_Client_Settings();
                settings.Http_Client_Name = Line_SC_Constant.Http_Client_Name;
                settings.Set_Api_Token(BCRM_Config.Platform.App.App_Token); // Read from appSettings.{EnvironmentName}.json
                settings.Set_Client_Environment(env);
                settings.Set_Request_MaxRetry(3);
                
                settings.Validate();

                iamClientService.Set_Settings(settings);

          
                if (EnvironmentUtil.IsProduction())
                {
                    iamClientService.Set_Endpoint("https://api.line.me/");
                }
                else
                {
                    iamClientService.Set_Endpoint("https://api.line.me/");
                }

                //iamClientService.Set_Token(Line.ChannelToken);

                // Config response converter (if any)
                iamClientService.Set_RespConverter(Line_ResponseConverter.Convert); 

                return iamClientService;
            });

            // Register HttpClient Name
            services.AddHttpClient(Line_SC_Constant.Http_Client_Name, c =>
            {
                // config HttpClient for IAM Client Service (if any)
            });
        }
    }
}
