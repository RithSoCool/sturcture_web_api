using BCRM.Common.Services;
using BCRM_App.Services.RemoteInternal.SMS;
using BCRM_App.Services.RemoteInternal.SMS.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCRM_App.Services.RemoteInternal.SMS
{
    public interface ISMS_Client_Service
    {
        Task<SMS_Response> SendAsync(String SenderName, String MobileNo, String Message, int? StoreId, String Reference, String Reference_2);
        Task<SMS_Response> TemplateSendAsync(String TemplateRef, String MobileNo, List<SMS_Template_Payload> Payload);
        Task<SMS_Response> VerificationOTPRequestAsync(String MobileNo, String RequestRef, String SenderName, int Length, int ExpireIn, String Reference, String Reference_2);
        Task<SMS_Response> VerificationOTPByCountryRequestAsync(String MobileNo, String RequestRef, String SenderName, int Length, int ExpireIn, String Reference, String Reference_2, int N_type, string CountryCode);
        Task<SMS_Response> VerificationOTPResendAsync(String TransactionId, String MobileNo, String Reference, String Reference_2);
        Task<SMS_Response> VerificationOTPVerifyAsync(String TransactionId, String OTP, String MobileNo);
    }

    public class SMS_Client_Service : BCRM_Client, ISMS_Client_Service
    {
        private readonly ILogger<SMS_Client_Service> _logger;

        public SMS_Client_Service(IHttpClientFactory clientFactory, ILogger<SMS_Client_Service> logger) : base(clientFactory)
        {
            _logger = logger;
        }

        public async Task<SMS_Response> SendAsync(String SenderName, String MobileNo, String Message, int? StoreId, String Reference, String Reference_2)
        {
            var reqParams = new
            {
                RequestRef = Guid.NewGuid(),
                SenderName = SenderName,
                MobileNo = MobileNo,
                Message = Message,
                StoreId = StoreId,
                Reference = Reference,
                Reference_2 = Reference_2
            };


            SMS_Response result = new SMS_Response();

            result.Success = await PostAsync(
                SMS_SC_Constant.Service.Api.Action.SMS_Send
                , SMS_SC_Constant.Service.Api.Path.v1.SMS_Send
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<SMS_Response> TemplateSendAsync(String TemplateRef, String MobileNo, List<SMS_Template_Payload> Payload)
        {
            var reqParams = new
            {
                RequestRef = Guid.NewGuid(),
                TemplateRef = TemplateRef,
                MobileNo = MobileNo,
                Payload = Payload
            };


            SMS_Response result = new SMS_Response();

            result.Success = await PostAsync(
                SMS_SC_Constant.Service.Api.Action.SMS_Template_Send
                , SMS_SC_Constant.Service.Api.Path.v1.SMS_Template_Send
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<SMS_Response> VerificationOTPRequestAsync(String MobileNo, String RequestRef, String SenderName, int Length, int ExpireIn, String Reference, String Reference_2)
        {
            var reqParams = new
            {
                MobileNo = MobileNo,
                RequestRef = RequestRef,
                Length = Length,
                ExpireIn = ExpireIn,
                SenderName = SenderName,
                Reference = Reference,
                Reference_2 = Reference_2
            };


            SMS_Response result = new SMS_Response();

            result.Success = await PostAsync(
                SMS_SC_Constant.Service.Api.Action.Verification_OTP_Request
                , SMS_SC_Constant.Service.Api.Path.v1.Verification_OTP_Request
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<SMS_Response> VerificationOTPByCountryRequestAsync(String MobileNo, String RequestRef, String SenderName, int Length, int ExpireIn, String Reference, String Reference_2, int N_type, string CountryCode)
        {
            var reqParams = new
            {
                MobileNo = MobileNo,
                RequestRef = RequestRef,
                N_type = N_type,
                CountryCode = CountryCode,
                Length = Length,
                ExpireIn = ExpireIn,
                SenderName = SenderName,
                Reference = Reference,
                Reference_2 = Reference_2
            };


            SMS_Response result = new SMS_Response();

            result.Success = await PostAsync(
                SMS_SC_Constant.Service.Api.Action.Verification_OTP_Request
                , SMS_SC_Constant.Service.Api.Path.v1.Verification_OTP_Request
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }
        public async Task<SMS_Response> VerificationOTPResendAsync(String TransactionId, String MobileNo, String Reference, String Reference_2)
        {
            var reqParams = new
            {
                TransactionId = TransactionId,
                MobileNo = MobileNo,
                Reference = Reference,
                Reference_2 = Reference_2
            };


            SMS_Response result = new SMS_Response();

            result.Success = await PostAsync(
                SMS_SC_Constant.Service.Api.Action.Verification_OTP_Resend
                , SMS_SC_Constant.Service.Api.Path.v1.Verification_OTP_Resend
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }

        public async Task<SMS_Response> VerificationOTPVerifyAsync(String TransactionId, String OTP, String MobileNo)
        {
            var reqParams = new
            {
                TransactionId = TransactionId,
                MobileNo = MobileNo,
                OTP = OTP
            };


            SMS_Response result = new SMS_Response();

            result.Success = await PostAsync(
                SMS_SC_Constant.Service.Api.Action.Verification_OTP_Verify
                , SMS_SC_Constant.Service.Api.Path.v1.Verification_OTP_Verify
                , reqParams
                , (resp) => { result.ResponseSuccess = resp; }
                , (respError) => { result.ResponseError = respError; });

            return result;
        }
    }
}
