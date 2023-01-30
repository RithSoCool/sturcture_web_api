using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class WL_Wallet_Ledger
    {
        public long LedgerId { get; set; }
        public long? Ref_LedgerId { get; set; }
        public long? Main_Void_LedgerId { get; set; }
        public long? RedeemId { get; set; }
        public int AssetId { get; set; }
        public int WalletId { get; set; }
        public int? WL_AppId { get; set; }
        public int? CRM_CustomerId { get; set; }
        public int? WL_IdentityId { get; set; }
        public int Main_TX { get; set; }
        public Guid TXReference { get; set; }
        public string Ext_TransactionId { get; set; }
        public int TX_Type { get; set; }
        public string TX_Type_Desc { get; set; }
        public int TX_Redeem_Mode { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public decimal Pre_Balance { get; set; }
        public decimal TX_Amount { get; set; }
        public decimal Remaining { get; set; }
        public decimal Balance { get; set; }
        public string Reference { get; set; }
        public string Extra_Ref { get; set; }
        public string Extra_Ref_2 { get; set; }
        public string TX_Exp_Data { get; set; }
        public DateTime? TX_Exp_DT { get; set; }
        public int? TX_Req_IdentityId { get; set; }
        public DateTime TX_Time { get; set; }
        public DateTime Updated_DT { get; set; }
    }
}
