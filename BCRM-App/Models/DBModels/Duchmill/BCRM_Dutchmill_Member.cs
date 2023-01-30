using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_Dutchmill_Member
    {
        public int MemberId { get; set; }
        public int? CCUserId { get; set; }
        public int? StoreGroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public string Address { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public int? SubDistrictId { get; set; }
        public string SubDistrict { get; set; }
        public string PostalCode { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public string LineID { get; set; }
        public string LineName { get; set; }
        public string LineChannelId { get; set; }
        public string LinePictureUrl { get; set; }
        public string ImageProfileUrl { get; set; }
        public int Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int? BrandRegister { get; set; }
        public string DutchmillShippingFirstName { get; set; }
        public string DutchmillShippingLastName { get; set; }
        public string DutchmillShippingMobileNo { get; set; }
        public string DutchmillShippingAddress { get; set; }
        public int? DutchmillShippingProvinceId { get; set; }
        public string DutchmillShippingProvince { get; set; }
        public int? DutchmillShippingDistrictId { get; set; }
        public string DutchmillShippingDistrict { get; set; }
        public int? DutchmillShippingSubDistrictId { get; set; }
        public string DutchmillShippingSubDistrict { get; set; }
        public string DutchmillShippingPostalCode { get; set; }
        public int? DutchmillShippingCountryId { get; set; }
        public string DutchmillShippingCountry { get; set; }
        public string DinaShippingFirstName { get; set; }
        public string DinaShippingLastName { get; set; }
        public string DinaShippingMobileNo { get; set; }
        public string DinaShippingAddress { get; set; }
        public int? DinaShippingProvinceId { get; set; }
        public string DinaShippingProvince { get; set; }
        public int? DinaShippingDistrictId { get; set; }
        public string DinaShippingDistrict { get; set; }
        public int? DinaShippingSubDistrictId { get; set; }
        public string DinaShippingSubDistrict { get; set; }
        public string DinaShippingPostalCode { get; set; }
        public int? DinaShippingCountryId { get; set; }
        public string DinaShippingCountry { get; set; }
        public bool? DutchmillConsent { get; set; }
        public bool? DutchmillCondition { get; set; }
        public bool? DinaConsent { get; set; }
        public bool? DinaCondition { get; set; }
        public bool? IsDinaLoginFirstTime { get; set; }
        public int? DinaStatus { get; set; }
        public DateTime? DinaDeleteDate { get; set; }
        public bool? DinaJConsent { get; set; }
    }
}
