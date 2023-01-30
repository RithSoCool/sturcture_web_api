using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class Document_Status_Log
    {
        public int LogId { get; set; }
        public int? CRM_CustomerId { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentRef { get; set; }
        public string BillingNo { get; set; }
        public int? Status { get; set; }
        public decimal? Spending { get; set; }
        public int? Point { get; set; }
        public bool? IsEarnPoint { get; set; }
        public bool? IsVoidPoint { get; set; }
        public string Remark { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? Stack { get; set; }
        public string DocumentRef_Stack { get; set; }
    }
}
