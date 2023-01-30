using System;
using System.Threading;

namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class Point_History_Resp
    {
        public int? Point { get; set; }
        public string BillingNo { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedTime_Desc
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");

                return CreatedTime.ToString("dd/MM/yyyy HH:mm");
            }
        }
        public int Status { get; set; }
        public string Status_Desc
        {
            get
            {
                switch (Status)
                {
                    case BCRM.Document.Constants.BCRM_Document_Const.Document.Status.Pending:
                        return "รออนุมัติ";
                    case BCRM.Document.Constants.BCRM_Document_Const.Document.Status.Completed:
                        return "สำเร็จ";
                    case BCRM.Document.Constants.BCRM_Document_Const.Document.Status.Rejected:
                        return "ไม่สำเร็จ";
                    default:
                        return "";
                }
            }
        }

        public string Store { get; set; }
        public string RejectMessage { get; set; }
    }
}
