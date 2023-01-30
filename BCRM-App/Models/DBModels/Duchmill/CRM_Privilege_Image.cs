using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class CRM_Privilege_Image
    {
        public int ImageId { get; set; }
        public int Group_ImageId { get; set; }
        public int Seq { get; set; }
        public string Image_Url { get; set; }
        public string BCI_Ref { get; set; }
        public int Req_IdentityId { get; set; }
        public string Req_Identity_SRef { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
