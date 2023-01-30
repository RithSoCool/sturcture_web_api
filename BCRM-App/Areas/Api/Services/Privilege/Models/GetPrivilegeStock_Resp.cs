namespace BCRM_App.Areas.Api.Services.Privilege.Models
{
    public class GetPrivilegeStock_Resp
    {
        public decimal Total_Qty { get; set; }
        public decimal Withdraw_Qty { get; set; }
        public decimal Reserved_Qty { get; set; }
        public decimal Remaining_Qty { get; set; }
    }
}
