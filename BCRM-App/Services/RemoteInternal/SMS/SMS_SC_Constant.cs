using System;

namespace BCRM_App.Services.RemoteInternal.SMS
{
    public class SMS_SC_Constant
    {
        public const string Http_Client_Name = "crm-api-service-sms";

        public class Service
        {
            public class Sender
            {
                public const String Default = "ChocoCRM";
                //public const String Lolane = "Lolane";
            }

            public class Endpoint
            {

                public class Development
                {
                    public const string Url = "https://dev-sms.chococrm.com";
                }

                public class Production
                {
                    public const string Url = "https://sms.chococrm.com";
                }
            }

            public class Api
            {
                public class Path
                {
                    public class v1
                    {
                        public const String SMS_Send = "Api/SMS/Send";
                        public const String SMS_Template_Send = "Api/SMS/Template/Send";
                        public const String Verification_OTP_Request = "Api/Verification/OTP/Request";
                        public const String Verification_OTP_Resend = "Api/Verification/OTP/Resend";
                        public const String Verification_OTP_Verify = "Api/Verification/OTP/Verify";
                    }
                }

                public enum Action
                {
                    SMS_Send,
                    SMS_Template_Send,
                    Verification_OTP_Request,
                    Verification_OTP_Resend,
                    Verification_OTP_Verify
                }
            }
        }
    }
}
