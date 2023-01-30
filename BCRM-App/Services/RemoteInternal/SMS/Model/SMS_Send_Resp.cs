using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM_App.Services.RemoteInternal.SMS.Model
{
    public class SMS_Send_Resp : IBCRM_Client_Resp
    {
        public String TransactionId { get; set; }
        public String RequestRef { get; set; }

        public String Sender { get; set; }
        public String MobileNo { get; set; }

        public String Message { get; set; }

        public int CreditUsed { get; set; }
        public String SentTime { get; set; }

        public JObject RawData { get; set; }

        public SMS_Send_Resp(JObject resp)
        {
            this.RawData = resp;
            JObject msg = resp["data"]["Message"].ToObject<JObject>();

            try { this.TransactionId = (String)msg["TransactionId"]; } catch (Exception ex) { }
            try { this.RequestRef = (String)msg["RequestRef"]; } catch (Exception ex) { }

            try { this.Sender = (String)msg["Sender"]; } catch (Exception ex) { }
            try { this.MobileNo = (String)msg["MobileNo"]; } catch (Exception ex) { }

            try { this.Message = (String)msg["Message"]; } catch (Exception ex) { }

            try { this.CreditUsed = (int)msg["CreditUsed"]; } catch (Exception ex) { }
            try { this.SentTime = (String)msg["SentTime"]; } catch (Exception ex) { }
        }
    }
}
