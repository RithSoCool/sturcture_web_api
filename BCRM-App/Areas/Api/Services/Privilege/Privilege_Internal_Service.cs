using AutoMapper;
using BCRM.Common.Context;
using BCRM.Common.Factory.Entities.Brand;
using BCRM.Common.Helpers;
using BCRM.Common.Models.Common;
using BCRM.Common.Services.Core.Model;
using BCRM.Common.Services.Data;
using BCRM.Common.Services.Privilege;
using BCRM.Privilege.Constants;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM_App.Areas.Api.Services.Customer.Models;
using BCRM_App.Areas.Api.Services.Privilege.Models;
using BCRM_App.Areas.Api.Services.Repository.Customer;
using BCRM_App.Areas.Api.Services.Repository.Wallet;
using BCRM_App.Configs;
using BCRM_App.Constants;
using BCRM_App.Models.DBModels.Duchmill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static TrackingModel;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Areas.Api.Services.Privilege
{
    public interface IPrivilege_Service
    {
        public void SetIdentityContext(string apiRequestId, IBCRM_IdentityContext appIdentityContext = null, IBCRM_IdentityContext userIdentityContext = null);
        public Task<GetCategories_Resp> GetCategories(int? parentCategoryId = null, string identity_SRef = null, int? customerId = null);
        public Task<GetPrivilegies_Resp> GetPrivilegies(int? categoryId, string search = null, string identity_SRef = null, int? customerId = null);
        public GetPrivilegeDetails_Resp GetPrivilegeDetails(string privilegeRef, string identity_SRef = null, int? customerId = null);
        public GetPrivilegeFullDetails_Resp GetPrivilegeFullDetails(string privilegeRef, string identity_SRef = null, int? customerId = null);
        public List<BCRM_Tag_Info> GetPrivilegeTags(int privilegeId, string identity_SRef = null, int? customerId = null);
        public Task<Redeem_Resp> Redeem(Redeem_Req redeemInfo, string identity_SRef = null, int? customerId = null);
        public Privilege_Transaction_History_Resp GetTrackingStatus(int? customerId, string redemptionref);
        public Task<GetPrivilegeStock_Resp> GetPrivilegeStock(int privilegeId, string inventoryRef);
    }

    public partial class Privilege_Internal_Service : Service_Base, IPrivilege_Service
    {
        private readonly IBCRM_Privilege_Service privilege_Service;
        private readonly IMapper mapper;
        private readonly BCRM_36_Entities dbContext;
        private readonly ICustomer_Service customerService;
        private readonly IBCRM_Tag_Service tag_Service;
        private readonly IBCRM_Customer_Repository bcrmCustomerRepository;
        private readonly Address_Repository addressRepository;
        private readonly Wallet_Repository walletRepository;

        public Privilege_Internal_Service(IBCRM_Privilege_Service privilege_Service,
                                          IMapper mapper,
                                          DuchmillModel.BCRM_36_Entities dbContext,
                                          ICustomer_Service customerService,
                                          IBCRM_Tag_Service tag_Service,
                                          IBCRM_Customer_Repository bcrmCustomerRepository,
                                          Address_Repository addressRepository,
                                          Wallet_Repository walletRepository)
        {
            this.privilege_Service = privilege_Service;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.customerService = customerService;
            this.tag_Service = tag_Service;
            this.bcrmCustomerRepository = bcrmCustomerRepository;
            this.addressRepository = addressRepository;
            this.walletRepository = walletRepository;
        }

        public async Task<GetCategories_Resp> GetCategories(int? parentCategoryId = null, string identity_SRef = null, int? customerId = null)
        {
            try
            {
                int activeStatus = 1;
                int primaryCategoryLevel = 1;
                int subCategoryLevel = 2;

                GetCategories_Resp categories_Resp = new GetCategories_Resp();

                using (DuchmillModel.BCRM_36_Entities DutchmillContext = await new BCRM_Brand_Entities_Factory<DuchmillModel.BCRM_36_Entities>().CreateAsync(App_Setting.Brands.Main.Config.Brand_Ref))
                {
                    var categories = DutchmillContext.CRM_Privilege_Categories.Where(it => it.Status == activeStatus && it.IsDeleted == false);
                    if (parentCategoryId == null)
                    {
                        categories = categories.Where(it => it.Category_Level == primaryCategoryLevel);
                    }
                    else
                    {
                        categories = categories.Where(it => it.Parent_CategoryId == parentCategoryId && it.Category_Level == subCategoryLevel);
                    }

                    if (categories != null)
                    {
                        categories_Resp.Categories = categories.ToList();
                    }
                }

                return categories_Resp;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<GetPrivilegies_Resp> GetPrivilegies(int? categoryId, string search = null, string identity_SRef = null, int? customerId = null)
        {
            try
            {
                List_Filters filters = new List_Filters();
                filters.Filters = new List<Data_Filter_Wrp>();

                if (categoryId > 0)
                {
                    Data_Filter_Wrp categoryFilter = new Data_Filter_Wrp
                    {
                        Seq = 1,
                        Key = "category",
                        Value = categoryId.ToString(),
                    };

                    filters.Filters.Add(categoryFilter);
                }

                if (!string.IsNullOrEmpty(search) && search.Length > 0)
                {
                    Data_Filter_Wrp seachFilter = new Data_Filter_Wrp
                    {
                        Seq = 2,
                        Key = "query",
                        Value = search
                    };

                    filters.Filters.Add(seachFilter);
                }

                Data_Filter_Wrp filter = new Data_Filter_Wrp()
                {
                    Key = "Status",
                    Value = "1"
                };

                filters.Filters.Add(filter);

                Privilege_Multi_Filters criteriaFilter = new Privilege_Multi_Filters();

                var privileges = await privilege_Service.Privilege_List(Brand_Ref: this.BrandRef, filters, criteriaFilter);
                GetPrivilegies_Resp getPrivilegies_Resp = new GetPrivilegies_Resp()
                {
                    Privilegies_Resp = privileges,
                };

                return getPrivilegies_Resp;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public GetPrivilegeDetails_Resp GetPrivilegeDetails(string privilegeRef, string identity_SRef = null, int? customerId = null)
        {
            try
            {
                var firstPrivilege = privilege_Service.Get_Privilege(Brand_Ref: this.BrandRef, Privilege_Ref: privilegeRef);
                var _firstPrivilege = mapper.Map<CRM_Privilege_Resp>(firstPrivilege);
                GetPrivilegeDetails_Resp privilegeDetails = new GetPrivilegeDetails_Resp()
                {
                    PrivilegeDetails = _firstPrivilege,
                };

                var subImage = (from img_G in dbContext.CRM_Privilege_Group_Images
                                join img in dbContext.CRM_Privilege_Images on img_G.Group_ImageId equals img.Group_ImageId
                                where img_G.PrivilegeId == firstPrivilege.PrivilegeId && img_G.Key == "sub-img"
                                select new { img.Image_Url }).FirstOrDefault();

                if (subImage != null)
                {
                    privilegeDetails.PrivilegeDetails.Privilege_Sub_Image_Url = subImage.Image_Url;
                }

                return privilegeDetails;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public GetPrivilegeFullDetails_Resp GetPrivilegeFullDetails(string privilegeRef, string identity_SRef = null, int? customerId = null)
        {
            try
            {

                privilege_Service.SetIdentity(idContext: AppIdentityContext);

                var firstPrivilege = privilege_Service.Get_Privilege_Full_Detail(Brand_Ref: this.BrandRef, Privilege_Ref: privilegeRef);
                GetPrivilegeFullDetails_Resp privilegeDetails = new GetPrivilegeFullDetails_Resp()
                {
                    PrivilegeDetails = firstPrivilege, 
                };

                return privilegeDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Redeem_Resp> Redeem(Redeem_Req redeemInfo, string identity_SRef = null, int? customerId = null)
        {
            try
            {
                Redeem_Resp redeem_Resp = new Redeem_Resp();
                PrivilegeDetails privilegeDetails = null;
                int? walletId;
                int quotaLimit = 10;
                string walletAltRef;

                using (DuchmillModel.BCRM_36_Entities DutchmillContext = await new BCRM_Brand_Entities_Factory<DuchmillModel.BCRM_36_Entities>().CreateAsync(App_Setting.Brands.Main.Config.Brand_Ref))
                {

                    //var customerInfo = DutchmillContext.CRM_Customers.Where(it => it.CustomerId == customerId || it.Identity_SRef == identity_SRef).Select(it => new { it.Identity_SRef }).FirstOrDefault();
                    var customerInfo = (from crm in DutchmillContext.CRM_Customers
                                        join bcrm in DutchmillContext.BCRM_Customers on crm.CustomerId equals bcrm.CRM_CustomerId
                                        where crm.CustomerId == customerId || crm.Identity_SRef == identity_SRef
                                        select new { crm.Identity_SRef,  
                                                     bcrm.Wallet_Alt_Ref, 
                                                     bcrm.Wallet_Id,
                                                   }).FirstOrDefault();

                    if (customerInfo == null) throw new Exception("Customer not found.");

                    var transCount = DutchmillContext.CRM_Privilege_Transactions.Where(it => it.CRM_CustomerId == customerId).Count();

                    if (transCount >= quotaLimit) throw new Exception("Out of quota for redeem reward.");

                    identity_SRef = customerInfo.Identity_SRef;
                    walletId = customerInfo.Wallet_Id;
                    walletAltRef = customerInfo.Wallet_Alt_Ref;

                    var _privilegeDetails = (from pv in DutchmillContext.CRM_Privileges
                                             where pv.Alt_Reference == redeemInfo.Privilege_Ref // && catM.Category_Level == 1
                                             select new PrivilegeDetails
                                             {
                                                 Alt_Reference = pv.Alt_Reference,
                                                 Point_Ref = pv.Point_Ref,
                                                 Type = pv.Type,
                                                 CPC_Issue_Mode = pv.CPC_Issue_Mode,
                                                 CPC_Qty_Per_Issue = pv.CPC_Qty_Per_Issue,
                                                 CPC_Type = pv.CPC_Type
                                             }).FirstOrDefault();


                    privilegeDetails = _privilegeDetails;

                }

                if (privilegeDetails == null) throw new System.Exception("Rewards cannot be redeemed. because the reward was not found.");


                var shippingAddress = addressRepository.GetAddress(customerId: customerId.Value, addressType: AppConstants.Customer.Address.Type.ShippingAddress);

                if (shippingAddress == null) throw new System.Exception("Shipping address not found.");

                var _shippingAddress = mapper.Map<BCRM.Common.Models.DBModel.CRM.CRM_Customer_Address>(shippingAddress);

                BCRM_Address_Info addr = new BCRM_Address_Info(_shippingAddress);

                int running = 10000 + customerId.Value;
                string randomStr = StringHelper.Instance.RandomString(5);
                string transactionId = $"P{running.ToString().Substring(1)}-{TxTimeStamp.Month}-{TxTimeStamp.Day}-{randomStr}";


                CRM_Privilege_Issue_Data issueData = new CRM_Privilege_Issue_Data()
                {
                    //Inventory_Ref = config.Privilege_Inventory_Ref,
                    Privilege_Ref = privilegeDetails.Alt_Reference,
                    Issue_Amount = redeemInfo.Issue_Amount.Value * privilegeDetails.CPC_Qty_Per_Issue,
                    Point_Redemption_Mode = BCRM_PV_Const.Transaction.Point.Redemption_Mode.Default,
                    Point_Per_Amount = privilegeDetails.Point_Ref,
                    Wallet_Source = BCRM_PV_Const.Transaction.Wallet_Source.CRM_Wallet,
                    //Wallet_Ref = walletInfo.Wallet_Alt_Ref,
                    Has_FFM = true,
                    Addr_Info = addr,
                };

                CRM_Privilege_Ref_Info refInfo = new CRM_Privilege_Ref_Info();

                privilege_Service.SetIdentity(idContext: AppIdentityContext);

                CRM_Privilege_TX_Result redeemResp = await privilege_Service.Issue(Brand_Ref: this.BrandRef,
                                                                                   Ref_Type: BCRM.CRM.Constants.BCRM_CRM_Const.Customer.Ref_Type.Identity_SRef,
                                                                                   Customer_Ref: identity_SRef,
                                                                                   transactionId,
                                                                                   issueData,
                                                                                   refInfo);

                if (redeemResp.Status == BCRM_PV_Const.Transaction.Result_Status.Success)
                {

                    List<BCRM_Customer_Coupon_Code> couponCodes = new List<BCRM_Customer_Coupon_Code>();
                    List<string> codeRefs = new List<string>();
                    List<CouponCode_Resp> codeInfos = new List<CouponCode_Resp>();

                    using (DuchmillModel.BCRM_36_Entities DutchmillContext = await new BCRM_Brand_Entities_Factory<DuchmillModel.BCRM_36_Entities>().CreateAsync(App_Setting.Brands.Main.Config.Brand_Ref))
                    {

                        if (redeemResp.PV_Type == BCRM_PV_Const.Privilege.Type.Coupon || redeemResp.PV_Type == BCRM_PV_Const.Privilege.Type.Coupon_Code || redeemResp.PV_Type == BCRM_PV_Const.Privilege.Type.Unlimited_Coupon)
                        {
                            var expireConfig = GetExpireTime();

                            var txId = DutchmillContext.CRM_Privilege_Transactions.FirstOrDefault(it => it.TXReference == redeemResp.TXReference).TransactionId;

                            BCRM_Customer_Coupon_Code couponCode = new BCRM_Customer_Coupon_Code()
                            {
                                CRM_CustomerId = customerId.Value,
                                Identity_SRef = identity_SRef,
                                Privilege_Name = redeemResp.Privilege.Name_Th,
                                Privilege_Id = redeemResp.Privilege.PrivilegeId,
                                Privilege_Tx_Id = (int)txId,
                                //ForPOS = RewardForPOS == "POS" ? true : false,
                                Privilege_Tx_Ref = redeemResp.TXReference,
                                Type = redeemResp.PV_Type,
                                Type_Desc = BCRM_PV_Const.Privilege.Type.Get_Desc(redeemResp.PV_Type),
                                Status = redeemResp.Status,
                                Status_Desc = AppConstants.Privilege.Status.GetDesc(redeemResp.Status),
                                Create_DT = TxTimeStamp,
                                Exp_Time_Within_Sec = expireConfig.ExpWithinSec,
                                Absolute_Exp_DT = expireConfig.AbsoluteExpTime,
                                //Brand = config.Name,
                                //BrandId = config.BrandId,
                                Expired_DT = expireConfig.SlideExpTime != null ? expireConfig.SlideExpTime : expireConfig.AbsoluteExpTime
                            };

                            if (BCRM_PV_Const.Privilege.Type.Coupon_Code == redeemResp.PV_Type)
                            {
                                if (redeemResp.CPC_Ledgers != null)
                                {
                                    foreach (var coupon in redeemResp.CPC_Ledgers)
                                    {
                                        var _couponCode = CloneCoupon(couponCode);
                                        _couponCode.CouponCode = coupon.Code;
                                        _couponCode.Reference = Guid.NewGuid();
                                        couponCodes.Add(_couponCode);

                                        codeRefs.Add(_couponCode.Reference.ToString());
                                    }
                                }
                                else
                                {
                                    couponCode.Status = BCRM_PV_Const.Transaction.Result_Status.Failed;
                                    couponCode.Remark = "Code is null.";
                                    couponCodes.Add(couponCode);
                                }
                            }
                            else
                            {
                                couponCode.Remark = "Privilege type an't out of category.";
                                couponCode.Status = BCRM_PV_Const.Transaction.Result_Status.Failed;
                                couponCodes.Add(couponCode);
                            }

                            DutchmillContext.BCRM_Customer_Coupon_Codes.AddRange(couponCodes);
                            DutchmillContext.SaveChanges();
                        }

                        foreach (var couponCode in couponCodes)
                        {

                            var couponCodeResp = customerService.ShowCouponCode(new Customer.Models.Show_Coupon_Code_Req()
                            {
                                CouponCodeRef = couponCode.Reference.Value.ToString(),
                                RedemptionRef = redeemResp.TXReference
                            }, customerId: customerId);

                            codeInfos.Add(couponCodeResp.CodeInfo);
                        }
                    }

                    int totalPoint = redeemInfo.Issue_Amount.Value * (privilegeDetails.Point_Ref * privilegeDetails.CPC_Qty_Per_Issue);
                    int amount = redeemInfo.Issue_Amount.Value;
                    var walletBalance = walletRepository.GetWallet(customerId: customerId, walletId:  walletId, wallet_Alt_Ref: walletAltRef);

                    Reward_Redemption_Resp rewardRedemption = new Reward_Redemption_Resp()
                    {
                        RewardRedemptionRef = redeemResp.TXReference.ToString(),
                        CouponCodeRef = codeRefs.FirstOrDefault(),
                        CouponCodeRefs = codeRefs,
                        Type = redeemResp.Privilege.Type,
                        Privilege_Image_Url = redeemResp.Privilege.Privilege_Image_Url,
                        PointUsed = totalPoint,
                        CouponCodeInfo = codeInfos.FirstOrDefault(),
                        CouponCodeInfos = codeInfos,
                        Balance = (int)walletBalance.Balance,
                    };

                    redeem_Resp.PrivilegeRedemption = rewardRedemption;
                }

                return redeem_Resp;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private (DateTime? AbsoluteExpTime, int? ExpWithinSec, DateTime? SlideExpTime) GetExpireTime()
        {
            DateTime? absoluteExpTime = null;
            int? expWithinSec = null;
            DateTime? slideExpTime = null;

            var expConfigs = App_Setting.Privilege.Expire_Configs.GetExpireTime();


            if (expConfigs.exp != null) absoluteExpTime = TxTimeStamp + expConfigs.exp;
            if (expConfigs.sec != null) expWithinSec = (int)expConfigs.sec;

            if (expWithinSec != null)
            {
                slideExpTime = TxTimeStamp.AddSeconds(expWithinSec.Value);
            }

            return (AbsoluteExpTime: absoluteExpTime, ExpWithinSec: expWithinSec, SlideExpTime: slideExpTime);
        }

        private BCRM_Customer_Coupon_Code CloneCoupon(BCRM_Customer_Coupon_Code couponCode)
        {
            BCRM_Customer_Coupon_Code _couponCode = new BCRM_Customer_Coupon_Code()
            {
                CRM_CustomerId = couponCode.CRM_CustomerId,
                Identity_SRef = couponCode.Identity_SRef,
                Privilege_Name = couponCode.Privilege_Name,
                ForPOS = couponCode.ForPOS,
                Privilege_Id = couponCode.Privilege_Id,
                Privilege_Tx_Id = couponCode.Privilege_Tx_Id,
                Privilege_Tx_Ref = couponCode.Privilege_Tx_Ref,
                Type = couponCode.Type,
                Type_Desc = couponCode.Type_Desc,
                Status = couponCode.Status,
                Status_Desc = couponCode.Status_Desc,
                Use_DT = couponCode.Use_DT,
                Exp_Time_Within_Sec = couponCode.Exp_Time_Within_Sec,
                Absolute_Exp_DT = couponCode.Absolute_Exp_DT,
                Expired_DT = couponCode.Expired_DT,
                Create_DT = couponCode.Create_DT,
                Brand = couponCode.Brand,
                BrandId = couponCode.BrandId,
            };

            return _couponCode;
        }

        public List<BCRM_Tag_Info> GetPrivilegeTags(int privilegeId, string identity_SRef = null, int? customerId = null)
        {
            try
            {
                List<BCRM_Tag_Info> tagInfos = new List<BCRM_Tag_Info>();

                tag_Service.SetIdentity(AppIdentityContext);
                List<BCRM_Tag_Mapping_Info> tag_List = tag_Service.Tags(Brand_Ref: this.BrandRef,
                                                                        Content_Type: BCRM.Data.Constants.BCRM_Data_Const.Tag.Mapping.Type.Privilege,
                                                                        ContentId: privilegeId);
                foreach (var tag in tag_List)
                {
                    var tagDetails = tag_Service.Info(Brand_Ref: this.BrandRef, Tag: tag.Tag);
                    tagInfos.Add(tagDetails);
                }

                return tagInfos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Privilege_Transaction_History_Resp GetTrackingStatus(int? customerId, string redemptionref)
        {
            try
            {

                var trackingStatus = (from ts in dbContext.CRM_Privilege_Transactions
                                      join tk in dbContext.FFM_Tickets on ts.FFM_TicketId equals tk.TicketId
                                      where ts.TXReference.ToString() == redemptionref && ts.CRM_CustomerId == customerId
                                      select new Privilege_Transaction_History_Resp(ts, tk)).FirstOrDefault();

                if (trackingStatus == null) throw new Exception("Tracking no not found.");

                return trackingStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetPrivilegeStock_Resp> GetPrivilegeStock(int privilegeId, string inventoryRef)
        {
            try
            {
                using (DuchmillModel.BCRM_36_Entities DutchmillContext = await new BCRM_Brand_Entities_Factory<DuchmillModel.BCRM_36_Entities>().CreateAsync(App_Setting.Brands.Main.Config.Brand_Ref))
                {
                    var stocks = DutchmillContext.IM_Inv_Entries.Where(it => it.PrivilegeId == privilegeId)
                                                               .Select(it => new GetPrivilegeStock_Resp
                                                               {
                                                                   Remaining_Qty = it.Remaining_Qty,
                                                                   Reserved_Qty = it.Reserved_Qty,
                                                                   Total_Qty = it.Total_Qty,
                                                                   Withdraw_Qty = it.Withdraw_Qty,
                                                               });

                    if (stocks == null) throw new Exception("Stock not found.");

                    decimal withdraw_Qty = stocks.Sum(it => it.Withdraw_Qty);
                    decimal total_Qty = stocks.Sum(it => it.Total_Qty);
                    decimal remaining_Qty = stocks.Sum(it => it.Remaining_Qty);
                    decimal reserved_Qty = stocks.Sum(it => it.Reserved_Qty);


                    GetPrivilegeStock_Resp privilegeStock_Resp = new GetPrivilegeStock_Resp()
                    {
                        Remaining_Qty = remaining_Qty,
                        Reserved_Qty = reserved_Qty,
                        Total_Qty = total_Qty,
                        Withdraw_Qty = withdraw_Qty,
                    };


                    return privilegeStock_Resp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
