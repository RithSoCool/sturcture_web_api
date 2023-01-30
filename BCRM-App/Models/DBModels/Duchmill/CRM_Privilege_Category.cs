using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Privilege_Category
    {
        public int CategoryId { get; set; }
        public Guid Reference { get; set; }
        public string Alt_Reference { get; set; }
        public int Seq { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public int Category_Level { get; set; }
        public int? Parent_CategoryId { get; set; }
        public string Name_Th { get; set; }
        public string Name_En { get; set; }
        public string Desc_Th { get; set; }
        public string Desc_En { get; set; }
        public string Remark { get; set; }
        public string Category_Image_Url { get; set; }
        public string Category_BCI_Ref { get; set; }
        public string Category_Sub_Image_Url { get; set; }
        public string Category_Sub_Image_BCI_Ref { get; set; }
        public int Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
