using System;
using System.Collections.Generic;
using System.Threading;

namespace BCRM_App.Areas.Api.Services.Document.Models
{
    public class Callback_Extra_Payload
    {
        public Dictionary<string, dynamic> Params { get; set; }
    }

    public class Callback_Payload
    {
        public Callback_Payload()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
        }

        public string Spending { get; set; }
        public int DocumentId { get; set; }
        public string DocumentRef { get; set; }
        public int DocumentType { get; set; }
        public string Document_Type_Desc { get; set; }
        public int BrandId { get; set; }
        public object StoreId { get; set; }
        public int ContainerId { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public string PointOfPurchase { get; set; }
        public string Identity_SRef { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public int Attachments { get; set; }
        public string Remark { get; set; }
        public string Remark_2 { get; set; }
        public string ITF_Ref { get; set; }
        public string ITF_Ref_2 { get; set; }
        public string ITF_ExtraData { get; set; }
        public string Approve_By { get; set; }
        public string UpdatedTime { get; set; }
        public string CreatedTime { get; set; }
        public int Status_From { get; set; }
        public int Status_To { get; set; }
        public string BillingNo { get; set; }
        public string RejectMessage { get; set; }



    }
}
