using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading;

namespace BCRM_App.Areas.Api.Services.Document.Models
{
    public class Upload_Req
    {
        public Upload_Req()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
        }

        public int? StoreId { get; set; }
        [Required]
        public string Store { get; set; }
        [Required]
        public string BillingNo { get; set; }
        [Required]
        public DateTime BillingDate { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }

        public string First_Name_Th { get; set; }
        public string Last_Name_Th { get; set; }
        public string MobileNo { get; set; }
    }
}
