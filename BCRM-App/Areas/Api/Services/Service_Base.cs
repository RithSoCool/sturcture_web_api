using BCRM.Common.Context;
using BCRM_App.Configs;
using System;

namespace BCRM_App.Areas.Api.Services
{
    public abstract class Service_Base
    {
        public Service_Base()
        {
            this.BrandRef = App_Setting.Brands.Main.Config.Brand_Ref;
        }

        public DateTime TxTimeStamp { get; internal set; }
        public string BrandRef { get; internal set; }
        public string ApiRequestId { get; internal set; }

        public IBCRM_IdentityContext AppIdentityContext { get; internal set; }
        public IBCRM_IdentityContext UserIdentityContext { get; internal set; }

        public virtual void SetIdentityContext( string apiRequestId, IBCRM_IdentityContext appIdentityContext = null, IBCRM_IdentityContext userIdentityContext = null)
        {
            try
            {
                TxTimeStamp = DateTime.Now;

                this.AppIdentityContext = appIdentityContext;
                this.UserIdentityContext = userIdentityContext;
                this.ApiRequestId = apiRequestId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
