using BCRM.Common.Api.Response;

namespace BCRM_App.Services.RemoteInternal.Authentication
{
    public class LoginResponse
    {
        public BCRM_Api_Response Res { get; set; }
        public string RedirectUrl { get; set; }
        public string LineStage { get; set; }
    }
}
