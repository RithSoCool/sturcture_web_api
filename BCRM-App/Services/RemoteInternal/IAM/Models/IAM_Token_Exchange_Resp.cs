using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM.Portable.Services.RemoteInternal.IAM.Model
{
    public class IAM_Token_Exchange_Resp : IBCRM_Client_Resp
    {
        public string Access_Token { get; set; }
        public long Access_Token_Exp_time { get; set; }
    }
}
