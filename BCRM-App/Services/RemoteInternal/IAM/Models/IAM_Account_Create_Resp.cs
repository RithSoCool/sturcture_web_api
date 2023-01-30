using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM.Portable.Services.RemoteInternal.IAM.Model
{
    public class IAM_Account_Create_Resp : IBCRM_Client_Resp
    {
        public string Provider_ref { get; set; }
        public int Account_Id { get; set; }
        public int Identity_Id { get; set; }
        public string Identity_Ref { get; set; }
        public string Identity_SRef { get; set; }
    }
}
