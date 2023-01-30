using BCRM.Portable.Services.RemoteInternal.IAM.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM.Portable.Services.RemoteInternal.IAM
{
    public class IAM_ResponseConverter
    {
        public static IBCRM_Client_Resp Convert(dynamic respData, Enum action)
        {
            IBCRM_Client_Resp resp = null;

            JObject jObject_raw = respData;

            // Case response property Data = "", null จะเป็น JValue type
            // ให้ return null ได้เลยเดี๋ยว library จะไปแปลงเป็น object BCRM_Client_Resp 
            if (jObject_raw["data"] is JValue) 
            {
                return null;
            }

            JObject jObject_data = (JObject)jObject_raw["data"];

            switch (action)
            {
                case IAM_SC_Constant.Service.Api.Action.Account_Login:
                    resp = jObject_data.ToObject<IAM_Account_Login_Resp>();
                    break;

                case IAM_SC_Constant.Service.Api.Action.Account_Logout:
                    // เนื่องจากเส้น logout ไม่มีส่ง data มาจึงไม่ต้อง map to Response object เฉพาะของ Account_Logout
                   break;

                case IAM_SC_Constant.Service.Api.Action.Account_Create:
                    resp = jObject_data.ToObject<IAM_Account_Create_Resp>();
                    break;

                case IAM_SC_Constant.Service.Api.Action.Token_RefreshToken:
                    resp = jObject_data.ToObject<IAM_Token_RefreshToken_Resp>();
                    break;

                case IAM_SC_Constant.Service.Api.Action.Token_Exchange:
                    resp = jObject_data.ToObject<IAM_Token_Exchange_Resp>();
                    break;

                case IAM_SC_Constant.Service.Api.Action.Oauth_Request:
                case IAM_SC_Constant.Service.Api.Action.Oauth_RequestGoogle:
                case IAM_SC_Constant.Service.Api.Action.Oauth_RequestLine:
                    resp = jObject_data.ToObject<IAM_Oauth_Request_Resp>();
                    break;
            }

            // default case ให้ return null
            // เพื่อให้ code ที่ call response converter กลับไปใช้วิธี default ในการ convert

            return resp;
        }
    }
}
