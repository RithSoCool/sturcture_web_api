using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class Banner_Group_Banner
    {
        public int Group_BannerId { get; set; }
        public int Seq { get; set; }
        public string Group_Name { get; set; }
        public string Description { get; set; }
        public DateTime? Valid_From { get; set; }
        public DateTime? Valid_Through { get; set; }
        public bool Status { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsDefault { get; set; }
    }
}
