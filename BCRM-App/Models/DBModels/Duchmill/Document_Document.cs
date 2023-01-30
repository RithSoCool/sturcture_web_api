using System;
using System.Collections.Generic;

#nullable disable

namespace BCRM_App.Models.DBModels.Duchmill
{
    public partial class Document_Document
    {
        public long DocumentId { get; set; }
        public Guid DocumentRef { get; set; }
        public int DocumentType { get; set; }
        public int BrandId { get; set; }
        public int? StoreId { get; set; }
        public int ContainerId { get; set; }
        public int Status { get; set; }
        public string PointOfPurchase { get; set; }
        public string Identity_SRef { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public int Attachments { get; set; }
        public string Remark { get; set; }
        public string Remark_2 { get; set; }
        public string ITF_Ref { get; set; }
        public string ITF_Ref_2 { get; set; }
        public string ITF_ExtraData { get; set; }
        public string Approve_By { get; set; }
        public string Int_Channel { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
