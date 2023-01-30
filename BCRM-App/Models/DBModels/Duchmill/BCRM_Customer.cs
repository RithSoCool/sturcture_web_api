using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_Customer
    {
        public int Id { get; set; }
        public int CRM_CustomerId { get; set; }
        public int? Wallet_Id { get; set; }
        public string Wallet_Alt_Ref { get; set; }
        public decimal? Total_Spending { get; set; }
        public decimal? Point_Balance { get; set; }
        public int? LineInfoId { get; set; }
        public string Line_UserId { get; set; }
        public string ImageProfileUrl { get; set; }
        public DateTime? ImageProfileUrl_Expire { get; set; }
        public DateTime? Update_DT { get; set; }
        public bool? Accept_Privacy_Policy { get; set; }
        public bool? Accept_Activity_Consent { get; set; }
    }
}
