using BCRM.Common.Models.DBModel.Privilege;
using BCRM.Common.Services.Privilege.Model;

namespace BCRM_App.Areas.Api.Services.Privilege
{
    public class GetPrivilegeDetails_Resp
    {
        public CRM_Privilege_Resp PrivilegeDetails { get; set; }
        public int ParentCategoryId { get; set; }
    }

    public class GetPrivilegeFullDetails_Resp
    {
        public Privilege_Detail_Payload PrivilegeDetails { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
