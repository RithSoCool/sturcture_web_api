using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class WL_Wallet
    {
        public int WalletId { get; set; }
        public int AssetId { get; set; }
        public Guid Reference { get; set; }
        public string Alt_Reference { get; set; }
        public int WL_Scope { get; set; }
        public string WL_Scope_Desc { get; set; }
        public int WL_BrandId { get; set; }
        public int? WL_AppId { get; set; }
        public string WL_App_Id { get; set; }
        public int WL_Type { get; set; }
        public int? CRM_CustomerId { get; set; }
        public string CRM_Customer_Ref { get; set; }
        public int? WL_IdentityId { get; set; }
        public string WL_Identity_SRef { get; set; }
        public decimal Issue { get; set; }
        public decimal Redeem { get; set; }
        public decimal Expire { get; set; }
        public decimal Balance { get; set; }
        public string Extra_Ref { get; set; }
        public string Extra_Ref_2 { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
