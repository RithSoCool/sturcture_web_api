using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Privilege_Category_Mapping
    {
        public int MappingId { get; set; }
        public int CategoryId { get; set; }
        public int PrivilegeId { get; set; }
        public int Category_Level { get; set; }
        public bool IsPrimary { get; set; }
        public int Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
