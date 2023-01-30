using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Models
{
    public class Req_Report
    {
        [Required]
        public int Date { get; set; } // UNIX Timestamp
    }

    public class Resp_Report
    {
        public string File;
    }
}
