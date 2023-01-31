using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class Banner_Banner
    {
        public int BannerId { get; set; }
        public int Seq { get; set; }
        public string Banner_Image_BCI_Ref { get; set; }
        public string Banner_Image_Image_Url { get; set; }
        public string Name { get; set; }
        public int? Call_To_Action { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime? Valid_From { get; set; }
        public DateTime? Valid_Through { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedUser { get; set; }
        public int? UpdatedUser { get; set; }
        public string Path { get; set; }
        public string Layout_Section { get; set; }
    }
}
