using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_Line_Flex_Template
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string Titile { get; set; }
        public string JsonTemplate { get; set; }
        public DateTime? Valid_From { get; set; }
        public DateTime? Valid_Though { get; set; }
        public int Status { get; set; }
        public string SP_Mapped { get; set; }
    }
}
