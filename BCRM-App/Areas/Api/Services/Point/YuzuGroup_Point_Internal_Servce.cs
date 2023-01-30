using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BCRM_App.Configs;
using System.Threading.Tasks;
using BCRM_App.Areas.Api.Services.Repository.Customer;
using BCRM_App.Constants;
using BCRM_App.Areas.Api.Services.Repository.Wallet;
using BCRM.Common.Context;
using BCRM_App.Models.DBModels.YuzuGroup;
using BCRM.Common.Services.Wallet;
using BCRM.Common.Models.Common;
using static BCRM_App.Areas.Api.Services.Point.Models.PointModel;
using BCRM.Common.Helpers;

namespace BCRM_App.Areas.Api.Services.Point
{
    public class YuzuGroup_Point_Internal_Service : Point_Internal_Service, IPointService
    {
        //private readonly BCRM_27_Entities entities;
        private readonly IBCRM_Customer_Repository customer_Repository;
        private readonly Wallet_Repository wallet_Repository;
        private readonly IBCRM_Wallet_Service walletService;

        public YuzuGroup_Point_Internal_Service(IBCRM_Customer_Repository customer_Repository,
                                               //YuzuGroupModel.BCRM_27_Entities entities,
                                               Wallet_Repository wallet_Repository,
                                               IBCRM_Wallet_Service walletService)
        {
            //this.entities = entities;
            this.customer_Repository = customer_Repository;
            this.wallet_Repository = wallet_Repository;
            this.walletService = walletService;
        }

