using System;
using System.Collections.Generic;
using System.Threading;
using BCRM_App.Configs;
using Newtonsoft.Json;

namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class Get_Customer_Info_Resp
    {
        [JsonIgnore]
        public string Identity_SRef { get; set; }
        [JsonIgnore]
        public string Reference { get; set; }
        public string First_Name_Th { get; set; }
        public string Last_Name_Th { get; set; }
        //public string MobileNo { get; set; }
        //public string Email { get; set; }
        //public int Gender { get; set; }
        //public DateTime? DateOfBirth { get; set; }

        //public string DateOfBirth_Label
        //{
        //    get
        //    {

        //        if (DateOfBirth == null) return "";

        //        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");

        //        return DateOfBirth.Value.ToString("dd/MM/yyyy HH:mm");
        //    }
        //}

        public string ImageProfileUrl { get; set; }
        [JsonIgnore]
        public DateTime? ImageProfileUrl_Expire { get; set; }
        [JsonIgnore]
        public string FilePath { get; set; }
        [JsonIgnore]
        public string FilePath_Full { get; set; }
        public decimal? Point_Balance { get; set; }
        public decimal? Total_Spending { get; set; }
    }

    public class Edit_Customer_Info_Resp
    {
        public string First_Name_Th { get; set; }
        public string Last_Name_Th { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirth_Label
        {
            get
            {

                if (DateOfBirth == null) return "";

                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");

                return DateOfBirth.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public string ImageProfileUrl { get; set; }
    }
}
