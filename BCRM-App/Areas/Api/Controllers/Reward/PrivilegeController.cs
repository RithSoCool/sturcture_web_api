using BCRM.Common.Factory;
using BCRM.Common.Filters.Action;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Constants.BCRM_Core_Const.Api.Filter;
using BCRM_App.Constants;
using BCRM_App.Filters;
using BCRM_App.Areas.Api.Services.Privilege;
using AutoMapper;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM_App.Configs;
using static BCRM_App.Areas.Api.Services.Privilege.Privilege_Internal_Service;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Areas.Api.Controllers.Reward
{
    [Area("Api")]
    [ApiVersion("1.0")]
    [ApiAuthorize(brandScope: AppConstants.Token.Scope.API)]
    public class PrivilegeController : API_BCRM_Controller
    {
        private readonly IMapper mapper;
        private readonly ICustomer_Service customerService;
        private readonly IPrivilege_Service privilege_Service;
        private readonly BCRM_36_Entities dbContext;

        public PrivilegeController(ILogger<PrivilegeController> logger,
                                IBCRM_Exception_Factory bcrm_Ex_Factory,
                                IMapper mapper,
                                ICustomer_Service customerService,
                                IPrivilege_Service privilege_Service,
                                DuchmillModel.BCRM_36_Entities dbContext,
                                IHttpContextAccessor httpContext_Accessor) : base(logger, bcrm_Ex_Factory, httpContext_Accessor)
        {
            this.mapper = mapper;
            this.customerService = customerService;
            this.privilege_Service = privilege_Service;
            this.dbContext = dbContext;
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                privilege_Service.SetIdentityContext(apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var privilegeCategories = await privilege_Service.GetCategories(customerId: customerId);

                privilegeCategories.Categories = privilegeCategories.Categories.Where(it => it.Name_En != "FORPOS").ToList();

                Data = privilegeCategories;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public async Task<IActionResult> GetSubCategories(int? parentCategoryId)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                privilege_Service.SetIdentityContext( apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var privilegeCategories = await privilege_Service.GetCategories(parentCategoryId: parentCategoryId, customerId: customerId);
                
                Data = privilegeCategories;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetPrivilegies(int? categoryId, bool isCheckStock = true)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                privilege_Service.SetIdentityContext( apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var privilegies = await privilege_Service.GetPrivilegies(categoryId: categoryId, customerId: customerId);

                
                
                if (privilegies != null && privilegies.Privilegies_Resp != null && privilegies.Privilegies_Resp.Privileges != null)
                {
                    var _privilegeResp = mapper.Map<GetPrivilegiesWithTier_Resp>(privilegies);

                    foreach (var privilege in _privilegeResp.Privileges)
                    {

                        if (isCheckStock)
                        {
                            try
                            {
                                var stock = await privilege_Service.GetPrivilegeStock(privilegeId: privilege.PrivilegeId, inventoryRef: null);
                                privilege.StockRemaining = stock.Remaining_Qty;
                            }
                            catch (Exception ex)
                            {
                                // log
                            }
                        }

                        var tier = privilege_Service.GetPrivilegeTags(privilegeId: privilege.PrivilegeId);
                        if (tier == null) continue;

                        var tag = tier.FirstOrDefault();
                        if (tag == null) continue;

                        privilege.TierName = tag.Tag;
                        privilege.TierValue = tag.Data.Int_Val;

                    }

                    var redeemCount = dbContext.CRM_Privilege_Transactions.Where(it => it.CRM_CustomerId == customerId).Count();
                    _privilegeResp.RedeemCount = redeemCount;
                    
                    var pointBalance = customerService.GetPointBalance(customerId: customerId);
                    _privilegeResp.Point_Balance = pointBalance.PointBalance;

                    Data = _privilegeResp;
                    Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
                }
                else
                {
                    Data = new GetPrivilegiesWithTier_Resp();
                    Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
                }
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetPrivilegeByRef(string privilegeRef, bool isCheckStock = true)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                privilege_Service.SetIdentityContext( apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var privilegeDetails = privilege_Service.GetPrivilegeDetails(privilegeRef: privilegeRef, customerId: customerId);

                if (privilegeDetails == null && privilegeDetails.PrivilegeDetails == null) throw new Exception("Privilege not found.");

                var _privilegeDetailResp = mapper.Map<GetPrivilegiesDetailsWithTier_Resp>(privilegeDetails);

                if (isCheckStock == true)
                {
                    var stock = await privilege_Service.GetPrivilegeStock(privilegeId: privilegeDetails.PrivilegeDetails.PrivilegeId, inventoryRef: null);

                    _privilegeDetailResp.PrivilegeDetails.StockRemaining = stock.Remaining_Qty;
                }

                //var tier = privilege_Service.GetPrivilegeTags(privilegeId: _privilegeDetailResp.PrivilegeDetails.PrivilegeId);
                //if (tier != null)
                //{
                //    var tag = tier.FirstOrDefault();
                //    if (tag != null)
                //    {
                //        _privilegeDetailResp.PrivilegeDetails.TierName = tag.Tag;
                //        _privilegeDetailResp.PrivilegeDetails.TierValue = tag.Data.Int_Val;

                //        string[] tierArr = _privilegeDetailResp.PrivilegeDetails.TierName.Split('-');
                //        string tierName = _privilegeDetailResp.PrivilegeDetails.TierName;

                //        if (tierArr.Length == 2) tierName =  tierArr[tierArr.Length - 1];

                //        var tiers = customerService.GetCustomerTiers(customerId: customerId);
                //        if (tiers != null)
                //        {
                //            var isSameTier = tiers.FirstOrDefault(it => it.TierName == _privilegeDetailResp.PrivilegeDetails.TierName);
                //            if (isSameTier == null)
                //            {
                //                _privilegeDetailResp.Error = new ErrorResp()
                //                {

                //                    Titile_TH = "ไม่สามารถแลกของรางวัลได้",
                //                    IsError = true,
                //                    Message_TH = $"ของรางวัลนี้สำหรับสมาชิก ระดับ {tierName} เท่านั้น",
                //                    Remark = tierName
                //                };
                //            }
                //        }
                //    }
                //}

                var redeemCount = dbContext.CRM_Privilege_Transactions.Where(it => it.CRM_CustomerId == customerId).Count();

                _privilegeDetailResp.RedeemCount = redeemCount;


                var pointBalance = customerService.GetPointBalance(customerId: customerId);
                _privilegeDetailResp.Point_Balance = pointBalance.PointBalance;

                Data = _privilegeDetailResp;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public IActionResult GetPrivilegeTags(int privilegeId)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                privilege_Service.SetIdentityContext( apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var privilegeTags = privilege_Service.GetPrivilegeTags(privilegeId: privilegeId, customerId: customerId);

                Data = privilegeTags;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public IActionResult GetPrivilegeFullDetails(string privilegeRef)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));

                var appIdentityContext = VerifyAppIdentityContext();

                privilege_Service.SetIdentityContext(apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var privilegeDetails = privilege_Service.GetPrivilegeFullDetails(privilegeRef: privilegeRef, customerId: customerId);

                Data = privilegeDetails;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        public async Task<IActionResult> Redeem([FromBody] Redeem_Req req)
        {
            try
            {
                if (!ModelState.IsValid) ModelState.ThrowErrorModelStage(_bcrm_Ex_Factory);
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));
                string identity_SRef = _ctrl_Util.GetRouteData<string>(AppConstants.RouteData.Identity_SRef);

                var appIdentityContext = VerifyAppIdentityContext();

                privilege_Service.SetIdentityContext( apiRequestId: this.Api_RequestId, userIdentityContext: IdentityContext, appIdentityContext: appIdentityContext);

                var redeemResp = await privilege_Service.Redeem(redeemInfo: req, identity_SRef: identity_SRef, customerId: customerId);

                Data = redeemResp;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public IActionResult GetTrackingStatus(string redemptionref)
        {
            try
            {
                int customerId = Int32.Parse(_ctrl_Util.GetRouteData<string>(AppConstants.RouteData.CustomerId));
                var tackingStatus = privilege_Service.GetTrackingStatus(customerId: customerId, redemptionref: redemptionref);

                Data = tackingStatus;

                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }

            return Build_JsonResp();
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public async Task<IActionResult> GetPrivilegeStock(int privilegeId)
        {
            try
            {
                var stock = await privilege_Service.GetPrivilegeStock(privilegeId: privilegeId, inventoryRef: null);

                Data = stock;
                Status = BCRM.Common.Constants.BCRM_Core_Const.Api.Result_Status.Success;
            }
            catch (Exception ex)
            {
                Build_BCRM_Exception(ex);
            }
            return Build_JsonResp();
        }
    }
}
