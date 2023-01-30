using System;
using System.Linq;
using BCRM_App.Services.RemoteInternal.Repository;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using System.Collections.Generic;
using BCRM_App.Configs;
using System.Threading.Tasks;
using AutoMapper;
using BCRM.Common.Services.CRM;
using BCRM.Common.Services.CRM.Model;
using BCRM_App.Areas.Api.Services.Customer.Models;
using BCRM_App.Areas.Api.Services.Repository.Customer;
using BCRM.Common.Services.Storage;
using BCRM_App.Constants;
using System.IO;
using Azure.Storage.Blobs.Models;
using BCRM_App.Areas.Api.Services.Repository.Wallet;
using BCRM.Common.Context;
using Pv_Tx_Ref = BCRM_App.Constants.AppConstants.Privilege.RewardRedemptionHistory.ActionType;
using BCRM.Privilege.Constants;
using BCRM.Common.Services.Data;
using BCRM_App.Areas.Api.Services.Repository.Customer.Models;
using BCRM.Common.Services.Wallet;
using BCRM_App.Extentions;
using Microsoft.EntityFrameworkCore;
using static BCRM_App.Areas.Api.Services.Privilege.Privilege_Internal_Service;

namespace BCRM_App.Areas.Api.Services.Customer
{

    public interface ICustomer_Service
    {
        public void SetIdentityContext(string apiRequestId, IBCRM_IdentityContext appIdentityContext = null, IBCRM_IdentityContext userIdentityContext = null);
        public Task<Register_Resp> Register(RegisterModel customerData);
        public Task<Edit_Customer_Info_Resp> EditCustomerInfo(EditProfileReq profile, string identity_SRef = null, int? customerId = null);
        public Get_Customer_Info_Resp GetCustomerInfo(string identity_SRef = null, int? customerId = null);
        public List<CRM_Customer_Address_Resp> GetAddressList(int? addressType, int? customerId);
        public CRM_Customer_Address_Resp GetAddress(int? addressId, int? addressType, int? customerId);
        public CRM_Customer_Address_Resp AddAddress(int? addressType, AddAddressModel address);
        public CRM_Customer_Address_Resp DeleteAddress(int addressId, int customerId);
        public CRM_Customer_Address_Resp UpdateAddress(int? addressType, AddressModel address);
        public string GetImageProfile(string customerReference);
        public Point_Balance_Resp GetPointBalance(int? customerId = null);
        public List<Point_History_Resp> GetPointHistory(DateTime? start = null, DateTime? end = null, string identity_SRef = null, int? customerId = null);
        public Redemption_History_Resp GetRedemptionHistory(int? rewardType = null, DateTime? start = null, DateTime? end = null, string identity_SRef = null, int? customerId = null);
        public Show_Coupon_Code_Resp ShowCouponCode(Show_Coupon_Code_Req codeRef, int? customerId = null);
        public List<BCRM_Tag_Mapping_Info> GetCustomerTags(int customerId);
        public Get_Consent_Resp GetConsent(int customerId);
        public Get_Consent_Resp UpdateConsent(int customerId, GetConsent_Req consent_Req);
        public MemberFromLastCampaign GetMemberLastCampaign(string mobileNo);
    }

    public partial class Customer_Internal_Service : Respository_Base<DuchmillModel.CRM_Customer>, ICustomer_Service
    {
        private readonly Line_Repository lineRepository;
        private readonly IBCRM_CRM_Customer_Service crmCustomerService;
        private readonly IBCRM_Storage_Service storageService;
        private readonly IMapper mapper;
        private readonly IBCRM_Wallet_Service walletService;
        private readonly IBCRM_Tag_Service tag_Service;
        private readonly IBCRM_Customer_Repository bcrmCustomerRepository;
        private readonly Wallet_Repository walletRepository;
        private readonly Address_Repository addressRepository;

