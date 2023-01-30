
using BCRM_App.Models.DBModels.Duchmill;
using System;
using System.Collections.Generic;

namespace BCRM_App.Areas.Api.Services.Privilege
{
    public class GetCategories_Resp
    {
        public List<CRM_Privilege_Category> Categories { get; set; }
    }
}
