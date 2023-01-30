using BCRM_App.Services.RemoteInternal.SMS.Model;

namespace BCRM_App.Areas.Api.Services.SMS.Models
{
    public class OTP_Verify_Resp
    {
        public SMS_Verification_OTP_Verify_Resp VerifyResponse { get; set; }
        public string Access_Token { get; set; }
        public long? Access_Token_Exp_time { get; set; }
    }
}
