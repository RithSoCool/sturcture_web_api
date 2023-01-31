using BCRM_App.Constants;
using BCRM_App.Extentions;
//using BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BCRM_App.Configs
{
    public static class App_Setting
    {
        public class Brands
        {
            public static List<BrandConfigs> Configs { get; set; } // all configs

            public class Main
            {
                public static BrandConfigs Config { get; set; }
            }
        }


        public class Privilege
        {
            public class Expire_Configs
            {
                public static int Day { get; set; }
                public static int Hour { get; set; }
                public static int Minute { get; set; }
                public static int Sec { get; set; }

                public static (double? sec, TimeSpan? exp) GetExpireTime()
                {
                    TimeSpan exp = new TimeSpan(Expire_Configs.Day,
                                                Expire_Configs.Hour,
                                                Expire_Configs.Minute,
                                                Expire_Configs.Sec);

                    var sec = exp.TotalSeconds;

                    if (sec > 60 * 60 * 1) return (sec: null, exp: exp);
                    return (sec: sec, exp: null);
                }
            }
        }

        public class SMS
        {
            public static string SenderName { get; set; }
            public static string AdHoc_Token { get; set; }
        }

        public static void Load_Configs(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var configuration = sp.GetService<IConfiguration>();
            var smsConfigs = configuration.GetSection("SMS");
            var brandConfigs = configuration.GetSection("Brands");
            var thirdPartyConfigs = configuration.GetSection("ThirdPartyApi");
            var configPrivilege = configuration.GetSection("Privilege");

            App_Setting.Brands.Configs = new List<BrandConfigs>();

            #region Yuzu Group Config
            BrandConfigs MainConfig = new BrandConfigs();
            MainConfig.BrandId = Int32.Parse(brandConfigs.BuildSectionSetting("Brands.Main.BrandId").Value);
            MainConfig.Name = brandConfigs.BuildSectionSetting("Brands.Main.Name").Value;
            MainConfig.Brand_Ref = brandConfigs.BuildSectionSetting("Brands.Main.Brand_Ref").Value;
            MainConfig.Provider_Ref = brandConfigs.BuildSectionSetting("Brands.Main.Provider_Ref").Value;
            MainConfig.Backend_Endpoint = brandConfigs.BuildSectionSetting("Brands.Main.Backend_Endpoint").Value;
            MainConfig.Frontend_Endpoint = brandConfigs.BuildSectionSetting("Brands.Main.Frontend_Endpoint").Value;
            MainConfig.App_Id = brandConfigs.BuildSectionSetting("Brands.Main.App.App_Id").Value;
            MainConfig.App_Secret = brandConfigs.BuildSectionSetting("Brands.Main.App.App_Secret").Value;
            MainConfig.App_Identity_Ref = brandConfigs.BuildSectionSetting("Brands.Main.App.App_Identity_Ref").Value;
            MainConfig.App_Identity_SRef = brandConfigs.BuildSectionSetting("Brands.Main.App.App_Identity_SRef").Value;
            MainConfig.App_Token = brandConfigs.BuildSectionSetting("Brands.Main.App.App_Token").Value;
            MainConfig.AssetId = Int32.Parse(brandConfigs.BuildSectionSetting("Brands.Main.Wallet.AssetId").Value);
            MainConfig.Privilege_Inventory_Ref = brandConfigs.BuildSectionSetting("Brands.Main.Privilege.Inventory_Ref").Value;
            MainConfig.Point_Rate = Int32.Parse(brandConfigs.BuildSectionSetting("Brands.Main.Point.Rate").Value);
            MainConfig.Line_Flex_Message_Token = brandConfigs.BuildSectionSetting("Brands.Main.Line.Flex_Message_Token").Value;
            MainConfig.Welcome_Point = Int32.Parse(brandConfigs.BuildSectionSetting("Brands.Main.Point.Welcome_Point").Value);
            MainConfig.Document_Alt_Reference = brandConfigs.BuildSectionSetting("Brands.Main.Document.Alt_Reference").Value;
            Brands.Main.Config = MainConfig;

            App_Setting.Brands.Configs.Add(MainConfig);
            #endregion

            #region SMS
            SMS.SenderName = smsConfigs.BuildSectionSetting("SMS.SenderName").Value;
            SMS.AdHoc_Token = smsConfigs.BuildSectionSetting("SMS.AdHoc_Token").Value;
            #endregion

            #region Privilege

            var _year = configPrivilege.BuildSectionSetting("Privilege.Expire_Configs.Year").Value;
            var _month = configPrivilege.BuildSectionSetting("Privilege.Expire_Configs.Month").Value;
            var _day = configPrivilege.BuildSectionSetting("Privilege.Expire_Configs.Day").Value;
            var _hour = configPrivilege.BuildSectionSetting("Privilege.Expire_Configs.Hour").Value;
            var _minute = configPrivilege.BuildSectionSetting("Privilege.Expire_Configs.Minute").Value;
            var _sec = configPrivilege.BuildSectionSetting("Privilege.Expire_Configs.Sec").Value;

            //Privilege.Expire_Configs.Year = int.Parse(_year);
            //Privilege.Expire_Configs.Month = int.Parse(_month);
            Privilege.Expire_Configs.Day = int.Parse(_day);
            Privilege.Expire_Configs.Hour = int.Parse(_hour);
            Privilege.Expire_Configs.Minute = int.Parse(_minute);
            //Privilege.Expire_Configs.Second = int.Parse(_sec);

            #endregion
        }
    }
}
