using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class Document_DN_Form_Value
    {
        public int ValueId { get; set; }
        public int ContainerId { get; set; }
        public long DocumentId { get; set; }
        public int FieldId { get; set; }
        public int FormId { get; set; }
        public int Seq { get; set; }
        public int DataType { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
