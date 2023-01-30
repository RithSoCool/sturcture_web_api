using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Models.Sms
{
    public class SMSOTP_Send
    {
        public SMSOTP_Send_Req SendInfo { get; set; }
    }

    public class SMSOTP_Resend
    {
        public SMSOTP_Resend_Req ResendInfo { get; set; }
    }

    public class SMSOTP_Verify
    {
        public string AccessToken { get; set; }
        public SMSOTP_Verify_Req VerifyInfo { get; set; }
        public TokenPayloadInfo TokenPayload { get; set; }

    }

    public class SMSOTP_Send_Req
    {
        [Required]
        [RegularExpression(@"^0[0-9]{9}$")]
        public string MobileNo { get; set; }
        [Required]
        public string State { get; set; }
    }
    public class SMSOTP_Resend_Req
    {
        [Required]
        [RegularExpression(@"^0[0-9]{9}$")]
        public string MobileNo { get; set; }
        [Required]
        public string TransactionId { get; set; }
    }
    public class SMSOTP_Verify_Req
    {
        [RegularExpression(@"^0[0-9]{9}$")]
        public string MobileNo { get; set; }
        [Required]
        public string OTP { get; set; }
        [Required]
        public string TransactionId { get; set; }
    }

    public class TokenPayloadInfo
    {
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Brand_Ref { get; set; }
        public string AccessToken { get; set; } // for test
    }
}
