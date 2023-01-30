using BCRM.Common.Services.CRM.Model;
using System.Collections.Generic;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class Register_Resp
    {
        public Register_Resp()
        {
            BrandActive = new List<string>();
        }

        public DuchmillModel.BCRM_Line_Info LineInfo { get; set; }
        public CRM_Customer_Data CRM_CustomerInfo { get; set; }
        public List<string> BrandActive { get; set; }
        public List<EarnWelcomePoint_Resp> WelcomePoints { get; set; }

        public bool IsEmployee { get; set; }
        public int? EmployeeId { get; set; }
    }
}
