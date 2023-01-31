using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class Banner_Mapping_Set_Banner
    {
        public int MappingId { get; set; }
        public int Seq { get; set; }
        public int Set_BannerId { get; set; }
        public int BannerId { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
