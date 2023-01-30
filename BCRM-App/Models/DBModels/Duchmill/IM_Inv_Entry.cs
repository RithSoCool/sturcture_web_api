using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class IM_Inv_Entry
    {
        public long EntryId { get; set; }
        public int InventoryId { get; set; }
        public int Type { get; set; }
        public string Type_Desc { get; set; }
        public int? ProductId { get; set; }
        public int? PrivilegeId { get; set; }
        public int Seq { get; set; }
        public decimal Total_Qty { get; set; }
        public decimal Withdraw_Qty { get; set; }
        public decimal Reserved_Qty { get; set; }
        public decimal Expired_Qty { get; set; }
        public decimal Remaining_Qty { get; set; }
        public decimal Minimum_Qty { get; set; }
        public int Stock_Mode { get; set; }
        public string Stock_Mode_Desc { get; set; }
        public int Fullfillment_Mode { get; set; }
        public string Fullfillment_Mode_Desc { get; set; }
        public bool IsManageSerial { get; set; }
        public int Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
