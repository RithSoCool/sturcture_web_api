using Newtonsoft.Json;
using System;


namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class MemberFromLastCampaign
    {
        public string First_Name_Th { get; set; }
        public string Last_Name_Th { get; set; }
        public int? Gender
        {
            get
            {
                switch (GenderOld)
                {
                    case "O": return 9;
                    case "M": return 1;
                    case "F": return 2;
                    default:
                        return 9;
                }
            }
        }

        [JsonIgnore]
        public string GenderOld { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long? SubDistrictId { get; set; }
        public string SubDistrict { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Line_UserId { get; set; }
    }
}
