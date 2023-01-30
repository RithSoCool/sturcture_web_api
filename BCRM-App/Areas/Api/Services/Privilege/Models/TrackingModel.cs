using BCRM_App.Constants;
using BCRM_App.Extentions;
using BCRM_App.Models.DBModels.Duchmill;
using System;
using System.Globalization;

public class TrackingModel
{
    public class Privilege_Transaction_History_Resp // : Privilege_Transaction_History
    {
        public Privilege_Transaction_History_Resp()
        {
            Privilege = new Privilege_Payload_Resp();
            TrackingStatus = new FFM_Ticket_Resp();
        }

        public Privilege_Transaction_History_Resp(BCRM_App.Models.DBModels.Duchmill.CRM_Privilege_Transaction transaction, FFM_Ticket tracking)
        {
            //Privilege = new Privilege_Payload_Resp();
            TrackingStatus = new FFM_Ticket_Resp();

            //TransactionId = transaction.TransactionId;
            //Ref_TransactionId = transaction.Ref_TransactionId;
            //Ref_Void_TransactionId = transaction.Ref_Void_TransactionId;
            //TXReference = transaction.Ext_TransactionId;
            Ext_TransactionId = transaction.Ext_TransactionId;
            //P_Type = transaction.P_Type;
            //CRM_CustomerId = transaction.CRM_CustomerId;
            //IdentityId = transaction.IdentityId;
            //Identity_SRef = transaction.Identity_SRef;
            //TX_Type = transaction.TX_Type;
            //TX_Type_Desc = transaction.TX_Type_Desc;
            Status = transaction.Status;
            Status_Desc = transaction.Status_Desc;
            Amount = transaction.Amount;
            Point_PerAmount = transaction.Point_PerAmount;
            Point_Total = transaction.Point_Total;
            //Req_IdentityId = transaction.Req_IdentityId;
            //Req_Identity_SRef = transaction.Req_Identity_SRef;
            //Updated_DT = transaction.Updated_DT.ToString();
            FFM_TicketId = transaction.FFM_TicketId;

            TrackingStatus = new FFM_Ticket_Resp(tracking);
        }

        //public long TransactionId { get; set; }
        //public long? Ref_TransactionId { get; set; }
        //public long? Ref_Void_TransactionId { get; set; }
        //public string TXReference { get; set; }
        public string Ext_TransactionId { get; set; }
        //public int? StoreId { get; set; }
        //public int P_Type { get; set; }
        //public int PrivilegeId { get; set; }

        //public long? Issue_LedgerId { get; set; }
        //public long? Redeem_LedgerId { get; set; }
        //public long? Void_LedgerId { get; set; }

        //public long? PC_ReserveId { get; set; }
        //public int CRM_CustomerId { get; set; }
        //public int IdentityId { get; set; }
        //public string Identity_SRef { get; set; }
        //public int TX_Type { get; set; }
        //public string TX_Type_Desc { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public int Amount { get; set; }
        public decimal Point_PerAmount { get; set; }
        public decimal Point_Total { get; set; }

        //public string Remark { get; set; }
        //public string Void_Remark { get; set; }
        //public int? Req_IdentityId { get; set; }
        //public string Req_Identity_SRef { get; set; }
        //public string ITF_Ref { get; set; }
        //public string ITF_Ref_2 { get; set; }

        public string TX_Time { get; set; }
        public string TX_Time_Desc
        {
            get
            {
                try
                {
                    var date = Convert.ToDateTime(TX_Time);
                    return date.ToDisplayDateTime_Th();
                }
                catch
                {
                    return TX_Time;
                }
            }
        }
        //public string TX_Ref { get; set; }
        //public string? Void_Time { get; set; }
        //public string Void_Ref { get; set; }
        //public string Updated_DT { get; set; }

        //// exp time
        //public string Exp_Time { get; set; }

        // FFM Module
        public long? FFM_TicketId { get; set; }

        // extra detail
        //public CRM_Privilege_Limit Privilege_Limit { get; set; }
        //public CRM_Privilege_Code CRM_Privilege_Code { get; set; }
        public Privilege_Payload_Resp Privilege { get; set; }
        //public Privilege_Serial_Payload Privilege_Serial { get; set; }

        public FFM_Ticket_Resp TrackingStatus { get; set; }

        public string CraeteTime_DT_Label { get; set; }
    }

    public class Privilege_Payload_Resp // : Privilege_Payload
    {
        //public int Privilege_Id { get; set; }
        //public string Reference { get; set; }
        public string Alt_Reference { get; set; }

