using BCRM.Common.Configs;
using BCRM_App.Configs;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Services.RemoteInternal.Authentication;
using BCRM_App.Areas.Api.Services.SMS;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM_App.Areas.Api.Services.Repository.Customer;
using static BCRM_App.Constants.AppConstants.Database.ConnectionString;
using BCRM_App.Areas.Api.Services.Repository.Wallet;
using BCRM_App.Areas.Api.Services.Privilege;
using BCRM.Common.Context;
using BCRM_App.Areas.Api.Services.Document;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models;

namespace BCRM_App.Extentions
{
    public static class ConfigureServicesExtentions
    {
        public static void Register_BCRM_Internal_Service(this IServiceCollection services)
        {
            var customerServiceDescriptor = new ServiceDescriptor(typeof(ICustomer_Service), typeof(Customer_Internal_Service), ServiceLifetime.Transient);

            var privilegeServiceDescriptor = new ServiceDescriptor(typeof(IPrivilege_Service), typeof(Privilege_Internal_Service), ServiceLifetime.Transient);

            //var documentInternalServiceDescriptor = new ServiceDescriptor(typeof(IDocument_Internal_Service), typeof(Document_Internal_Service), ServiceLifetime.Transient);
            var documentServiceDescriptor = new ServiceDescriptor(typeof(IDocument_Internal_Service), typeof(Brand_Document_Service), ServiceLifetime.Transient);

            services.AddTransient<IBCRM_Customer_Repository, BCRM_Customer_Repository>(); // สำหรับ Singerton service


            // BCRM - Line Flex Message Template Builder
            services.AddScoped<FlexMessageBuilder>();

            services.AddScoped<Wallet_Repository>();
            services.AddScoped<Line_Repository>();
            services.AddScoped<Address_Repository>();

            //services.Add(documentInternalServiceDescriptor);
            services.Add(documentServiceDescriptor);
            services.Add(customerServiceDescriptor);
            services.Add(privilegeServiceDescriptor);
        }

        public static void Register_SMS_Internal_Service(this IServiceCollection services)
        {
            var DutchmillSmsBaseDescriptor = new ServiceDescriptor(typeof(ISMS_Internal_Service), typeof(Brand_SMS_Service), ServiceLifetime.Scoped);
            services.Add(DutchmillSmsBaseDescriptor);
        }

        public static void Register_Authen_Internal_Service(this IServiceCollection services)
        {
            var DutchmillAuthenDescriptor = new ServiceDescriptor(typeof(IAuthentication_Internal_Service), typeof(Brand_Authentication_Service), ServiceLifetime.Transient);

            services.Add(DutchmillAuthenDescriptor);
        }

        public static void Register_Database(this IServiceCollection services)
        {
            // Dutchmill DB

            Dutchmill = BCRM_Config.Credentials.Database.BCRM.ConnectionString(App_Setting.Brands.Main.Config.Brand_Ref);
            
            services.AddDbContext<BCRM_36_Entities>(options =>
            {
                options.UseSqlServer(Dutchmill,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
                });
            }, ServiceLifetime.Transient);
        }
    }
}