        public void Add_Welcome_Point(SpendingInfo spendingInfo, int transactionType, int customerId, int? brandId = null, string brand = null)
        {
            try
            {
                Guid transactionRef = Guid.NewGuid();

                var yuzuGroupOptionsBuilder = new DbContextOptionsBuilder<BCRM_27_Entities>().UseSqlServer(AppConstants.Database.ConnectionString.YuzuGroup);
                using (BCRM_27_Entities yuzuGroupContext = new BCRM_27_Entities(yuzuGroupOptionsBuilder.Options))
                {
                    BCRM_Yuzu_Group_Cumulative_Purchase yuzuWelcomePoint = new BCRM_Yuzu_Group_Cumulative_Purchase()
                    {
                        TXReference = transactionRef,
                        CRM_CustomerId = customerId,
                        Pre_Spending = 0,
                        Spending = 0,
                        Total_Spending = 0,
                        Point_Tran_Id = spendingInfo.TransactionId,
                        Point_Tran_Ref = spendingInfo.TransactionRef,
                        Point_Tran_Ref_Type = transactionType,
                        Point_Tran_Ref_Type_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.ReferenceType.Get_Desc(transactionType),
                        Brand = brand,
                        BrandId = brandId,
                        Create_DT = DateTime.Now,
                        Status = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.WelcomePoint,
                        Status_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.WelcomePointDesc,
                        Remark = "Welcome Point"
                    };

                    yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Add(yuzuWelcomePoint);
                    yuzuGroupContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Calculate_Point_Resp> Add_Cumulative_Purchase(SpendingInfo spendingInfo, int transactionType, int customerId, int? brandId = null, string brand = null)
        {
            try
            {

                var yuzuGroupOptionsBuilder = new DbContextOptionsBuilder<BCRM_27_Entities>().UseSqlServer(AppConstants.Database.ConnectionString.YuzuGroup);
                using (BCRM_27_Entities yuzuGroupContext = new BCRM_27_Entities(yuzuGroupOptionsBuilder.Options))
                {

                    decimal? lastTotalSpending = 0;
                    decimal spending = spendingInfo.Spending;

                    var dupicateTransaction = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Where(it => ((it.Point_Tran_Id != null && it.Point_Tran_Id == spendingInfo.TransactionId) ||
                                                                                                         (it.Point_Tran_Ref != null && it.Point_Tran_Ref == spendingInfo.TransactionRef)))
                                                                                                         .Select(it => "Dupicate transaction.").FirstOrDefault();
                    if (dupicateTransaction != null) throw new Exception("Transaction has dupicated.");

                    var _totalSpending = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Where(it => it.CRM_CustomerId == customerId &&
                                                                                                           (it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Inactive || it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Void))
                                                                                               .Sum(it => it.Spending);

                    if (_totalSpending != null)
                    {
                        lastTotalSpending = _totalSpending;
                    }

                    Guid transactionRef = Guid.NewGuid();

                    BCRM_Yuzu_Group_Cumulative_Purchase spendingData = new BCRM_Yuzu_Group_Cumulative_Purchase()
                    {
                        TXReference = transactionRef,
                        CRM_CustomerId = customerId,
                        Pre_Spending = lastTotalSpending,
                        Spending = spendingInfo.Spending,
                        Total_Spending = lastTotalSpending + spending,
                        Point_Tran_Id = spendingInfo.TransactionId,
                        Point_Tran_Ref = spendingInfo.TransactionRef,
                        Point_Tran_Ref_Type = transactionType,
                        Point_Tran_Ref_Type_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.ReferenceType.Get_Desc(transactionType),
                        Brand = brand,
                        BrandId = brandId,
                        Create_DT = DateTime.Now,
                        Status = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.CumulativeSpending,
                        Status_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.CumulativeSpendingDesc,
                        Remark = spendingInfo.Remark
                    };

                    if (spendingInfo.RecalculatePoint == true)
                    {
                        spendingData.Spending = 0;
                    }

                    yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Add(spendingData);
                    yuzuGroupContext.SaveChanges();
                }

                var currentPointResp = await Calculate_Point(customerId: customerId);

                return currentPointResp;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Calculate_Point_Resp> Calculate_Point(int customerId)
        {
            try
            {
                decimal totalSpending;

                int prevTotalPoint = 0;
                int totalPoint = 0;

                int pointEarn = 0;
                int pointBalance = 0;
                int prePointBalance = 0;

                bool isEarnPoint = false;

                var walletRef = customer_Repository.GetWalletRef(brandName: App_Setting.Brands.YuzuGroup.Config.Name, customerId: customerId);

                var yuzuGroupOptionsBuilder = new DbContextOptionsBuilder<BCRM_27_Entities>().UseSqlServer(AppConstants.Database.ConnectionString.YuzuGroup);
                using (BCRM_27_Entities yuzuGroupContext = new BCRM_27_Entities(yuzuGroupOptionsBuilder.Options))
                {
                    var spendingHistories = from hs in yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases
                                            where hs.CRM_CustomerId == customerId && (
                                                  hs.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.CumulativeSpending ||
                                                  hs.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.RecalPoint)
                                            select hs;

                    //if (spendingHistories == null || spendingHistories.Count() == 0) throw new Exception("Cumulative spending transaction not found.");

                    var _totalSpending = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Where(it => it.CRM_CustomerId == customerId &&
                                                                                                           (it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Inactive || it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Void))
                                                                                               .Sum(it => it.Spending);

                    totalSpending = _totalSpending != null ? _totalSpending.Value : 0;

                    var _prevTotalPoint = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Where(it => it.CRM_CustomerId == customerId && it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.BurnPoint).Sum(it => it.Point);

                    prevTotalPoint = _prevTotalPoint != null ? _prevTotalPoint.Value : 0;
                    prevTotalPoint = prevTotalPoint < 0 ? 0 : prevTotalPoint;
                    totalPoint = Convert.ToInt32(Math.Floor(totalSpending / App_Setting.Brands.YuzuGroup.Config.Point_Rate));

                    if (totalPoint > prevTotalPoint)
                    {

                        var lastEarnPointTran = spendingHistories.OrderBy(it => it.TransactionId).LastOrDefault();

                        pointEarn = totalPoint - prevTotalPoint;

                        WL_Add_Funds_Info info = new WL_Add_Funds_Info
                        {
                            Reference = null,
                            Extra_Ref = App_Setting.Brands.YuzuGroup.Config.Name,
                            Extra_Ref_2 = pointEarn.ToString()
                        };

                        Dictionary<string, object> parameters = new Dictionary<string, object>()
                        {
                            { "caller", "crm"}
                        };

                        WL_Wallet_TX_Result walletResult = await walletService.Add_Funds_v2(Brand_Ref: App_Setting.Brands.YuzuGroup.Config.Brand_Ref,
                                                                                            Wallet_Ref: walletRef.Wallet_Alt_Ref,
                                                                                            TransactionId: lastEarnPointTran.TXReference.ToString(),
                                                                                            Amount: pointEarn,
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

                        if (walletResult.Is_Duplicate_TX == true)
                        {
                            throw new Exception("Reference ซ้ำ");
                        }

                        foreach (var spendingHistory in spendingHistories)
                        {
                            if (spendingHistory.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.RecalPoint)
                            {
                                spendingHistory.Wallet_LedgerId = (int)walletResult.Ledger.LedgerId;
                                spendingHistory.WalletId = (int)walletResult.Ledger.WalletId;
                                spendingHistory.Wallet_Alt_Ref = walletRef.Wallet_Alt_Ref;
                                spendingHistory.Wallet_LedgerRef = walletResult.TXReference.ToString();
                            }
                            else
                            {
                                spendingHistory.Wallet_LedgerId = (int)walletResult.Ledger.LedgerId;
                                spendingHistory.WalletId = (int)walletResult.Ledger.WalletId;
                                spendingHistory.Wallet_Alt_Ref = walletRef.Wallet_Alt_Ref;

                                spendingHistory.Status = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.EarnPoint;
                                spendingHistory.Status_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.EarnPointDesc;
                                spendingHistory.Update_DT = DateTime.Now;
                                spendingHistory.Wallet_LedgerRef = walletResult.TXReference.ToString();
                            }
                        }

                        if (lastEarnPointTran != null)
                        {
                            lastEarnPointTran.Last_Bill_For_Earn_Point = true;
                            lastEarnPointTran.Point = pointEarn;
                            lastEarnPointTran.Spending_Rate_To_Point = App_Setting.Brands.YuzuGroup.Config.Point_Rate;
                            //lastEarnPointTran.Spending_Left = totalSpending - (totalPoint * App_Setting.Brands.YuzuGroup.Config.Point_Rate);
                        }

                        yuzuGroupContext.SaveChanges();

                        prePointBalance = (int)walletResult.Ledger.Pre_Balance;
                        pointBalance = (int)walletResult.Ledger.Balance;
                        isEarnPoint = true;
                    }
                    else
                    {
                        foreach (var spendingHistory in spendingHistories)
                        {
                            if (spendingHistory.Wallet_Alt_Ref != null) continue;

                            spendingHistory.Wallet_Alt_Ref = walletRef.Wallet_Alt_Ref;
                            spendingHistory.Spending_Rate_To_Point = App_Setting.Brands.YuzuGroup.Config.Point_Rate;
                        }

                        yuzuGroupContext.SaveChanges();

                        var walletInfo = wallet_Repository.GetWallet(walletId: walletRef.Wallet_Id, customerId: customerId);
                        prePointBalance = (int)walletInfo.Balance;
                        totalPoint = 0;
                        pointBalance = (int)walletInfo.Balance;
                        isEarnPoint = false;
                    }

                    var bcrmCustomerInfo = yuzuGroupContext.BCRM_Customers.Where(it => it.CRM_CustomerId == customerId).FirstOrDefault();
                    if (bcrmCustomerInfo == null) throw new Exception("Customer not found.");

                    bcrmCustomerInfo.YuzuGroup_Total_Spending = totalSpending;
                    bcrmCustomerInfo.YuzuGroup_Total_Spending_At_Round = totalSpending;
                    bcrmCustomerInfo.YuzuGroup_Point_Balance = pointBalance;
                    yuzuGroupContext.Update(bcrmCustomerInfo);
                    yuzuGroupContext.SaveChanges();
                }

                Calculate_Point_Resp calculate_Point_Resp = new Calculate_Point_Resp()
                {
                    PrePointBalance = prePointBalance,
                    EarnPoint = pointEarn,
                    PointBalance = pointBalance,
                    Pre_Spending = totalSpending,
                    Total_Spending = totalSpending,
                    IsEarnPoint = isEarnPoint,
                };

                return calculate_Point_Resp;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public BurnPoint_Resp BurnPoint(PointInfo pointInfo, int transactionType, int customerId, int? brandId = null, string brand = null)
        {
            try
            {
                BurnPoint_Resp burnPoint_Resp = new BurnPoint_Resp();

                var yuzuGroupOptionsBuilder = new DbContextOptionsBuilder<BCRM_27_Entities>().UseSqlServer(AppConstants.Database.ConnectionString.YuzuGroup);
                using (BCRM_27_Entities yuzuGroupContext = new BCRM_27_Entities(yuzuGroupOptionsBuilder.Options))
                {
                    var burnPointTrans = yuzuGroupContext.WL_Wallet_Ledgers.Where(it => it.Ext_TransactionId == pointInfo.TransactionRef).FirstOrDefault();
                    if (burnPointTrans == null) throw new Exception("Transaction burn point not found.");

                    var walletInfo = wallet_Repository.GetWallet(customerId: customerId, walletId: burnPointTrans.WalletId);

                    BCRM_Yuzu_Group_Cumulative_Purchase burnPoint = new BCRM_Yuzu_Group_Cumulative_Purchase()
                    {
                        CRM_CustomerId = customerId,
                        Point = pointInfo.Point > 0 ? pointInfo.Point * -1 : pointInfo.Point,
                        Point_Tran_Id = pointInfo.TransactionId,
                        Point_Tran_Ref = pointInfo.TransactionRef,
                        Point_Tran_Ref_Type = transactionType,
                        Point_Tran_Ref_Type_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.ReferenceType.Get_Desc(transactionType),
                        Brand = brand,
                        WalletId = burnPointTrans.WalletId,
                        Wallet_Alt_Ref = walletInfo.Alt_Reference,
                        Wallet_LedgerRef = burnPointTrans.Ext_TransactionId,
                        Wallet_LedgerId = (int)burnPointTrans.LedgerId,
                        BrandId = brandId,
                        Create_DT = DateTime.Now,
                        Status = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.BurnPoint,
                        Status_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.BurnPointDesc,
                    };

                    yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Add(burnPoint);
                    yuzuGroupContext.SaveChanges();

                    var bcrmCustomerInfo = yuzuGroupContext.BCRM_Customers.Where(it => it.CRM_CustomerId == customerId).FirstOrDefault();
                    if (bcrmCustomerInfo == null) throw new Exception("Customer not found.");

                    bcrmCustomerInfo.YuzuGroup_Point_Balance = walletInfo.Balance;
                    yuzuGroupContext.Update(bcrmCustomerInfo);
                    yuzuGroupContext.SaveChanges();

                    burnPoint_Resp.PointBalance = (int)walletInfo.Balance;
                }

                return burnPoint_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<VoidPoint_Resp> VoidPoint(VoidPointInfo voidPointInfo, int transactionType, int customerId, int? brandId = null, string brand = null)
        {
            try
            {
                int? wallet_LedgerId = null;
                BCRM_Yuzu_Group_Cumulative_Purchase transaction = null;
                BCRM.Common.Models.DBModel.Wallet.WL_Wallet_Ledger voidLedgerTransaction = null;

                var yuzuGroupOptionsBuilder = new DbContextOptionsBuilder<BCRM_27_Entities>().UseSqlServer(AppConstants.Database.ConnectionString.YuzuGroup);
                using (BCRM_27_Entities yuzuGroupContext = new BCRM_27_Entities(yuzuGroupOptionsBuilder.Options))
                {
                    transaction = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.FirstOrDefault(it => it.Point_Tran_Ref == voidPointInfo.TransactionRef);
                    if (transaction == null) throw new Exception("Transaction not found.");
                    if (transaction.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Void || transaction.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Inactive) throw new Exception("Transaction already voided.");
                }

                if (transaction.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.EarnPoint || transaction.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.RecalPoint)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
                        {
                            { "caller", "crm"}
                        };

                    WL_Void_Add_Info voidInfo = new WL_Void_Add_Info()
                    {
                        Extra_Ref = transaction.Brand,
                        Extra_Ref_2 = (transaction.Spending != null ? transaction.Spending.Value * -1 : 0).ToString()
                    };
                    IBCRM_IdentityContext identityContext = new IdentityContext
                    {
                        RequestId = Guid.NewGuid().ToString()
                    };

                    identityContext.Set_IAM_Token(App_Setting.Brands.YuzuGroup.Config.App_Token);
                    identityContext.Verify();

                    BCRM_Request_Context reqContext = new BCRM_Request_Context(identityContext);

                    var voidRes = await walletService.Void_Add(Brand_Ref: App_Setting.Brands.YuzuGroup.Config.Brand_Ref,
                                                 Wallet_Ref: transaction.Wallet_Alt_Ref,
                                                 Ref_Type: BCRM.Wallet.Constants.BCRM_WL_Const.Ledger.Ref_Type.Wallet,
                                                 Ref_TransactionId: transaction.Wallet_LedgerRef,
                                                 info: voidInfo,
                                                 req_Context: reqContext,
                                                 Params: parameters);
                    voidLedgerTransaction = voidRes;

                }

                using (BCRM_27_Entities yuzuGroupContext = new BCRM_27_Entities(yuzuGroupOptionsBuilder.Options))
                {
                    decimal totalSpending;

                    var _totalSpending = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Where(it => it.CRM_CustomerId == customerId &&
                                                                                       (it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Inactive || it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Void))
                                                                           .Sum(it => it.Spending);

                    totalSpending = _totalSpending != null ? _totalSpending.Value : 0;

                    BCRM_Yuzu_Group_Cumulative_Purchase voidTransaction = new BCRM_Yuzu_Group_Cumulative_Purchase()
                    {
                        CRM_CustomerId = customerId,
                        TXReference = Guid.NewGuid(),
                        Point_Tran_Id = voidPointInfo.TransactionId,
                        Point_Tran_Ref = voidPointInfo.TransactionRef,
                        Pre_Spending = totalSpending,
                        Spending = transaction.Spending * -1,
                        Total_Spending = totalSpending - transaction.Spending,
                        Point_Tran_Ref_Type = transactionType,
                        Point_Tran_Ref_Type_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.ReferenceType.Get_Desc(transactionType),
                        WalletId = transaction.WalletId,
                        Wallet_LedgerRef = voidLedgerTransaction != null ? voidLedgerTransaction.Reference : null,
                        Wallet_LedgerId = voidLedgerTransaction != null ? (int)voidLedgerTransaction.LedgerId : 0,
                        Wallet_Alt_Ref = transaction.Wallet_Alt_Ref,
                        Brand = brand,
                        BrandId = brandId,
                        Create_DT = DateTime.Now,
                        Update_DT = DateTime.Now,
                        Status = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Void,
                        Status_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.VoidDesc,
                    };

                    if (transaction.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.EarnPoint || transaction.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.RecalPoint)
                    {
                        var voidLedger = yuzuGroupContext.WL_Wallet_Ledgers.Where(it => it.Ref_LedgerId == transaction.Wallet_LedgerId).FirstOrDefault();

                        if (voidLedger != null)
                        {
                            var dupicateTx = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Where(it => it.Wallet_LedgerId == voidTransaction.Wallet_LedgerId && it.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Void);
                            if (dupicateTx.Count() == 0)
                            {
                                wallet_LedgerId = (int)voidLedger.LedgerId;
                                voidTransaction.Point = (int)voidLedger.TX_Amount;
                                voidTransaction.Wallet_LedgerId = (int)voidLedger.LedgerId;
                            }
                        }
                    }
                    yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.Add(voidTransaction);
                    yuzuGroupContext.SaveChanges();

                    transaction.Status = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Inactive;
                    transaction.Status_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.InactiveDesc;

                    //if (voidLedgerTransaction != null) transaction.Wallet_LedgerId = (int)voidLedgerTransaction.LedgerId;
                    //if (voidLedgerTransaction != null) transaction.Wallet_LedgerRef = voidLedgerTransaction.Reference;

                    transaction.Void_TransactionId = voidTransaction.TransactionId;
                    transaction.Void_Transaction_DT = DateTime.Now;
                    transaction.Update_DT = DateTime.Now;
                    yuzuGroupContext.Update(transaction);
                    yuzuGroupContext.SaveChanges();

                    var bcrmCustomerInfo = yuzuGroupContext.BCRM_Customers.Where(it => it.CRM_CustomerId == customerId).FirstOrDefault();
                    if (bcrmCustomerInfo == null) throw new Exception("Customer not found.");

                    var walletInfo = wallet_Repository.GetWallet(customerId: customerId, walletId: voidTransaction.WalletId);

                    bcrmCustomerInfo.YuzuGroup_Point_Balance = walletInfo.Balance;
                    yuzuGroupContext.Update(bcrmCustomerInfo);
                    yuzuGroupContext.SaveChanges();
                }

                Calculate_Transactions(customerId: customerId, transactionId: transaction.TransactionId);

                string voidRef = $"Recal-{StringHelper.Instance.RandomString(15)}";

                // add 0 spending to recalculate transaction
                var spendingInfo = new SpendingInfo()
                {
                    Spending = 0,
                    TransactionId = null,
                    TransactionRef = voidRef,
                    Remark = "Recalculate transaction",
                    RecalculatePoint = true
                };

                var res = await Add_Cumulative_Purchase(spendingInfo: spendingInfo, transactionType: transactionType, customerId: customerId, brandId: brandId, brand: brand);

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Calculate_Transactions(int customerId, int transactionId)
        {
            try
            {
                var yuzuGroupOptionsBuilder = new DbContextOptionsBuilder<BCRM_27_Entities>().UseSqlServer(AppConstants.Database.ConnectionString.YuzuGroup);
                using (BCRM_27_Entities yuzuGroupContext = new BCRM_27_Entities(yuzuGroupOptionsBuilder.Options))
                {
                    var voidTran = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.FirstOrDefault(it => it.TransactionId == transactionId && it.CRM_CustomerId == customerId);
                    if (voidTran == null) throw new Exception("Transaction not found.");

                    var lastTransaction = yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases.OrderBy(it => it.TransactionId)
                                                                   .LastOrDefault(it => it.CRM_CustomerId == customerId &&
                                                                                        it.TransactionId < transactionId &&
                                                                                        it.Status != AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.Void);

                    var spendingHistories = (from hs in yuzuGroupContext.BCRM_Yuzu_Group_Cumulative_Purchases
                                             where hs.CRM_CustomerId == customerId &&
                                                  (hs.TransactionId > transactionId || hs.Wallet_LedgerId == voidTran.Wallet_LedgerId)
                                             select hs).OrderBy(it => it.TransactionId);

                    spendingHistories = spendingHistories.Where(it => it.Wallet_LedgerId == voidTran.Wallet_LedgerId ||
                                                                it.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.CumulativeSpending ||
                                                                it.Status == AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.RecalPoint
                                                            ).OrderBy(it => it.TransactionId);

                    if (spendingHistories == null || spendingHistories.Count() == 0) throw new Exception("Cumulative spending transaction not found.");

                    decimal? Last_Total_Spending = lastTransaction != null ? lastTransaction.Total_Spending : 0;

                    Last_Total_Spending = Last_Total_Spending != null ? Last_Total_Spending : 0;

                    int index = 0;
                    foreach (var item in spendingHistories)
                    {
                        if (item.TransactionId == voidTran.TransactionId) continue;

                        //item.Pre_Spending = Last_Total_Spending;
                        //item.Spending = item.Spending;
                        //item.Total_Spending = item.Pre_Spending + item.Spending;

                        item.Status = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.RecalPoint;
                        item.Status_Desc = AppConstants.Point.YuzuGroup.Cumulative_Purchase.Status.RecalPointDesc;
                        item.Update_DT = DateTime.Now;
                        item.Remark = $"Recalculate point at [{item.Update_DT.Value.ToString("dd/MM/yyyy HH:mm:ss")}]";

                        Last_Total_Spending = item.Total_Spending;
                        index++;
                    }
                    yuzuGroupContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}