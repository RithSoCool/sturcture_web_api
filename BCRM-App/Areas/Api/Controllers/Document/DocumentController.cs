using BCRM.Common.Factory;
using BCRM.Common.Filters.Action;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static BCRM.Common.Constants.BCRM_Core_Const.Api.Filter;
using System.Collections.Generic;
using BCRM_App.Constants;
using BCRM_App.Filters;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM.Common.Services.RemoteInternal.IAM;
using BCRM.Common.Services.RemoteInternal.IAM.Model;
using BCRM_App.Configs;
using System.Text;
using BCRM.Common.Services.Wallet;
using BCRM_App.Areas.Api.Services.Customer.Models;
using BCRM_App.Areas.Api.Services.Document;
using BCRM_App.Areas.Api.Services.Document.Models;
using BCRM_App.Extentions;
using Org.BouncyCastle.Ocsp;
using BCRM_App.Areas.Api.Services.Privilege;

namespace BCRM_App.Areas.Api.Controllers.Document
{
    [Area("Api")]
    [ApiVersion("1.0")]
    public class DocumentController : API_BCRM_Controller
    {
        private readonly Brand_Document_Service document_Internal_Service;

        public DocumentController(ILogger<Document_Internal_Service> logger,
                                  IBCRM_Exception_Factory bcrm_Ex_Factory,
                                  IDocument_Internal_Service document_Internal_Service,
                                  IHttpContextAccessor httpContext_Accessor) : base(logger, bcrm_Ex_Factory, httpContext_Accessor)
        {
            this.document_Internal_Service = document_Internal_Service as Brand_Document_Service;
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
        public IActionResult Upload([FromForm] Upload_Req req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);

                string identity_SRef = _ctrl_Util.GetRouteData<string>(AppConstants.RouteData.Identity_SRef);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                document_Internal_Service.SetIdentityContext(apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var uploadResp = document_Internal_Service.Upload(customerId: customerId, identity_SRef: identity_SRef, uploadInfo: req);

                Data = uploadResp;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        [BCRM_Api_Logging(Log_Header: true, Log_Req: true, Log_Resp: true, Req_Keys: new string[] { "_req" })]
        [ApiKeyFilter(api_key_localtions: new string[] { "Brands", "Main", "Document", "API_Key" })]
        public async Task<IActionResult> Callback([FromBody] Callback_Extra_Payload _req)
        {
            try
            {
                var req = DictionaryExtention.DictionaryToObject<Callback_Payload>(_req.Params);

                var appIdentityContext = VerifyAppIdentityContext();

                document_Internal_Service.SetIdentityContext(apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var updateStatusResp = await document_Internal_Service.Callback(req);

                //Data = updateStatusResp; 
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Build_JsonResp();
        }
    }
}
