using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class Banner_Set_Banner
    {
        public int Set_BannerId { get; set; }
        public int Seq { get; set; }
        public string Set_Name { get; set; }
        public string Description { get; set; }
        public string Layout_Section { get; set; }
        public DateTime Valid_From { get; set; }
        public DateTime Valid_Through { get; set; }
        public bool Carousel { get; set; }
        public int? Layout_Type { get; set; }
        public string Layout_Type_Desc { get; set; }
        public bool Status { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
