using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Services.Privilege
{
    public class Redeem_Req
    {
        [Required]
        public string Privilege_Ref { get; set; }
        [Required]
        public int? Issue_Amount { get; set; }

        public string BrandName { get; set; }
    }
}
