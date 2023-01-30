using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage
{
    public class Line_SC_Constant
    {
        public const string Http_Client_Name = "bcrm-api-service-Line-v2";

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
                        public const String Reply = "v2/bot/message/reply";
                        public const String Push = "v2/bot/message/push";
                    }
                }

                public enum Action
                {
                    Push,
                    Reply
                }
            }
        }
    }
}
