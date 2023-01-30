
using BCRM.Common.Models.DBModel.Wallet;
using BCRM.Common.Services.CRM;
using BCRM.Common.Services.Wallet;

namespace BCRM_App.Areas.Api.Services.Document.Models
{
    public class Callback_Resp
    {
        public CRM_Point_TX_Result Transaction { get; set; }
    }
}
