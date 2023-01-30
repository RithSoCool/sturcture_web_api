using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Services.Customer.Models
{
    public class GetConsent_Req
    {
        public bool? Accept_Privacy_Policy { get; set; }
        public bool? Accept_Activity_Consent { get; set; }
    }
}
