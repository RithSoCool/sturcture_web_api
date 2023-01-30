using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCRM.Portable.Services.RemoteInternal.IAM
{
    public class IAM_SC_Constant
    {
        public const string Http_Client_Name = "bcrm-api-service-iam";

        public class Service
        {
            public class Endpoint
            {
                /* ============================================
                 * สำหรับ Endpoint URL ของ IAM จะไม่ได้ config ที่นี่
                 * แต่จะใช้ url ที่ declare ไว้ที่ lib BCRM-Common
                 * code ด้านล่างนี้ วางโครงไว้ให้สำหรับ กรณีที่ต้องเขียน remote service (http service) อื่นๆ
                 * จะได้ยึดโครงตามนี้ได้
                 * ============================================*/
                public class Development
                {
                    public const string Url = "";
                }

                public class Production
                {
                    public const string Url = "";
                }
            }

            public class Api
            {
                public class Path
                {
                    public class v1
                    {
                        public const string Oauth_Request = "iam/api/v1/oauth/request";
                        public const string Oauth_RequestGoogle = "iam/api/v1/oauth/request";
                        public const string Oauth_RequestLine = "iam/api/v1/oauth/request";

                        public const string Account_Login = "iam/api/v1/account/login";
                        public const string Account_Logout = "iam/api/v1/account/logout";
                        public const string Account_Create = "iam/api/v1/account/create";

                        public const string Token_RefreshToken = "iam/api/v1/Token/RefreshToken";
                        public const string Token_Exchange = "iam/api/v1/Token/Exchange";
                    }
                }

                public enum Action
                {
                    Oauth_Request,
                    Oauth_RequestGoogle,
                    Oauth_RequestLine,

                    Account_Login,
                    Account_Logout,
                    Account_Create,

                    Token_RefreshToken,
                    Token_Exchange,
                }
            }
        }
    }
}
