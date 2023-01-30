using BCRM.Common.Models.DBModel.Common;
using BCRM.Common.Services.RemoteInternal.IAM;
using BCRM.Common.Services.RemoteInternal.IAM.Model;
using BCRM_App.Areas.Api.Models.Sms;
using BCRM_App.Areas.Api.Services.SMS.Models;
using BCRM_App.Services.RemoteInternal.SMS;
using BCRM_App.Services.RemoteInternal.SMS.Model;
using System;
using System.Threading.Tasks;

namespace BCRM_App.Areas.Api.Services.SMS
{
    public interface ISMS_Internal_Service
    {
        public string AppId { get; set; }
        public string BrandRef { get; set; }
        public Task<SMS_Verification_OTP_Request_Resp> SendSMS(SMSOTP_Send req);
        public Task<SMS_Verification_OTP_Resend_Resp> ResendSMS(SMSOTP_Resend req);
        public Task<OTP_Verify_Resp> VerifySMS(SMSOTP_Verify req);
        public Task<IAM_Token_Exchange_Resp> GenerateToken(dynamic req, dynamic tokenInfo);
    }

    public class SMS_Internal_Service_Base : ISMS_Internal_Service
    {

        private readonly ISMS_Client_Service sms_Service;
        internal readonly IIAM_Client_Service iamService;

        public SMS_Internal_Service_Base(ISMS_Client_Service sms_Service,
                                         IIAM_Client_Service iamService)
        {
            this.sms_Service = sms_Service;
            this.iamService = iamService;
            this.ExpireInSec = 60;
            this.OTPLength = 6;
        }

        public int ExpireInSec { get; set; }
        public int OTPLength { get; set; }
        public string AppId { get; set; }
        public string BrandRef { get; set; }
        public string SenderName { get; set; }

        public virtual async Task<SMS_Verification_OTP_Request_Resp> SendSMS(SMSOTP_Send req)
        {
            try
            {
                string reference = req.SendInfo.MobileNo + "-REF" + DateTime.Now.ToString("yyyyMMddHHmmss");
                SMS_Response response = await sms_Service.VerificationOTPRequestAsync(req.SendInfo.MobileNo, req.SendInfo.MobileNo + reference, SenderName, OTPLength, ExpireInSec, null, null);

                if (response.Success)
                {
                    var otp = (SMS_Verification_OTP_Request_Resp)response.ResponseSuccess;
                    return otp;
                }
                else
                {
                    string errorMessageTh = response.ResponseError.RawData["error"]["ErrorMessageTh"].ToString();
                    string errorMessageEn = response.ResponseError.RawData["error"]["ErrorMessageEn"].ToString();

                    throw new Exception(!string.IsNullOrEmpty(errorMessageTh) ? errorMessageTh : errorMessageEn);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<SMS_Verification_OTP_Resend_Resp> ResendSMS(SMSOTP_Resend req)
        {
            try
            {
                SMS_Response response = await sms_Service.VerificationOTPResendAsync(req.ResendInfo.TransactionId, req.ResendInfo.MobileNo, null, null);

                if (response.Success)
                {
                    var otp = (SMS_Verification_OTP_Resend_Resp)response.ResponseSuccess;
                    return otp;
                }
                else
                {
                    string errorMessageTh = response.ResponseError.RawData["error"]["ErrorMessageTh"].ToString();

                    string errorMessageEn = response.ResponseError.RawData["error"]["ErrorMessageEn"].ToString();

                    throw new Exception(!string.IsNullOrEmpty(errorMessageTh) ? errorMessageTh : errorMessageEn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<OTP_Verify_Resp> VerifySMS(SMSOTP_Verify req)
        {
            try
            {
                SMS_Response response = await sms_Service.VerificationOTPVerifyAsync(req.VerifyInfo.TransactionId, req.VerifyInfo.OTP, req.VerifyInfo.MobileNo);

                if (response.Success)
                {
                    var otp = (SMS_Verification_OTP_Verify_Resp)response.ResponseSuccess;


                    OTP_Verify_Resp verifyOtpResponse = new OTP_Verify_Resp()
                    {
                        VerifyResponse = otp
                    };

                    return verifyOtpResponse;
                }
                else
                {
                    string errorMessageTh = response.ResponseError.RawData["error"]["ErrorMessageTh"].ToString();

                    string errorMessageEn = response.ResponseError.RawData["error"]["ErrorMessageEn"].ToString();

                    throw new Exception(!string.IsNullOrEmpty(errorMessageTh) ? errorMessageTh : errorMessageEn);
                }
            }
            catch (Exception)
            {

                throw;
            }
            throw new System.NotImplementedException();
        }

        public virtual Task<IAM_Token_Exchange_Resp> GenerateToken(dynamic req, dynamic tokenInfo)
        {
            return null;
        }
    }
}
