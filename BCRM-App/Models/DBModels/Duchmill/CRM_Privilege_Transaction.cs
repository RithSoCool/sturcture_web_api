using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Privilege_Transaction
    {
        public long TransactionId { get; set; }
        public long? Ref_TransactionId { get; set; }
        public long? Ref_Void_TransactionId { get; set; }
        public Guid TXReference { get; set; }
        public string Ext_TransactionId { get; set; }
        public int BrandId { get; set; }
        public int? StoreId { get; set; }
        public int P_Type { get; set; }
        public int PrivilegeId { get; set; }
        public long? Issue_LedgerId { get; set; }
        public long? Redeem_LedgerId { get; set; }
        public long? Void_LedgerId { get; set; }
        public long? PC_ReserveId { get; set; }
        public int CRM_CustomerId { get; set; }
        public int IdentityId { get; set; }
        public string Identity_SRef { get; set; }
        public int TX_Type { get; set; }
        public string TX_Type_Desc { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public int Amount { get; set; }
        public decimal Point_PerAmount { get; set; }
        public decimal Point_Total { get; set; }
        public long? CRM_Point_Redeem_TransactionId { get; set; }
        public long? WL_RedeemId { get; set; }
        public long? WL_Redeem_LedgerId { get; set; }
        public long? FFM_TicketId { get; set; }
        public long? TP_Topup_CodeId { get; set; }
        public Guid? TP_Topup_TXRef { get; set; }
        public string Remark { get; set; }
        public string Void_Remark { get; set; }
        public string ITF_Ref { get; set; }
        public string ITF_Ref_2 { get; set; }
        public int? Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public DateTime TX_Time { get; set; }
        public string TX_Ref { get; set; }
        public DateTime? Void_Time { get; set; }
        public string Void_Ref { get; set; }
        public DateTime Updated_DT { get; set; }
    }
}
