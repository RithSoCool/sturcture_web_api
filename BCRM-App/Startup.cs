using BCRM.Common.Configs;
using BCRM.Common.Constants;
using BCRM.Common.Extensions;
using BCRM.Common.Helpers;
using BCRM.Logging;
using BCRM.Logging.Extension;
//using BCRM_App.Areas.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Net.Http;
using BCRM_App.Configs;
using BCRM_App.Extentions;
using BCRM.Common.Services.RemoteInternal.IAM;
using BCRM_App.Services.RemoteInternal.SMS;
using System;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using Microsoft.AspNetCore.Http;
using BCRM.Common.Services.Data;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage;
using BCRM_App.Areas.Api;

namespace BCRM_App
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Application Insights
            services.AddApplicationInsightsTelemetry();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new LowerCaseNamingStrategy() })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .Add_BCRMLogging_Internal();            

            // BCRM - Register App
            services.Register_BCRM_App(Configuration, Access_Mode: BCRM_Core_Const.Credentials.AccessMode.External);

            // BCRM - Client
            services.AddBCRM_Client(setup =>
            {
                setup.Set_Client_Environment(WebHostEnvironment);
                setup.Set_Request_MaxRetry(3); 
                setup.UseApiEndpointHttpMessageHandler(sp => new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => { return true; }
                });
            });

            // BCRM - Common
            services.AddBCRM_Common(Configuration, setting =>
            {
                setting.Preload_Default_Credentials = true;
                setting.Data_DB_Access_Mode = BCRM_Core_Const.Credentials.Database.Access.Read;
            });

            services.AddHttpContextAccessor();

            // BCRM - Common
            //services.AddBCRM_Common(Configuration);

            // BCRM - CRM
            services.AddBCRM_CRM(Configuration);

            // BCRM - IAM
            services.Add_IAM_Client_Service();

            // Load Config
            App_Setting.Load_Configs(services);

            // Register BCRM Database (All Brands)
            //services.Register_Database();

            // Register Authen Internal Service (All Brands)
            //services.Register_Authen_Internal_Service();

            // Register SMS Service
            services.Add_SMS_Client_Service();

            // BCRM - StorageService
            services.AddBCRM_Storage(Configuration);

            // BCRM - Document Service
            services.AddBCRM_Document(Configuration);

            // BCRM - Privilege
            services.AddBCRM_Privilege(Configuration);

            // Register SMS Internal Service (All Brands)
            //services.Register_SMS_Internal_Service();

            // Register Repository Service
            //services.Register_BCRM_Internal_Service();

            // BCRM - Line Flex Message
            services.Add_Line_Client_Service();

            // BCRM - Line Flex Message Template Builder
            //services.AddScoped<FlexMessageBuilder>();


            // BCRM - Logging
            BCRM_Logging_AzureAnalytics_Configs sink_Hot = BCRM_Logging_Sink.Read_Sink_Settings(Configuration, "BCRM_Logging:Sinks:bcrm-hot") as BCRM_Logging_AzureAnalytics_Configs;
            BCRM_Logging_AzureStorage_Configs sink_Cold = BCRM_Logging_Sink.Read_Sink_Settings(Configuration, "BCRM_Logging:Sinks:bcrm-cold") as BCRM_Logging_AzureStorage_Configs;

            sink_Hot.Credentials_FromKeyVault(BCRM_Config.Azure.KeyVault.Endpoint.External, BCRM_Core_Const.Azure.KeyVault.Secrets.BCRM_Apps_Log_Analytics);
            sink_Cold.Credentials_FromKeyVault(BCRM_Config.Azure.KeyVault.Endpoint.External, BCRM_Core_Const.Azure.KeyVault.Secrets.BCRM_Apps_Log_BlobStorage);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .BCRM_Enrich()
                .BCRM_Destructure()
                // Sink - Azure Analytics
                .BCRM_AZ_Analytics(sink_Hot)
                // Sink - Azure Storage
                .BCRM_AZ_Storage(sink_Cold)
                .CreateLogger();

            // Api - Versioning
            services.AddApiVersioning(options => {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Auto Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(configurePolicy =>
                            configurePolicy.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            }
            else
            {
                app.UseCors(configurePolicy =>
                    configurePolicy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                //app.UseCors(configurePolicy => configurePolicy
                //    .WithOrigins("https://bcrm-dutchmill.azurewebsites.net", "https://bcrm-platform-bo.azurewebsites.net")
                //    .AllowAnyMethod()
                //    .AllowAnyHeader());
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Area - Api
                BCRM_App_Api_RouteConfig.Config(endpoints);

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
