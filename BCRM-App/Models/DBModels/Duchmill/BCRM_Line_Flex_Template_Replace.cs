using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_Line_Flex_Template_Replace
    {
        public int ReplaceId { get; set; }
        public int TemplateId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int? Type { get; set; }
    }
}