        public Customer_Internal_Service(DuchmillModel.BCRM_36_Entities dbContext,
                                              Line_Repository lineRepository,
                                              IBCRM_CRM_Customer_Service crmCustomerService,
                                              IBCRM_Storage_Service storageService,
                                              IMapper mapper,
                                              IBCRM_Wallet_Service walletService,
                                              IBCRM_Tag_Service tag_Service,
                                              IBCRM_Customer_Repository bcrmCustomerRepository,
                                              Wallet_Repository walletRepository,
                                              Address_Repository addressRepository) : base(dbContext)
        {
            this.lineRepository = lineRepository;
            this.crmCustomerService = crmCustomerService;
            this.storageService = storageService;
            this.mapper = mapper;
            this.walletService = walletService;
            this.tag_Service = tag_Service;
            this.bcrmCustomerRepository = bcrmCustomerRepository;
            this.walletRepository = walletRepository;
            this.addressRepository = addressRepository;

            TxTimeStamp = DateTime.Now;
        }

        public async Task<Register_Resp> Register(RegisterModel registerData)
        {
            try
            {
                #region Set Identity Context 

                walletRepository.SetIdentityContext( apiRequestId: this.ApiRequestId, appIdentityContext: this.AppIdentityContext, userIdentityContext: this.UserIdentityContext);
                #endregion

                #region Query And Validate Data 
                var cusInSys = Query(it => it.MobileNo == registerData.MobileNo).Select(it => "User has exist in system.").FirstOrDefault();
                if (!string.IsNullOrEmpty(cusInSys)) throw new Exception("Customer already has in system.");

                var lineInfo = lineRepository.Query(it => it.Line_UserId == registerData.Line_UserId).FirstOrDefault();
                if (lineInfo == null) throw new Exception("Customer data not found.");
                #endregion

                #region Create Customer Data
                string customerRef = Guid.NewGuid().ToString();
                List<string> brandActive = new List<string>();
                brandActive.Add(App_Setting.Brands.Main.Config.Name);

                //if (App_Setting.Brands.YuzuGroup.Config.Name != this.CurrentBrand) brandActive.Add(this.CurrentBrand);

                CRM_Customer_Regis_Channel regisChannel = new CRM_Customer_Regis_Channel
                {
                    Registered_Channel_Flag = BCRM.CRM.Constants.BCRM_CRM_Const.Customer.Regis_Channel.Others,
                    Registered_Channel = "external"
                };

                var customerData = mapper.Map<CRM_Customer_DMG_Info>(registerData);

                var customerInfo = await crmCustomerService.Create(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref, lineInfo.Identity_SRef, customerRef, regisChannel, DMG_Info: customerData);

                lineInfo.CRM_CustomerId = customerInfo.CRM_CustomerId;
                lineRepository.Update(lineInfo);

                var bcrmInfo = new UpdateBCRMInfo_Req()
                {
                    AccountId = lineInfo.AccountId,
                    Line_UserId = lineInfo.Line_UserId,
                    Picture_Url = lineInfo.Picture_Url,
                    CRM_Wallet_Alt_Ref = customerInfo.CRM_Wallet_Alt_Ref,
                    Name = lineInfo.Name,
                    Accept_Activity_Consent = registerData.Accept_Activity_Consent,
                    Accept_Privacy_Policy = registerData.Accept_Privacy_Policy
                };

                bcrmCustomerRepository.CreateBCRMInfo(CRM_CustomerId: customerInfo.CRM_CustomerId, bcrmInfo: bcrmInfo);
                #endregion

                #region Sync Address 
                // add address
                var address = mapper.Map<AddressModel>(registerData);
                address.CRM_CustomerId = customerInfo.CRM_CustomerId;
                address.IsDefault = true;
                addressRepository.SyncAddress(address);

                // add shipping address
                var shippingAddress = mapper.Map<AddressModel>(registerData);
                shippingAddress.CRM_CustomerId = customerInfo.CRM_CustomerId;
                shippingAddress.IsDefault = true;
                addressRepository.SyncShippingAddress(shippingAddress);
                #endregion

                //var welcomePoints = await EarnWelcomePoint(customerId: customerInfo.CRM_CustomerId);

                Register_Resp registerResponse = new Register_Resp()
                {
                    LineInfo = lineInfo,
                    CRM_CustomerInfo = customerInfo,
                    BrandActive = brandActive,
                    //WelcomePoints = welcomePoints
                };

                return registerResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Get_Customer_Info_Resp GetCustomerInfo(string identity_SRef, int? customerId)
        {
            try
            {
                if (string.IsNullOrEmpty(identity_SRef) && customerId == null) throw new Exception("Please specify key.");

                var customerInfo = (from crm in ((DuchmillModel.BCRM_36_Entities)DB).CRM_Customers
                                    join bcrm in ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers on crm.CustomerId equals bcrm.CRM_CustomerId
                                    where identity_SRef == crm.Identity_SRef || customerId == crm.CustomerId
                                    select new Get_Customer_Info_Resp()
                                    {
                                        Identity_SRef = crm.Identity_SRef,
                                        First_Name_Th = crm.First_Name_Th,
                                        Last_Name_Th = crm.Last_Name_Th,
                                        Point_Balance = bcrm.Point_Balance,
                                        ImageProfileUrl = bcrm.ImageProfileUrl,
                                        ImageProfileUrl_Expire = bcrm.ImageProfileUrl_Expire,
                                        Total_Spending = bcrm.Total_Spending
                                    }).FirstOrDefault();

                if (customerInfo == null) throw new Exception("Customer not found.");

                return customerInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CRM_Customer_Address_Resp GetAddress(int? addressId, int? addressType, int? customerId)
        {
            try
            {
                DuchmillModel.CRM_Customer_Address address = new DuchmillModel.CRM_Customer_Address();
                var addressList = addressRepository.Query(it => it.CRM_CustomerId == customerId &&

                                                                it.IsDeleted == false);

                if (addressId != null)
                {
                    address = addressList.Where(it => it.AddressId == addressId).FirstOrDefault();
                }
                else
                {
                    address = addressList.Where(it => it.Addr_Type == addressType &&
                                                      it.IsDefault == true).FirstOrDefault();
                }

                if (address == null) return new CRM_Customer_Address_Resp();
                var address_Resp = mapper.Map<CRM_Customer_Address_Resp>(address);

                if (address.Addr_Type == AppConstants.Customer.Address.Type.Address)
                {
                    var customerInfo = this.Query(it => it.CustomerId == customerId).Select(it => new { it.Gender, it.Email, it.DateOfBirth }).FirstOrDefault();

                    address_Resp.Email = customerInfo.Email;
                    address_Resp.Gender = customerInfo.Gender;
                    address_Resp.DateOfBirth = customerInfo.DateOfBirth;
                }

                return address_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CRM_Customer_Address_Resp> GetAddressList(int? addressType, int? customerId)
        {
            try
            {
                List<DuchmillModel.CRM_Customer_Address> address = new List<DuchmillModel.CRM_Customer_Address>();

                var _address = addressRepository.Query(it => it.CRM_CustomerId == customerId && it.IsDeleted == false);
                if (addressType != null) _address = _address.Where(it => it.Addr_Type == addressType);

                address = _address.ToList();
                var address_Resp = mapper.Map<List<CRM_Customer_Address_Resp>>(address);

                return address_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CRM_Customer_Address_Resp UpdateAddress(int? addressType, AddressModel address)
        {
            try
            {
                if (addressType == AppConstants.Customer.Address.Type.Address)
                {
                    var _address = addressRepository.SyncAddress(address);
                    var address_Resp = mapper.Map<CRM_Customer_Address_Resp>(_address);
                    return address_Resp;
                }
                else
                {
                    var _address = addressRepository.SyncShippingAddress(address);
                    var address_Resp = mapper.Map<CRM_Customer_Address_Resp>(_address);
                    return address_Resp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CRM_Customer_Address_Resp AddAddress(int? addressType, AddAddressModel address)
        {
            try
            {
                if (addressType == AppConstants.Customer.Address.Type.Address)
                {
                    var addr = mapper.Map<AddressModel>(address);
                    var _address = addressRepository.SyncAddress(addr);
                    var address_Resp = mapper.Map<CRM_Customer_Address_Resp>(_address);
                    return address_Resp;
                }
                else
                {
                    var addr = mapper.Map<AddressModel>(address);
                    var _address = addressRepository.SyncShippingAddress(addr);
                    var address_Resp = mapper.Map<CRM_Customer_Address_Resp>(_address);
                    return address_Resp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Edit_Customer_Info_Resp> EditCustomerInfo(EditProfileReq req, string identity_SRef = null, int? customerId = null)
        {
            try
            {
                var customerInfo = (from crm in ((DuchmillModel.BCRM_36_Entities)DB).CRM_Customers
                                    join bcrm in ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers on crm.CustomerId equals bcrm.CRM_CustomerId
                                    where identity_SRef == crm.Identity_SRef || customerId == crm.CustomerId
                                    select new { crm, bcrm }).FirstOrDefault();

                if (customerInfo == null) throw new Exception("Customer not found.");

                if (!string.IsNullOrEmpty(req.First_Name_Th)) customerInfo.crm.First_Name_Th = req.First_Name_Th;
                if (!string.IsNullOrEmpty(req.Last_Name_Th)) customerInfo.crm.Last_Name_Th = req.Last_Name_Th;
                customerInfo.crm.Email = req.Email;
                if (req.Gender != null) customerInfo.crm.Gender = req.Gender.Value;
                if (req.DateOfBirth != null) customerInfo.crm.DateOfBirth = req.DateOfBirth;

                customerInfo.crm.Updated_DT = DateTime.Now;
                customerInfo.bcrm.Update_DT = DateTime.Now;

                ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();

                if (!string.IsNullOrEmpty(req.Province))
                {
                    var addressId = addressRepository.Query(it => it.CRM_CustomerId == customerId && it.Addr_Type == AppConstants.Customer.Address.Type.Address).Select(it => new { it.AddressId }).FirstOrDefault();

                    var _address = mapper.Map<AddressModel>(req);
                    _address.IsDefault = true;
                    if (addressId != null) _address.AddressId = addressId.AddressId;

                    _address.CRM_CustomerId = customerInfo.crm.CustomerId;
                    addressRepository.SyncAddress(_address);
                }

                if (req.ImageProfile != null && req.ImageProfile.Length > 0)
                {
                    string fileName = $"{customerInfo.crm.Reference}.png";
                    string blobPath = $"{AppConstants.Blob.Path.CustomerProfile}/{fileName}";

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        req.ImageProfile.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        // Headers
                        BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders();
                        blobHttpHeaders.ContentType = req.ImageProfile.ContentType;

                        BCRM_Storage_Blob_Data blobData = new BCRM_Storage_Blob_Data
                        {
                            Blob_Path = blobPath,
                            Content = memoryStream,
                            Blob_Headers = blobHttpHeaders
                        };

                        BlobContentInfo resp = await storageService.UploadAsync(App_Setting.Brands.Main.Config.Brand_Ref, "bcrm-data", blobData);
                    }

                    var imageProfile = GetImageProfile(customerInfo.crm.Reference);
                    customerInfo.bcrm.ImageProfileUrl = imageProfile;
                    ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers.Update(customerInfo.bcrm);
                    ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();
                }

                var customer = new Edit_Customer_Info_Resp()
                {
                    First_Name_Th = customerInfo.crm.First_Name_Th,
                    Last_Name_Th = customerInfo.crm.Last_Name_Th,
                    Gender = customerInfo.crm.Gender,
                    MobileNo = customerInfo.crm.MobileNo,
                    Email = customerInfo.crm.Email,
                    DateOfBirth = customerInfo.crm.DateOfBirth,
                    ImageProfileUrl = customerInfo.bcrm.ImageProfileUrl,
                };

                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetImageProfile(string customerReference)
        {
            try
            {
                string fileName = $"{customerReference}.png";

                string blobPath = $"{AppConstants.Blob.Path.CustomerProfile}/{fileName}";

                int day = 30;

                DateTime expireOn = DateTime.Now.AddDays(day);

                BCRM_Storage_Blob_SASInfo sasInfo = new BCRM_Storage_Blob_SASInfo
                {
                    Blob_Path = blobPath,
                    Expire_Span = TimeSpan.FromDays(day)
                };

                var res = storageService.Generate_Blob_Link(App_Setting.Brands.Main.Config.Brand_Ref, "bcrm-data", sasInfo);

                return res.Item1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CRM_Customer_Address_Resp DeleteAddress(int addressId, int customerId)
        {
            try
            {
                var addressDeleted = addressRepository.DeleteAddress(addressId: addressId, customerId: customerId);
                var address_Resp = mapper.Map<CRM_Customer_Address_Resp>(addressDeleted);
                return address_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Point_Balance_Resp GetPointBalance( int? customerId = null)
        {
            try
            {
                Point_Balance_Resp pointBalance = new Point_Balance_Resp();

                var customer = ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers.Where(it => it.CRM_CustomerId == customerId).FirstOrDefault();

                if (customer == null) throw new Exception("Customer not found.");

                var wallet = walletRepository.GetWallet(customerId: customer.CRM_CustomerId, walletId: customer.Wallet_Id, wallet_Alt_Ref: customer.Wallet_Alt_Ref);

                customer.Point_Balance = wallet.Balance;
                ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers.Update(customer);
                ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();

                pointBalance.PointBalance = (int)wallet.Balance;
                pointBalance.Total_Spending = customer.Total_Spending != null ? customer.Total_Spending.Value : 0;

                return pointBalance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Point_History_Resp> GetPointHistory(DateTime? start = null, DateTime? end = null, string identity_SRef = null, int? customerId = null)
        {
            try
            {

                var pointHistory = (from doc in ((DuchmillModel.BCRM_36_Entities)DB).Document_Documents
                                    join doc_ref in ((DuchmillModel.BCRM_36_Entities)DB).Document_Tx_Refs on doc.DocumentId equals doc_ref.DocumentId
                                    where doc.Identity_SRef == identity_SRef && doc.IsDeleted == false
                                    select new Point_History_Resp()
                                    {
                                        Point = doc_ref.Point,
                                        BillingNo = doc_ref.BillingNo,
                                        CreatedTime = doc.CreatedTime,
                                        Status = doc.Status,
                                        Store = doc.Remark,
                                        RejectMessage = doc_ref.RejectMessage,
                                    }).ToList();

                if (pointHistory == null) pointHistory = new List<Point_History_Resp>();

                pointHistory = pointHistory.OrderByDescending(it => it.CreatedTime).ToList();

                return pointHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Redemption_History_Resp GetRedemptionHistory(int? rewardType = null, DateTime? start = null, DateTime? end = null, string identity_SRef = null, int? customerId = null)
        {
            try
            {
                List<RewardRedemption> redemptionHistories = new List<RewardRedemption>();

                var _redemptionHistories = from pv_tx in ((DuchmillModel.BCRM_36_Entities)DB).CRM_Privilege_Transactions
                                           join pv in ((DuchmillModel.BCRM_36_Entities)DB).CRM_Privileges on (int)pv_tx.PrivilegeId equals pv.PrivilegeId
                                           join code in ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customer_Coupon_Codes on (int)pv_tx.TransactionId equals code.Privilege_Tx_Id
                                                into codes
                                           from code in codes.DefaultIfEmpty()
                                           where pv_tx.CRM_CustomerId == customerId
                                                 && (start == null || end == null || (pv_tx.TX_Time.Date >= start.Value.Date && pv_tx.TX_Time.Date <= end.Value.Date))
                                           select new RewardRedemption()
                                           {
                                               //TransactionId = pv_tx.TransactionId,
                                               RedemptionRef = pv_tx.TXReference,
                                               CouponCodeRef = code != null ? code.Reference : null,
                                               Name_Th = pv.Name_Th,
                                               Name_En = pv.Name_En,
                                               SubName_Th = pv.SubName_Th,
                                               SubName_En = pv.SubName_En,
                                               Type = pv.Type,
                                               PrivilegeId = pv.PrivilegeId,
                                               Privilege_Image_Url = pv.Privilege_Image_Url,
                                               Point_Ref = pv_tx.Point_PerAmount,
                                               PointUsed = pv_tx.Point_Total,
                                               ITF_Ref = pv_tx.ITF_Ref,
                                               ITF_Ref_2 = pv_tx.ITF_Ref_2,
                                               Remark = pv_tx.Remark,
                                               UpdatedTime = pv_tx.Updated_DT,
                                               CreatedTime = pv_tx.TX_Time
                                           }; 

                switch (rewardType)
                {
                    case Pv_Tx_Ref.All:
                        redemptionHistories = _redemptionHistories != null ? _redemptionHistories.ToList() : new List<RewardRedemption>();
                        break;

                    case Pv_Tx_Ref.PhysicalReward:
                        _redemptionHistories = _redemptionHistories.Where(it => it.Type == BCRM_PV_Const.Privilege.Type.Reward_Privilege);

                        redemptionHistories = _redemptionHistories != null ? _redemptionHistories.ToList() : new List<RewardRedemption>();
                        break;
                    case Pv_Tx_Ref.CouponCode:
                        _redemptionHistories = _redemptionHistories.Where(it => it.Type == BCRM_PV_Const.Privilege.Type.Coupon
                                                                             || it.Type == BCRM_PV_Const.Privilege.Type.Coupon_Code
                                                                             || it.Type == BCRM_PV_Const.Privilege.Type.Unlimited_Coupon);

                        redemptionHistories = _redemptionHistories != null ? _redemptionHistories.ToList() : new List<RewardRedemption>();

                        redemptionHistories = redemptionHistories.Where(it => it.Code_IsExpired == false).ToList();

                        break;
                    case Pv_Tx_Ref.CouponCodeExpired:
                        redemptionHistories = _redemptionHistories.Where(it => (it.Type == BCRM_PV_Const.Privilege.Type.Coupon
                                                                             || it.Type == BCRM_PV_Const.Privilege.Type.Coupon_Code
                                                                             || it.Type == BCRM_PV_Const.Privilege.Type.Unlimited_Coupon)).ToList();

                        redemptionHistories = _redemptionHistories != null ? _redemptionHistories.ToList() : new List<RewardRedemption>();

                        redemptionHistories = redemptionHistories.Where(it => it.Code_IsExpired == true).ToList();

                        break;
                    default:
                        break;
                }

                redemptionHistories = redemptionHistories.OrderByDescending(it => it.CreatedTime).ToList();

                if (redemptionHistories != null)
                {
                    foreach (var pv in redemptionHistories)
                    {
                        var subImage = (from img_G in ((DuchmillModel.BCRM_36_Entities)DB).CRM_Privilege_Group_Images
                                        join img in ((DuchmillModel.BCRM_36_Entities)DB).CRM_Privilege_Images on img_G.Group_ImageId equals img.Group_ImageId
                                        where img_G.PrivilegeId == pv.PrivilegeId && img_G.Key == "sub-img"
                                        select new { img.Image_Url }).FirstOrDefault();

                        if (subImage != null)
                        {
                            pv.Privilege_Sub_Image_Url = subImage.Image_Url;
                        }
                    }
                }

                Redemption_History_Resp redemptionHistory_Resp = new Redemption_History_Resp()
                {
                    RedemptionHistories = redemptionHistories
                };

                return redemptionHistory_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Show_Coupon_Code_Resp ShowCouponCode(Show_Coupon_Code_Req req, int? customerId = null)
        {
            try
            {
                Guid codeRef;
                Guid txRef;

                if (req.RedemptionRef == null) throw new Exception("Invalid request format");

                if (!Guid.TryParse(req.CouponCodeRef, out codeRef)) throw new Exception("Invalid request format.");

                var couponCode = ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customer_Coupon_Codes.FirstOrDefault(it => it.Reference == codeRef && it.Privilege_Tx_Ref == req.RedemptionRef && it.CRM_CustomerId == customerId);

                if (couponCode == null) throw new Exception("Coupon code not found.");
                if (couponCode.Status == AppConstants.Privilege.Status.InActive) throw new Exception("Coupon code not found.");
                //if (couponCode.Status == AppConstants.Privilege.Status.Expired) throw new Exception("Coupon code already expired.");

                DateTime? absoluteExpTime;
                DateTime? slideExpTime = null;
                DateTime? createTime;
                int? expWithinSec;

                absoluteExpTime = couponCode.Absolute_Exp_DT;
                createTime = couponCode.Create_DT;
                expWithinSec = couponCode.Exp_Time_Within_Sec;

                if (couponCode.ForPOS != true)
                {
                    if ((couponCode.Expired_DT.HasValue && TxTimeStamp > couponCode.Expired_DT))
                    {
                        couponCode.Status = AppConstants.Privilege.Status.Expired;
                        couponCode.Status_Desc = AppConstants.Privilege.Status.ExpiredDesc;
                        couponCode.Update_DT = TxTimeStamp;
                        ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();
                    }
                    else if (couponCode.Status == AppConstants.Privilege.Status.Active)
                    {
                        couponCode.Use_DT = TxTimeStamp;
                        couponCode.Status = AppConstants.Privilege.Status.Used;
                        couponCode.Status_Desc = AppConstants.Privilege.Status.UsedDesc;
                        couponCode.Update_DT = TxTimeStamp;
                        ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();
                    }
                    else
                    {
                        couponCode.Status = AppConstants.Privilege.Status.Used;
                        couponCode.Status_Desc = AppConstants.Privilege.Status.UsedDesc;
                        couponCode.Update_DT = TxTimeStamp;
                        ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();
                    }
                }
                else
                {
                    if ((couponCode.Expired_DT.HasValue && TxTimeStamp > couponCode.Expired_DT))
                    {
                        couponCode.Status = AppConstants.Privilege.Status.Expired;
                        couponCode.Status_Desc = AppConstants.Privilege.Status.ExpiredDesc;
                        couponCode.Update_DT = TxTimeStamp;
                        ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();
                    }
                    else
                    {
                        couponCode.Update_DT = TxTimeStamp;
                        ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();
                    }
                }

                TimeSpan? remainTime = null;

                if (couponCode.Expired_DT.HasValue) remainTime = couponCode.Expired_DT - TxTimeStamp;

                CouponCode_Resp couponCodeResp = new CouponCode_Resp()
                {
                    CouponCode = couponCode.CouponCode,
                    ExpiredTime = couponCode.Expired_DT,
                    ExpiredWithinSec = remainTime != null ? remainTime.Value.TotalSeconds : 0
                };

                Show_Coupon_Code_Resp show_Coupon_Code_Resp = new Show_Coupon_Code_Resp()
                {
                    CodeInfo = couponCodeResp
                };

                return show_Coupon_Code_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BCRM_Tag_Mapping_Info> GetCustomerTags(int customerId)
        {
            try
            {
                tag_Service.SetIdentity(AppIdentityContext);
                List<BCRM_Tag_Mapping_Info> tag_List = tag_Service.Tags(Brand_Ref: this.BrandRef,
                                                                        Content_Type: BCRM.Data.Constants.BCRM_Data_Const.Tag.Mapping.Type.CRM_Customer,
                                                                        ContentId: customerId);
                return tag_List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Get_Consent_Resp GetConsent(int customerId)
        {
            try
            {
                var consent = ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers.Where(it => it.CRM_CustomerId == customerId)
                                                                                  .Select(it => new Get_Consent_Resp
                                                                                  {
                                                                                      Accept_Privacy_Policy = it.Accept_Privacy_Policy,
                                                                                      Accept_Activity_Consent = it.Accept_Activity_Consent,
                                                                                  })
                                                                                   .FirstOrDefault();
                return consent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Get_Consent_Resp UpdateConsent(int customerId, GetConsent_Req consent_Req)
        {
            try
            {
                var bcrmInfo = ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers.FirstOrDefault(it => it.CRM_CustomerId == customerId);
                if (bcrmInfo == null) throw new Exception("Customer not found.");

                if (consent_Req.Accept_Activity_Consent != null) bcrmInfo.Accept_Activity_Consent = consent_Req.Accept_Activity_Consent;
                if (consent_Req.Accept_Privacy_Policy != null) bcrmInfo.Accept_Privacy_Policy = consent_Req.Accept_Privacy_Policy;

                ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();

                Get_Consent_Resp consent_Resp = new Get_Consent_Resp()
                {
                    Accept_Privacy_Policy = consent_Req.Accept_Privacy_Policy,
                    Accept_Activity_Consent = consent_Req.Accept_Activity_Consent
                };

                return consent_Resp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<EarnWelcomePoint_Resp>> EarnWelcomePoint(int customerId)
        {
            try
            {
                List<EarnWelcomePoint_Resp> earnWelcomePoints = new List<EarnWelcomePoint_Resp>();

                var bcrmInfo = ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Customers.FirstOrDefault(it => it.CRM_CustomerId == customerId);
                if (bcrmInfo == null) throw new Exception("Customer not found.");

                List<EarnWelcomePoint> earnInfos = new List<EarnWelcomePoint>();
                List<WL_Wallet_TX_Result> earnPointResponse = new List<WL_Wallet_TX_Result>();

                #region Build Earn Point Infos

                string earnRef = $"WEL-{bcrmInfo.CRM_CustomerId}";

                EarnWelcomePoint earnWelcomePoint = new EarnWelcomePoint()
                {
                    Wallet_Alt_Ref = bcrmInfo.Wallet_Alt_Ref,
                    BrandName = App_Setting.Brands.Main.Config.Name,
                    PointEarn = App_Setting.Brands.Main.Config.Welcome_Point,
                    TransactionId = earnRef
                };

                earnInfos.Add(earnWelcomePoint);

                #endregion

                #region Earn Point
                try
                {
                    List<Task> tasks = new List<Task>();
                    foreach (var earnPointInfo in earnInfos)
                    {
                        if (earnPointInfo.PointEarn <= 0) continue;
                        var earnTask = Add_Funds(earnInfo: earnPointInfo);
                        tasks.Add(earnTask);
                    }

                    await Task.Factory.ContinueWhenAll(tasks.ToArray(), async (earns) =>
                    {
                        var wallets = ((DuchmillModel.BCRM_36_Entities)DB).WL_Wallets.Where(it => it.CRM_CustomerId == customerId).Select(it => new { it.Extra_Ref_2, it.Balance }).ToList();

                        if (wallets != null)
                        {
                            foreach (var wl in wallets)
                            {
                                bcrmInfo.Point_Balance = wl.Balance;

                                EarnWelcomePoint_Resp earnWelcomePoint = new EarnWelcomePoint_Resp()
                                {
                                    WelcomePoint = (int)wl.Balance,
                                    BrandName = wl.Extra_Ref_2
                                };

                                if (earnWelcomePoint.WelcomePoint > 0) earnWelcomePoints.Add(earnWelcomePoint);
                            }

                            ((DuchmillModel.BCRM_36_Entities)DB).Update(bcrmInfo);
                            ((DuchmillModel.BCRM_36_Entities)DB).SaveChanges();
                        }

                    });

                }
                catch (Exception ex)
                {
                    // log
                }
                //tasks.
                #endregion


                return earnWelcomePoints;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<WL_Wallet_TX_Result> Add_Funds(EarnWelcomePoint earnInfo)
        {
            try
            {

                WL_Add_Funds_Info info = new WL_Add_Funds_Info
                {
                    Reference = "Welcome Point",
                    Extra_Ref = earnInfo.BrandName,
                    Extra_Ref_2 = earnInfo.PointEarn.ToString(),
                };

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "caller", "crm"}
                };

                WL_Wallet_TX_Result walletResult = await walletService.Add_Funds_v2(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref,
                                                                                    Wallet_Ref: earnInfo.Wallet_Alt_Ref,
                                                                                    TransactionId: earnInfo.TransactionId.ToString(),
                                                                                    Amount: earnInfo.PointEarn,
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


                return walletResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MemberFromLastCampaign GetMemberLastCampaign(string mobileNo)
        {
            try
            {
                var memberFromV2 =  ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Dutchmill_Members.FirstOrDefault(it => it.MobileNo == mobileNo);
                if (memberFromV2 != null)
                {
                    var memberFromLastCampaign = mapper.Map<MemberFromLastCampaign>(memberFromV2);
                    return memberFromLastCampaign;
                }
                else
                {
                    var memberFromV1 = ((DuchmillModel.BCRM_36_Entities)DB).BCRM_Dutchmill_Member_V1s.FirstOrDefault(it => it.MobileNo == mobileNo);
                    if (memberFromV1 == null) return null;

                    var memberFromLastCampaign = mapper.Map<MemberFromLastCampaign>(memberFromV1);
                    return memberFromLastCampaign;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
