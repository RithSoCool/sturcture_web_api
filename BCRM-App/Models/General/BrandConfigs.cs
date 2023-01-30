using System.Collections.Generic;

namespace BCRM_App.Models.General
{
    public class BrandConfigs
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Brand_Ref { get; set; }
        public string Provider_Ref { get; set; }
        public string App_Id { get; set; }
        public string App_Secret { get; set; }
        public string App_Token { get; set; }
        public string App_Identity_Ref { get; set; }
        public string App_Identity_SRef { get; set; }
        public string Backend_Endpoint { get; set; }
        public string Frontend_Endpoint { get; set; }
        public int AssetId { get; set; }
        public string Asset_Alt_Ref { get; set; }
        public string Privilege_Inventory_Ref { get; set; }
        public int Point_Rate { get; set; }
        public int Welcome_Point { get; set; }
        public string Line_Flex_Message_Token { get; set; }
        public string Document_Alt_Reference { get; set; }
        public List<TierConfig> TierConfigs { get; set; }
    }

    public class TierConfig
    {
        public string Brand { get; set; }
        public string TierName { get; set; }
        public int TierValue { get; set; }
        public decimal? Up_Tier_Spending { get; set; }
        public string Up_Tier_Name { get; set; }
    }
}
