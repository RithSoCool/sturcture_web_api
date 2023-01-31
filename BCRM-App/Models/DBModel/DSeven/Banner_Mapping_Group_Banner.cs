using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class Banner_Mapping_Group_Banner
    {
        public int MappingId { get; set; }
        public int Seq { get; set; }
        public int Type { get; set; }
        public string Type_Desc { get; set; }
        public int Group_BannerId { get; set; }
        public int? Set_BannerId { get; set; }
        public int? BannerId { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
