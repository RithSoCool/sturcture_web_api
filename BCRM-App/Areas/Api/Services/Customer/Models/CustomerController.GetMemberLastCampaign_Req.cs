using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Controllers.Customer
{
    public partial class CustomerController
    {
        public class GetMemberLastCampaign_Req
        {
            [Required]
            public string MobileNo { get; set; }
        }
    }
}
