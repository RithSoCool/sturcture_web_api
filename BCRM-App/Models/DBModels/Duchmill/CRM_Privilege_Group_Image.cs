using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Privilege_Group_Image
    {
        public int Group_ImageId { get; set; }
        public int Seq { get; set; }
        public int PrivilegeId { get; set; }
        public int Type { get; set; }
        public string Type_Desc { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Desc_Th { get; set; }
        public string Desc_En { get; set; }
        public string Remark { get; set; }
        public int Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
