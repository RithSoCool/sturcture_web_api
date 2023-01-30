using AutoMapper;
using BCRM.Common.Context;
using BCRM.Common.Models.Common;
using BCRM.Common.Models.DBModel.Wallet;
using BCRM.Common.Services.CRM;
using BCRM.Common.Services.Data;
using BCRM.Common.Services.Wallet;
using BCRM_App.Areas.Api.Services.Repository.Customer;
using BCRM_App.Configs;
using BCRM_App.Services.RemoteInternal.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static org.apache.zookeeper.ZooDefs;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Areas.Api.Services.Repository.Wallet
{
    public partial class Wallet_Repository : Respository_Base<DuchmillModel.WL_Wallet>
    {
        private readonly IBCRM_Wallet_Service wallet_Service;
        private readonly IBCRM_CRM_Point_Service crm_Point_Service;
        private readonly IBCRM_Cache_Service cache_Service;
        private readonly IMapper mapper;

        public Wallet_Repository(IBCRM_Wallet_Service wallet_Service,
                                 IBCRM_CRM_Point_Service crm_Point_Service,
                                 IBCRM_Cache_Service cache_Service,
                                 IMapper mapper,
                                 DuchmillModel.BCRM_36_Entities dbContext) : base(dbContext)
        {
            this.wallet_Service = wallet_Service;
            this.crm_Point_Service = crm_Point_Service;
            this.cache_Service = cache_Service;
            this.mapper = mapper;

            TxTimeStamp = DateTime.Now;
        }

        public async Task<WL_Wallet> ActiveWallet(int customerId, string identity_SRef, string customerRef, string defaultBrandRef, string brandName)
        {
            try
            {
                if (UserIdentityContext == null) throw new System.Exception("Identity context can't can't be null.");

                var old_wallet = Query(it => it.CRM_CustomerId == customerId && it.Extra_Ref_2 == brandName).FirstOrDefault();

                var config = App_Setting.Brands.Configs.FirstOrDefault(it => it.Name == brandName);
                if (config == null) throw new Exception("Wallet config not found.");

                if (old_wallet == null)
                {

                    BCRM.Common.Models.DBModel.Wallet.WL_Asset asset = wallet_Service.Get_Asset(Brand_Ref: defaultBrandRef, AssetId: config.AssetId);

                    WL_Create_Info walletInfo = new WL_Create_Info()
                    {
                        Scope = BCRM.Wallet.Constants.BCRM_WL_Const.Wallet.Scope.Brand,
                        CRM_CustomerId = customerId,
                        CRM_Customer_Ref = customerRef,

                        // optional
                        WL_AppId = cache_Service.To_AppId(App_Setting.Brands.Main.Config.App_Id),
                        WL_App_Id = App_Setting.Brands.Main.Config.App_Id,
                        WL_IdentityId = cache_Service.To_IdentityId(identity_SRef),
                        WL_Identity_Ref = identity_SRef,
                        Extra_Ref_2 = brandName,
                    };

                    BCRM_Request_Context reqInfo = new BCRM_Request_Context(this.UserIdentityContext);

                    WL_Wallet wallet = await wallet_Service.Create(Brand_Ref: defaultBrandRef,
                                 Asset_Ref: asset.Alt_Reference,
                                 info: walletInfo,
                                 req_Context: reqInfo);

                    return wallet;
                }
                else
                {

                    old_wallet.IsDeleted = false;
                    Update(old_wallet);

                    var w = mapper.Map<WL_Wallet>(old_wallet);
                    return await Task.Run(() => w);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public WL_Wallet InactiveWallet(int customerId, string brandName)
        {
            try
            {
                var customer = ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers.Where(it => it.CRM_CustomerId == customerId).FirstOrDefault();

                if (customer == null) throw new System.Exception("Customer not found");

                var wallet = Query(it => it.CRM_CustomerId == customerId && it.Extra_Ref_2 == brandName).FirstOrDefault();

                if (wallet == null) throw new System.Exception("Wallet not found.");

                wallet.IsDeleted = true;
                wallet.UpdatedTime = TxTimeStamp;
                Update(wallet);


                var walletInactive = mapper.Map<WL_Wallet>(wallet);
                return walletInactive;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public WL_Wallet GetWallet(int? customerId, int? walletId, string wallet_Alt_Ref = null)
        {
            try
            {
                if (walletId == null && wallet_Alt_Ref == null) throw new Exception("Please specify wallet key.");

                var wallet = Query(it => it.CRM_CustomerId == customerId && (it.WalletId == walletId || it.Alt_Reference == wallet_Alt_Ref)).FirstOrDefault();

                if (wallet == null) throw new Exception("Wallet not found.");


                var _wallet = mapper.Map<WL_Wallet>(wallet);
                return _wallet;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<WL_Wallet_TX_Result> EarnPointForTest(int? customerId, int point, string wallet_Alt_Ref = null)
        {
            try
            {
                WL_Add_Funds_Info info = new WL_Add_Funds_Info
                {
                    Reference = null,
                    Extra_Ref = "FOR TEST",
                };

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "caller", "crm"}
                };

                WL_Wallet_TX_Result walletResult = await wallet_Service.Add_Funds_v2(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                                                                    Wallet_Ref: wallet_Alt_Ref,
                                                                                    TransactionId: $"FOR-TEST-{Guid.NewGuid()}",
                                                                                    Amount: point,
                                                                                    info: info,
                                                                                    req_Context: null,
                                                                                    Params: parameters);
                if (walletResult == null)
                {
                    throw new Exception("เกิดข้อผิดพลาดที่ Wallet Service");
                }

                if (walletResult.Status != BCRM.Wallet.Constants.BCRM_WL_Const.Ledger.Result_Status.Success)
                {
                    throw new Exception("เกิดข้อผิดพลาดที่ Wallet Service");
                }

                return walletResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WL_Wallet_TX_Result> EarnPoint(int? customerId, string wallet_Alt_Ref, Transaction earnPoint)
        {
            try
            {
                WL_Add_Funds_Info info = new WL_Add_Funds_Info
                {
                    Reference = earnPoint.Transaction_Ref,
                    Extra_Ref = earnPoint.Transaction_Extra_Ref,
                    Extra_Ref_2 = earnPoint.Transaction_Extra_Ref_2,
                };

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "caller", "crm"}
                };

                WL_Wallet_TX_Result walletResult = await wallet_Service.Add_Funds_v2(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                                                                    Wallet_Ref: wallet_Alt_Ref,
                                                                                    TransactionId: earnPoint.TransactionId,
                                                                                    Amount: earnPoint.Point,
                                                                                    info: info,
                                                                                    req_Context: null,
                                                                                    Params: parameters);
                if (walletResult == null)
                {
                    throw new Exception("เกิดข้อผิดพลาดที่ Wallet Service");
                }

                if (walletResult.Status != BCRM.Wallet.Constants.BCRM_WL_Const.Ledger.Result_Status.Success)
                {
                    throw new Exception("เกิดข้อผิดพลาดที่ Wallet Service");
                }

                return walletResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CRM_Point_TX_Result> EarnPoint_V2(int? customerId, string wallet_Alt_Ref, Transaction earnPoint)
        {
            try
            {
                CRM_Point_Issue_Data issue_Data = new CRM_Point_Issue_Data()
                {
                    Point_Calc = 9,
                    Point = earnPoint.Point,
                    Spending = earnPoint.Point,
                };

                CRM_Point_Ref_Info ref_Info = new CRM_Point_Ref_Info
                {
                    Reference = earnPoint.Transaction_Ref,
                    Extra_Ref = earnPoint.Transaction_Extra_Ref,
                    Extra_Ref_2 = earnPoint.Transaction_Extra_Ref_2
                };

                crm_Point_Service.SetIdentity(this.AppIdentityContext);

               var issue_Result =  await crm_Point_Service.Issue(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                                                Ref_Type: BCRM.CRM.Constants.BCRM_CRM_Const.Customer.Ref_Type.CRM_CustomerId,
                                                                Customer_Ref: customerId.ToString(),
                                                                TransactionId: earnPoint.TransactionId,
                                                                Issue_Data: issue_Data,
                                                                Ref_Info: ref_Info);

                return issue_Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WL_Wallet_TX_Result> VoidPoint(int? customerId, string wallet_Alt_Ref, Transaction transaction)
        {

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "caller", "crm"}
                };

                WL_Void_Add_Info info = new WL_Void_Add_Info
                {
                    Reference = transaction.Transaction_Ref,
                    Extra_Ref = transaction.Transaction_Extra_Ref,
                    Extra_Ref_2 = transaction.Transaction_Extra_Ref_2
                };

                var ledger = await wallet_Service.Void_Add(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                  Wallet_Ref: wallet_Alt_Ref,
                                  Ref_Type: BCRM.Wallet.Constants.BCRM_WL_Const.Ledger.Ref_Type.Wallet,
                                  Ref_TransactionId: transaction.TransactionId,
                                  info: info,
                                  req_Context: null,
                                  Params: parameters);

                WL_Wallet_TX_Result wL_Wallet_TX_Result = new WL_Wallet_TX_Result()
                {
                    Ledger = ledger
                };

                return wL_Wallet_TX_Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<CRM_Point_TX_Result> VoidPoint_V2(int? customerId, string wallet_Alt_Ref, Transaction transaction)
        {

            try
            {

                CRM_Point_Ref_Info ref_Info = new CRM_Point_Ref_Info
                {
                    Reference = transaction.Transaction_Ref,
                    Extra_Ref = transaction.Transaction_Extra_Ref,
                    Extra_Ref_2 = transaction.Transaction_Extra_Ref_2
                };

                crm_Point_Service.SetIdentity(this.AppIdentityContext);

                var void_Result = await crm_Point_Service.Void_Issue(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                                                Ref_Type: BCRM.CRM.Constants.BCRM_CRM_Const.Customer.Ref_Type.CRM_CustomerId,
                                                                Customer_Ref: customerId.ToString(),
                                                                TX_Ref_Type: BCRM.CRM.Constants.BCRM_CRM_Const.Point.Transaction.Ref_Type.CRM,
                                                                TransactionId: transaction.TransactionId,
                                                                Ref_Info: ref_Info);

                return void_Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCRM.Common.Models.DBModel.CRM.CRM_Customer, BCRM.Common.Models.DBModel.CRM.CRM_Point_Transaction)> UpdateBalance_V2(int? customerId, string wallet_Alt_Ref)
        {
            try
            {
                crm_Point_Service.SetIdentity(this.AppIdentityContext);

                var balance = await crm_Point_Service.Update_Balance(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                                                     Ref_Type: BCRM.CRM.Constants.BCRM_CRM_Const.Customer.Ref_Type.CRM_CustomerId,
                                                                     Customer_Ref: customerId.ToString());

                return balance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
