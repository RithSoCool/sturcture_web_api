using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class IM_Inventory
    {
        public int InventoryId { get; set; }
        public int? Seq { get; set; }
        public int Inv_Scope { get; set; }
        public int? StoreId { get; set; }
        public Guid Reference { get; set; }
        public string Inv_Reference { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Remark { get; set; }
        public int? Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
