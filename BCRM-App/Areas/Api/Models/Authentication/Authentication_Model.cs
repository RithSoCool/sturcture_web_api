using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Models
{
    public class Req_Authentication_GenerateToken
    {
        [Required]
        public string PartnerKey { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int BranchId { get; set; }
    }

    public class Resp_Authentication_Token
    {
        public string Account_Token;
    }
}
