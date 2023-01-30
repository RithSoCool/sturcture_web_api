using BCRM_App.Services.RemoteInternal.SMS.Model;
using Newtonsoft.Json.Linq;
using System;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM_App.Services.RemoteInternal.SMS
{
    public class SMS_ResponseConverter
    {
        public static IBCRM_Client_Resp Convert(dynamic respData, Enum action)
        {
            IBCRM_Client_Resp resp = null;

            JObject jObject_raw = respData;

            switch (action)
            {
                case SMS_SC_Constant.Service.Api.Action.SMS_Send:
                    resp = new SMS_Send_Resp(respData);
                    break;

                case SMS_SC_Constant.Service.Api.Action.SMS_Template_Send:
                    resp = new SMS_Template_Send_Resp(respData);
                    break;

                case SMS_SC_Constant.Service.Api.Action.Verification_OTP_Request:
                    resp = new SMS_Verification_OTP_Request_Resp(respData);
                    break;

                case SMS_SC_Constant.Service.Api.Action.Verification_OTP_Resend:
                    resp = new SMS_Verification_OTP_Resend_Resp(respData);
                    break;

                case SMS_SC_Constant.Service.Api.Action.Verification_OTP_Verify:
                    resp = new SMS_Verification_OTP_Verify_Resp(respData);
                    break;
            }

            // default case ให้ return null
            // เพื่อให้ code ที่ call response converter กลับไปใช้วิธี default ในการ convert

            return resp;
        }
    }
}
