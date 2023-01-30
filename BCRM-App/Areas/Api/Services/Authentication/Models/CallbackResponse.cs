using BCRM.Common.Services.RemoteInternal.IAM.Model;

namespace BCRM_App.Services.RemoteInternal.Authentication
{

    public class CallbackResponse
    {
        public IAM_Token_Exchange_Resp Token { get; set; }
        public string RedirectUrl { get; set; }
    }
}
