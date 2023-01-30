using static BCRM.Common.Services.Document.BCRM_Document_Service;
using System.Collections.Generic;

namespace BCRM_App.Areas.Api.Services.Document.Models
{
    public class Upload_Resp
    {
        public List<Req_Document_Upload> Upload_Resps { get; set; }
    }
}
