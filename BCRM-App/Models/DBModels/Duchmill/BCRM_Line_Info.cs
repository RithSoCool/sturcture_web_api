using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class BCRM_Line_Info
    {
        public int AccountId { get; set; }
        public string IAM_OAuth_TX_Ref { get; set; }
        public string Line_OAuth_State { get; set; }
        public string Line_UserId { get; set; }
        public string Name { get; set; }
        public string Picture_Url { get; set; }
        public string Access_Token { get; set; }
        public string Payload { get; set; }
        public string OAuth_Ref { get; set; }
        public int? CRM_CustomerId { get; set; }
        public string Identity_SRef { get; set; }
        public DateTime? Last_Line_Login_DT { get; set; }
        public DateTime? Last_SM_Login_DT { get; set; }
        public DateTime Updated_DT { get; set; }
        public DateTime Created_DT { get; set; }
        public bool? IsDeleted { get; set; }
        public string Remart { get; set; }
        public int? Previous_CRM_CustomerId { get; set; }
        public int? Transform_To_AccountId { get; set; }
    }
}
