using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class CRM_Customer
    {
        public int CustomerId { get; set; }
        public int IdentityId { get; set; }
        public string Identity_SRef { get; set; }
        public string Reference { get; set; }
        public string Customer_Ref { get; set; }
        public int Gov_Id_Type { get; set; }
        public string Gov_Id_Ref { get; set; }
        public string Title_Th { get; set; }
        public string First_Name_Th { get; set; }
        public string Middle_Name_Th { get; set; }
        public string Last_Name_Th { get; set; }
        public string Title_En { get; set; }
        public string First_Name_En { get; set; }
        public string Middle_Name_En { get; set; }
        public string Last_Name_En { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? MBS_TierId { get; set; }
        public string MBS_Tier_Th { get; set; }
        public string MBS_Tier_En { get; set; }
        public DateTime? MBS_Tier_Exp { get; set; }
        public string CRM_Wallet_Alt_Ref { get; set; }
        public int Point_TX { get; set; }
        public int Point_Earned { get; set; }
        public int Point_Redeemed { get; set; }
        public int Point_Expired { get; set; }
        public int Point_Balance { get; set; }
        public int Spending_TX { get; set; }
        public decimal Spending { get; set; }
        public decimal Avg_Spending { get; set; }
        public int POS_Order_TX { get; set; }
        public int Registered_Channel_Flag { get; set; }
        public string Registered_Channel { get; set; }
        public int? Registered_StoreId { get; set; }
        public DateTime? Membership_Since { get; set; }
        public DateTime Created_DT { get; set; }
        public DateTime Updated_DT { get; set; }
        public bool IsDeleted { get; set; }
    }
}
