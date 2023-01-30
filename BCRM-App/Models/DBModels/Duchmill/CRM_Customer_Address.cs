using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Customer_Address
    {
        public long AddressId { get; set; }
        public int CRM_CustomerId { get; set; }
        public int Seq { get; set; }
        public string Lang_Code { get; set; }
        public string Label { get; set; }
        public string Addr_Label { get; set; }
        public string Address { get; set; }
        public string HouseNo { get; set; }
        public string VillageNo { get; set; }
        public string Lane { get; set; }
        public string Building { get; set; }
        public string Street { get; set; }
        public long? SubDistrictId { get; set; }
        public string SubDistrict { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Addr_Remark { get; set; }
        public string ContactNo { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? Addr_Type { get; set; }
        public string Addr_Type_Desc { get; set; }
        public int? Status { get; set; }
        public string Title { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
    }
}
