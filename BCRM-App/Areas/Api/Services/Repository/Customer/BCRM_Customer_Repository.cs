using System;
using System.Linq;
using BCRM_App.Services.RemoteInternal.Repository;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Configs;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM_App.Areas.Api.Services.Repository.Wallet;
using Microsoft.EntityFrameworkCore;
using BCRM_App.Areas.Api.Services.Repository.Customer.Models;
using BCRM_App.Constants;
using BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Areas.Api.Services.Repository.Customer
{

    public interface IBCRM_Customer_Repository
    {
        public void CreateBCRMInfo(int? CRM_CustomerId, UpdateBCRMInfo_Req bcrmInfo);
        public void UpdateBCRMInfo(int? CRM_CustomerId, UpdateBCRMInfo_Req bcrmInfo);
    }


    public class BCRM_Customer_Repository : Respository_Base<DuchmillModel.BCRM_Customer>, IBCRM_Customer_Repository
    {
        public BCRM_Customer_Repository(DuchmillModel.BCRM_36_Entities dbContext) : base(dbContext)
        {
            TxTimeStamp = DateTime.Now;
        }

        public void CreateBCRMInfo(int? CRM_CustomerId, UpdateBCRMInfo_Req customerInfo)
        {
            try
            {
                var bcrmCustomerInfo = Query(it => it.CRM_CustomerId == CRM_CustomerId).FirstOrDefault();
                if (bcrmCustomerInfo == null)
                {
                    DuchmillModel.BCRM_Customer bcrmInfo = new BCRM_Customer()
                    {
                        CRM_CustomerId = CRM_CustomerId.Value,
                        LineInfoId = customerInfo.AccountId,
                        Line_UserId = customerInfo.Line_UserId,
                        ImageProfileUrl = customerInfo.Picture_Url,
                        Accept_Activity_Consent = customerInfo.Accept_Activity_Consent,
                        Accept_Privacy_Policy = customerInfo.Accept_Privacy_Policy,
                        Wallet_Alt_Ref = customerInfo.CRM_Wallet_Alt_Ref,
                    };

                    Add(bcrmInfo);
                }
                else
                {
                    UpdateBCRMInfo(CRM_CustomerId: CRM_CustomerId, lineInfo: customerInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBCRMInfo(int? CRM_CustomerId, UpdateBCRMInfo_Req lineInfo)
        {
            try
            {
                var bcrmCustomerInfo = Query(it => it.CRM_CustomerId == CRM_CustomerId).FirstOrDefault();
                if (bcrmCustomerInfo != null)
                {
                    bcrmCustomerInfo.LineInfoId = lineInfo.AccountId;
                    bcrmCustomerInfo.Line_UserId = lineInfo.Line_UserId;
                    bcrmCustomerInfo.ImageProfileUrl = lineInfo.Picture_Url;

                    Update(bcrmCustomerInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