        //public int Progress_Status { get; set; }
        //public string Progress_Status_Desc { get; set; }

        public int Status { get; set; }
        public string Status_Desc { get; set; }

        //public int Seq { get; set; }

        public int Type { get; set; }
        public string Type_Desc { get; set; }

        //public int CPC_Type { get; set; }
        //public int CPC_Qty_Per_Issue { get; set; }

        public string Name_Th { get; set; }
        public string Name_En { get; set; }
        //public decimal Value_Ref { get; set; }
        //public int Reserve_Timeout { get; set; }

        // TODO: remove
        //public int Code_Qty_Per_Redeem { get; set; }
        public string Privilege_Image_Url { get; set; }
        //public string Privilege_BCI_Ref { get; set; }
        //public string UpdatedTime { get; set; }
    }

    public class FFM_Ticket_Resp
    {
        public FFM_Ticket_Resp()
        {
            var cultureInfo = new CultureInfo("th-TH");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        }

        public FFM_Ticket_Resp(FFM_Ticket tracking)
        {
            this.TicketId = tracking.TicketId;
            //this.FFM_Reference = tracking.FFM_Reference;
            //this.CRM_CustomerId = tracking.CRM_CustomerId;
            //this.FFM_Type = tracking.FFM_Type;
            //this.FFM_Type_Desc = tracking.FFM_Type_Desc;
            //this.FFM_Ref_Priv_TransactionId = tracking.FFM_Ref_Priv_TransactionId;
            //this.FFM_Detail = tracking.FFM_Detail;
            //this.Title = tracking.Title;
            this.First_Name = tracking.First_Name;
            this.Last_Name = tracking.Last_Name;
            this.ContactNo = tracking.ContactNo;
            this.Address = tracking.Address;
            this.SubDistrictId = tracking.SubDistrictId;
            this.SubDistrict = tracking.SubDistrict;
            this.DistrictId = tracking.DistrictId;
            this.District = tracking.District;
            this.ProvinceId = tracking.ProvinceId;
            this.Province = tracking.Province;
            this.CountryId = tracking.CountryId;
            this.Country = tracking.Country;
            this.PostalCode = tracking.PostalCode;
            this.Status = tracking.Status;
            this.TrackingNo = tracking.TrackingNo;
            this.ShippingId = tracking.ShippingId;
            this.Shipping_Company = tracking.Shipping_Company;
            this.Shipping_TrackingUrl = tracking.Shipping_TrackingUrl;
            this.Updated_DT = tracking.Updated_DT;
            this.Created_DT = tracking.Created_DT;

            var cultureInfo = new CultureInfo("th-TH");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        }

        public long TicketId { get; set; }
        //public int BrandId { get; set; }
        //public int? StoreId { get; set; }
        //public int? AppId { get; set; }

        //public int? CRM_CustomerId { get; set; }
        //public string FFM_Reference { get; set; }
        //public int FFM_Type { get; set; }
        //public string FFM_Type_Desc { get; set; }
        //public long? FFM_Ref_Priv_TransactionId { get; set; }
        //public int? IdentityId { get; set; }
        //public string FFM_Detail { get; set; }

        //public string Title { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        //public int Ref_Address_Type { get; set; }
        //public long? Ref_AddressId { get; set; }
        public string ContactNo { get; set; }
        //public string Lang_Code { get; set; }
        public string Address { get; set; }
        //public string HouseNo { get; set; }
        //public string VillageNo { get; set; }
        //public string Lane { get; set; }
        //public string Building { get; set; }
        //public string Street { get; set; }
        public long? SubDistrictId { get; set; }
        public string SubDistrict { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        //public string Addr_Remark { get; set; }
        //public string Extra_Ref { get; set; }
        //public string Extra_Ref_2 { get; set; }
        public int Status { get; set; }
        public string Status_Label
        {
            get
            {
                return AppConstants.Privilege.Fulfillment.Status.Get_Desc(Status);
            }
        }
        public string TrackingNo { get; set; }
        public int? ShippingId { get; set; }
        public string Shipping_Company { get; set; }
        public string Shipping_TrackingUrl { get; set; }

        public DateTime Updated_DT { get; set; }
        public string Updated_DT_Label
        {
            get
            {
                return Updated_DT.ToDisplayDateTime_Th();
            }
        }
        public DateTime Created_DT { get; set; }
        public string Created_DT_Label
        {
            get
            {
                return Created_DT.ToDisplayDateTime_Th();
            }
        }
        //public bool IsDeleted { get; set; }


    }
}
