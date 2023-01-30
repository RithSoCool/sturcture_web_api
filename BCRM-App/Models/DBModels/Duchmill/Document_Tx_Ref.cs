using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class Document_Tx_Ref
    {
        public int RefId { get; set; }
        public int DocumentId { get; set; }
        public int? Tx_Id { get; set; }
        public string Tx_Referance { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? Point { get; set; }
        public int? Point_Left { get; set; }
        public decimal? Spending { get; set; }
        public string BillingNo { get; set; }
        public string RejectMessage { get; set; }
        public int? Stack { get; set; }
        public string DocumentRef_Stack { get; set; }
    }
}
