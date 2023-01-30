namespace BCRM_App.Areas.Api.Services.Privilege
{

    public partial class Privilege_Internal_Service
    {
        public class PrivilegeDetails
        {
            public string Alt_Reference { get; set; }
            public int Point_Ref { get; set; }
            public int CategoryId { get; set; }
            public int Category_Level { get; set; }
            public string BrandName { get; set; }
            public int Type { get; set; }
            public int CPC_Type { get; set; }
            public int CPC_Issue_Mode { get; set; }
            public int CPC_Qty_Per_Issue { get; set; }
        }
    }
}
