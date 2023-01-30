using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM_App.Services.RemoteInternal.SMS.Model
{
    public class SMS_Verification_OTP_Resend_Resp : IBCRM_Client_Resp
    {
        public String TransactionId { get; set; }
        public String RequestRef { get; set; }
        public int Status { get; set; }
        public String StatusDesc { get; set; }
        public String SenderName { get; set; }
        public String MobileNo { get; set; }
        public String OTP_Reference { get; set; }
        public int ExpireIn_Second { get; set; }
        public String SentTime { get; set; }
        public String ResentTime { get; set; }
        public String ExpireTime { get; set; }

        public JObject RawData { get; set; }

        public SMS_Verification_OTP_Resend_Resp(JObject resp)
        {
            this.RawData = resp;

            JObject msg = resp["data"]["OTP"].ToObject<JObject>();

            try { this.TransactionId = (String)msg["TransactionId"]; } catch (Exception ex) { }
            try { this.RequestRef = (String)msg["RequestRef"]; } catch (Exception ex) { }
            try { this.Status = (int)msg["Status"]; } catch (Exception ex) { }
            try { this.StatusDesc = (String)msg["StatusDesc"]; } catch (Exception ex) { }
            try { this.SenderName = (String)msg["SenderName"]; } catch (Exception ex) { }
            try { this.MobileNo = (String)msg["MobileNo"]; } catch (Exception ex) { }
            try { this.OTP_Reference = (String)msg["OTP_Reference"]; } catch (Exception ex) { }
            try { this.ExpireIn_Second = (int)msg["ExpireIn_Second"]; } catch (Exception ex) { }
            try { this.ResentTime = (String)msg["ResentTime"]; } catch (Exception ex) { }
            try { this.ExpireTime = (String)msg["ExpireTime"]; } catch (Exception ex) { }
            try { this.SentTime = (String)msg["SentTime"]; } catch (Exception ex) { }
        }
    }
}
