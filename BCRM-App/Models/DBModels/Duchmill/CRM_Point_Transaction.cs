using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Point_Transaction
    {
        public long TransactionId { get; set; }
        public long? Ref_TransactionId { get; set; }
        public long? Ref_Void_TransactionId { get; set; }
        public Guid TXReference { get; set; }
        public string Ext_TransactionId { get; set; }
        public int WL_WalletId { get; set; }
        public long WL_LedgerId { get; set; }
        public Guid WL_TXReference { get; set; }
        public long? WL_RedeemId { get; set; }
        public int BrandId { get; set; }
        public int? StoreId { get; set; }
        public int IdentityId { get; set; }
        public int CustomerId { get; set; }
        public int TX_Type { get; set; }
        public string TX_Type_Desc { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public int Redeem_Ref { get; set; }
        public int? CRM_PrivilegeId { get; set; }
        public long? CRM_Priv_TransactionId { get; set; }
        public long? CRM_RedemptionId { get; set; }
        public int? CRM_PackageId { get; set; }
        public long? CRM_Pkg_PurchaseId { get; set; }
        public bool Spending_TX { get; set; }
        public decimal Spending { get; set; }
        public int Point_Calc { get; set; }
        public int Conversion_Mode { get; set; }
        public decimal Point_Conversion_Rate { get; set; }
        public int Point_Per_Conversion { get; set; }
        public int Pre_Balance { get; set; }
        public int Point { get; set; }
        public int Balance { get; set; }
        public decimal CV_Spending_Left { get; set; }
        public string Reference { get; set; }
        public long? POS_OrderId { get; set; }
        public string ITF_Ref { get; set; }
        public string ITF_Ref_2 { get; set; }
        public int? TX_Req_IdentityId { get; set; }
        public DateTime? Exp_Time { get; set; }
        public DateTime? Void_TX_Time { get; set; }
        public DateTime Updated_DT { get; set; }
        public DateTime TX_Time { get; set; }
    }
}
