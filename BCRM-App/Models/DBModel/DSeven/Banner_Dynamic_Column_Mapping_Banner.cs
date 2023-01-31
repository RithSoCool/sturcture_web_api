using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModel.DSeven
{
    public partial class Banner_Dynamic_Column_Mapping_Banner
    {
        public int MappingId { get; set; }
        public int Seq { get; set; }
        public string Column_Name { get; set; }
        public int Dyn_TableId { get; set; }
        public int Dyn_ColumnId { get; set; }
        public string Dyn_Sys_Column { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
