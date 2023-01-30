using BCRM.Portable.Services.RemoteInternal.IAM.Model;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage
{
    public class Line_ResponseConverter
    {
        public static IBCRM_Client_Resp Convert(dynamic respData, Enum action)
        {
            IBCRM_Client_Resp resp = null;

            JObject jObject_raw = respData;

            if (jObject_raw["data"] is JValue)
            {
                return null;
            }

            JObject jObject_data = (JObject)jObject_raw["data"];

            switch (action)
            {
                case Line_SC_Constant.Service.Api.Action.Push:
                    //resp = jObject_data.ToObject<Line_FlexMessage_Send_Resp>();
                    break;
            }

            return resp;
        }
    }
}
