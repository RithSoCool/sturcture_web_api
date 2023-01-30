using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Config
    {
        public int ConfigId { get; set; }
        public int? Point_AssetId { get; set; }
        public string Point_Asset_ARef { get; set; }
        public int? Point_SchemeId { get; set; }
        public int? Point_Scheme_Type { get; set; }
        public int Point_Conversion_Mode { get; set; }
        public decimal Point_Conversion_Rate { get; set; }
        public int Point_Per_Conversion { get; set; }
        public int Point_Per_TX { get; set; }
    }
}
