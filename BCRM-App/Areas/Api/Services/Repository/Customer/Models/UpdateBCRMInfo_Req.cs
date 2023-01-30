using System;

namespace BCRM_App.Areas.Api.Services.Repository.Customer.Models
{
    public class UpdateBCRMInfo_Req
    {
        public int AccountId { get; set; }
        public int? CRM_CustomerId { get; set; }
        public string Line_UserId { get; set; }
        public string Name { get; set; }
        public string Picture_Url { get; set; }
        public string Identity_SRef { get; set; }
        public string CRM_Wallet_Alt_Ref { get; set; }
        public bool? Accept_Privacy_Policy { get; set; }
        public bool? Accept_Activity_Consent { get; set; }
    }
}
