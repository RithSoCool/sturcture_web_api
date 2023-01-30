using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;

namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models
{
    public class Line_Response
    {
        public bool Success { get; set; }
        public IBCRM_Client_Resp ResponseSuccess { get; set; }
        public BCRM_Client_Error_Resp ResponseError { get; set; }
    }
}
