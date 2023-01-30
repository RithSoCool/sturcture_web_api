using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class FFM_Ticket
    {
        public long TicketId { get; set; }
        public int BrandId { get; set; }
        public int? StoreId { get; set; }
        public int? AppId { get; set; }
        public string FFM_Reference { get; set; }
        public int Status { get; set; }
        public int? IdentityId { get; set; }
        public int? CRM_CustomerId { get; set; }
        public int FFM_Type { get; set; }
        public string FFM_Type_Desc { get; set; }
        public long? FFM_Ref_Priv_TransactionId { get; set; }
        public string TrackingNo { get; set; }
        public int? ShippingId { get; set; }
        public string Shipping_Company { get; set; }
        public string Shipping_TrackingUrl { get; set; }
        public string FFM_Detail { get; set; }
        public string Title { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int Ref_Address_Type { get; set; }
        public long? Ref_AddressId { get; set; }
        public string Lang_Code { get; set; }
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
        public string Extra_Ref { get; set; }
        public string Extra_Ref_2 { get; set; }
        public DateTime Updated_DT { get; set; }
        public DateTime Created_DT { get; set; }
        public bool IsDeleted { get; set; }
    }
}
