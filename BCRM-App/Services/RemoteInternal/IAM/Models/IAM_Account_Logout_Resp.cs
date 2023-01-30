using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM.Portable.Services.RemoteInternal.IAM.Model
{
    public class IAM_Account_Logout_Resp : IBCRM_Client_Resp
    {
        // Logout has not body, just read Status (Success/Fail/Error) only
    }
}
