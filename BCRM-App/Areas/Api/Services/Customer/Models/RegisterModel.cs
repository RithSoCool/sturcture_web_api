using BCRM.Common.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace BCRM_App.Areas.Api.Services.Customer
{
    public class RegisterModel
    {
        public RegisterModel()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
        }

        [Required]
        public string First_Name_Th { get; set; }
        [Required]
        public string Last_Name_Th { get; set; }
        [Required]
        public int? Gender { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }

        public string MobileNo { get; set; }

        [Required]
        public string Email { get; set; }

        public string AddressTitle { get; set; }
        
        public string Address { get; set; }

        public long? SubDistrictId { get; set; }

        public string SubDistrict { get; set; }
        public int? DistrictId { get; set; }

        public string District { get; set; }

        public int? ProvinceId { get; set; }
        [Required]
        public string Province { get; set; }

        public string PostalCode { get; set; }

        public string Line_UserId { get; set; }

        public bool? Accept_Privacy_Policy { get; set; }
        public bool? Accept_Activity_Consent { get; set; }
    }

    public class AddressModel
    {
        public AddressModel()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
        }

        [Required]
        public long AddressId { get; set; }
        [Required]
        public string First_Name_Th { get; set; }
        [Required]
        public string Last_Name_Th { get; set; }

        public int? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string MobileNo { get; set; }

        public string Email { get; set; }

        public string AddressTitle { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public long? SubDistrictId { get; set; }
        [Required]
        public string SubDistrict { get; set; }
        [Required]
        public int? DistrictId { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public int? ProvinceId { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string PostalCode { get; set; }

        public int? CRM_CustomerId { get; set; }
        public string Identity_SRef { get; set; }

        public int? AddressType { get; set; }

        [Required]
        public bool? IsDefault { get; set; }
    }

    public class AddAddressModel
    {
        public long? AddressId { get; set; }
        [Required]
        public string First_Name_Th { get; set; }
        [Required]
        public string Last_Name_Th { get; set; }
        //[Required]
        public int? Gender { get; set; }
        //[Required]
        public DateTime? DateOfBirth { get; set; }

        public string MobileNo { get; set; }

        public string Email { get; set; }

        [Required]
        public string AddressTitle { get; set; }

        public string Address { get; set; }

        [Required]
        public long? SubDistrictId { get; set; }
        [Required]
        public string SubDistrict { get; set; }
        [Required]
        public int? DistrictId { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public int? ProvinceId { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string PostalCode { get; set; }

        public int? CRM_CustomerId { get; set; }
        public string Identity_SRef { get; set; }

        public int? AddressType { get; set; }

        public bool IsDefault { get; set; }
    }

    public class EditProfileReq
    {
        public EditProfileReq()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
        }

        public string First_Name_Th { get; set; }
        public string Last_Name_Th { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        //public string MobileNo { get; set; }
        public string Email { get; set; }
        public string AddressTitle { get; set; }
        public string Address { get; set; }
        public long? SubDistrictId { get; set; }
        public string SubDistrict { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public IFormFile? ImageProfile { get; set; }
    }
}
