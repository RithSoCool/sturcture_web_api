using BCRM.Common.Services.Document;
using Org.BouncyCastle.Ocsp;
using static BCRM.Common.Services.Document.BCRM_Document_Service;
using System.Collections.Generic;
using BCRM.Common.Helpers;
using Microsoft.Identity.Client;
using static BCRM.Common.Constants.PDPA.BCRM_PDPA_Const.Article;
using Microsoft.AspNetCore.Http;
using BCRM_App.Configs;
using BCRM_App.Areas.Api.Services.Document.Models;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Models.DBModels.Duchmill;
using System;
using System.Linq;
using BCRM_App.Extentions;
using BCRM_App.Areas.Api.Services.Repository.Wallet;
using BCRM.Common.Models.DBModel.CRM;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models;
using System.Threading.Tasks;
using static BCRM_App.Areas.Api.Services.Repository.Wallet.Wallet_Repository;
using BCRM.Common.Services.Wallet;
using BCRM.Common.Services.CRM;
using BCRM.Common.Context;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage;

namespace BCRM_App.Areas.Api.Services.Document
{
    public interface IDocument_Internal_Service
    {
        public Upload_Resp Upload(int customerId, string identity_SRef, Upload_Req uploadInfo);
        public Task<Callback_Resp> Callback(Callback_Payload callbackInfo);
    }

    public class Document_Internal_Service : Service_Base, IDocument_Internal_Service
    {
        private readonly IBCRM_Document_Service document_Service;
        public readonly Wallet_Repository wallet_Repository;

        public Document_Internal_Service(IBCRM_Document_Service document_Service,
                                         Wallet_Repository wallet_Repository)
        {
            this.document_Service = document_Service;
            this.wallet_Repository = wallet_Repository;
        }

        public virtual Task<Callback_Resp> Callback(Callback_Payload callbackInfo)
        {
            try
            {
                throw new System.NotImplementedException();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public virtual Upload_Resp Upload(int customerId, string identity_SRef, Upload_Req uploadInfo)
        {
            try
            {
                List<Req_Document_Upload> uploadReceiptResps = new List<Req_Document_Upload>();

                foreach (var image in uploadInfo.Images)
                {
                    if (image == null) continue;

                    string randomStr = StringHelper.Instance.RandomString(5);
                    int running = 100000 + customerId;

                    string month = TxTimeStamp.Month < 10 ? $"0{TxTimeStamp.Month}" : TxTimeStamp.Month.ToString();
                    string day = TxTimeStamp.Day < 10 ? $"0{TxTimeStamp.Day}" : TxTimeStamp.Day.ToString();

                    string transactionRef = $"D{running.ToString().Substring(1)}-{month}{day}-{randomStr}";

                    var docDate = Convert.ToDateTime(uploadInfo.BillingDate);

                    Req_Document_Upload document = new Req_Document_Upload()
                    {
                        Attachments = new List<IFormFile>() { image },
                        Container_Ref = App_Setting.Brands.Main.Config.Document_Alt_Reference,
                        PointOfPurchase = uploadInfo.Store,
                        Identity_SRef = identity_SRef,
                        Name = $"{uploadInfo.First_Name_Th} {uploadInfo.Last_Name_Th}",
                        MobileNo = uploadInfo.MobileNo,
                        DocumentDate = docDate,
                        DocumentNo = transactionRef,
                        Remark = $"Store: {uploadInfo.Store}",
                    };

                    Wrap_Document_Model uploadReceiptResp = document_Service.Upload_Document(Brand_Ref: App_Setting.Brands.Main.Config.Brand_Ref, req: document);

                    uploadReceiptResps.Add(document);
                }

                Upload_Resp upload_Resp = new Upload_Resp()
                {
                    Upload_Resps = uploadReceiptResps
                };

                return upload_Resp;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Brand_Document_Service : Document_Internal_Service, IDocument_Internal_Service
    {
        private readonly BCRM_36_Entities entities;
        private readonly ICustomer_Service customerService;
        private readonly FlexMessageBuilder flexMessageBuilder;
        private readonly ILine_FlexMessage_Client_Service flex_Message_Client_Service;

        public Brand_Document_Service(IBCRM_Document_Service document_Service,
                                      DuchmillModel.BCRM_36_Entities dbContext,
                                      ICustomer_Service customerService,
                                      FlexMessageBuilder flexMessageBuilder,
                                      ILine_FlexMessage_Client_Service flex_Message_Client_Service,
                                      Wallet_Repository wallet_Repository) : base(document_Service, wallet_Repository)
        {
            this.entities = dbContext;
            this.customerService = customerService;
            this.flexMessageBuilder = flexMessageBuilder;
            this.flex_Message_Client_Service = flex_Message_Client_Service;
        }

        public override Upload_Resp Upload(int customerId, string identity_SRef, Upload_Req uploadInfo)
        {

            try
            {
                var customer = (from crm in entities.CRM_Customers
                                where crm.CustomerId == customerId
                                select crm).FirstOrDefault();

                if (customer == null) throw new System.Exception("Customer not found.");

                uploadInfo.First_Name_Th = customer.First_Name_Th;
                uploadInfo.Last_Name_Th = customer.Last_Name_Th;
                uploadInfo.MobileNo = customer.MobileNo;

                var uploadResp = base.Upload(customerId, identity_SRef, uploadInfo);

                foreach (var resp in uploadResp.Upload_Resps)
                {

                    var _document = (from doc in entities.Document_Documents
                                     where doc.DocumentNo == resp.DocumentNo
                                     select doc).FirstOrDefault();

                    Document_Tx_Ref doc_Ref = new Document_Tx_Ref()
                    {
                        DocumentId = (int)_document.DocumentId,
                        CreateTime = TxTimeStamp,
                        Point = 0,
                        Point_Left = 0,
                        BillingNo = uploadInfo.BillingNo,
                        Spending = 0,
                    };

                    entities.Document_Tx_Refs.Add(doc_Ref);
                    entities.SaveChanges();

                    Document_DN_Form_Value billingValue = new Document_DN_Form_Value()
                    {
                        ContainerId = 1,
                        DocumentId = (int)_document.DocumentId,
                        FieldId = 2,
                        FormId = 1,
                        Seq = 1,
                        DataType = 1,
                        Key = "BillingNo",
                        Value = uploadInfo.BillingNo,
                        CreatedTime = TxTimeStamp
                    };

                    entities.Document_DN_Form_Values.Add(billingValue);
                    entities.SaveChanges();
                }

                return uploadResp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task<Callback_Resp> Callback(Callback_Payload req)
        {
            try
            {
                Callback_Resp callback_Resp = new Callback_Resp();

                var document = (from doc in entities.Document_Documents
                                join doc_ref in entities.Document_Tx_Refs on doc.DocumentId equals doc_ref.DocumentId
                                where doc.DocumentRef.ToString() == req.DocumentRef && doc.Identity_SRef == req.Identity_SRef
                                select new { doc, doc_ref }).FirstOrDefault();

                if (document == null) throw new Exception("Reciept not found.");

                var customer = (from crm in entities.CRM_Customers
                                join bcrm in entities.BCRM_Customers on crm.CustomerId equals bcrm.CRM_CustomerId
                                where crm.Identity_SRef == req.Identity_SRef
                                select new { crm.Identity_SRef, crm.CRM_Wallet_Alt_Ref, crm.CustomerId, bcrm.Line_UserId }).FirstOrDefault();

                if (customer == null) throw new Exception("Customer not found.");

                decimal spending;

                decimal.TryParse(req.Spending, out spending);

                Document_Status_Log log = new Document_Status_Log()
                {
                    CRM_CustomerId = customer.CustomerId,
                    DocumentId = (int)document.doc.DocumentId,
                    DocumentRef = document.doc.DocumentNo,
                    BillingNo = req.BillingNo,
                    Status = req.Status,
                    Spending = spending,
                    CreateTime = TxTimeStamp
                };

                entities.Document_Status_Logs.Add(log);
                entities.SaveChanges();

                switch (req.Status)
                {
                    case BCRM.Document.Constants.BCRM_Document_Const.Document.Status.Completed:

                        if (string.IsNullOrWhiteSpace(req.BillingNo))
                        {
                            document.doc.Remark_2 = "!!! SystemMessage: กรุณากรอก BillingNo !!!";
                            entities.SaveChanges();

                            throw new Exception("BillingNo can't be null.");
                        }

                        if (!decimal.TryParse(req.Spending, out spending))
                        {
                            document.doc.Remark_2 = "!!! SystemMessage: กรุณากรอก Spending !!!";
                            entities.SaveChanges();

                            throw new Exception("Spending can't be null.");
                        }

                        //var pvTransaction = entities.CRM_Point_Transactions.FirstOrDefault(it => it.TXReference.ToString() == document.doc_ref.Tx_Referance);

                        //if (pvTransaction != null)
                        //{
                        //    if (pvTransaction.TX_Type == BCRM.CRM.Constants.BCRM_CRM_Const.Point.Transaction.TX_Type.Issue)
                        //    {
                        //        await VoidTransaction(doc: document.doc, doc_ref: document.doc_ref, customer: customer, req: req, log: log);
                        //    }
                        //}

                        var earnPointResp = await EarnPoint(doc: document.doc, doc_ref: document.doc_ref, customer: customer, req: req, log: log);
                        callback_Resp.Transaction = earnPointResp;
                        break;
                    case BCRM.Document.Constants.BCRM_Document_Const.Document.Status.Rejected:

                        var voidPointResp = await VoidTransaction(doc: document.doc, doc_ref: document.doc_ref, customer: customer, req: req, log: log);
                        callback_Resp.Transaction = voidPointResp;
                        break;
                    default:

                        break;
                }

                customerService.GetPointBalance(customerId: customer.CustomerId);

                //var callbackResp = base.Callback(req);

                return callback_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<CRM_Point_TX_Result> EarnPoint(Document_Document doc, Document_Tx_Ref doc_ref, dynamic customer, Callback_Payload req, Document_Status_Log log)
        {
            try
            {
                int stack = doc_ref.Stack != null ? doc_ref.Stack.Value : 0;
                string docRefStack = string.Empty;
                stack = stack + 1;
                docRefStack = stack == 0 ? doc.DocumentRef.ToString() : $"{doc.DocumentRef.ToString()}_{stack}";

                Transaction earnPoint_Req = new Transaction()
                {
                    TransactionId = docRefStack,
                    Transaction_Ref = doc.DocumentNo,
                    Transaction_Extra_Ref = req.Spending,
                    Point = Convert.ToInt32(req.Spending)
                };

                wallet_Repository.SetIdentityContext(apiRequestId: this.ApiRequestId, userIdentityContext: UserIdentityContext, appIdentityContext: AppIdentityContext);
                var earnPointTask = wallet_Repository.EarnPoint_V2(customerId: customer.CustomerId,
                                                                wallet_Alt_Ref: customer.CRM_Wallet_Alt_Ref,
                                                                earnPoint: earnPoint_Req);
                var earnPointTansaction = await earnPointTask as CRM_Point_TX_Result;

                if (earnPointTansaction.Is_Duplicate_TX) throw new Exception("ไม่สามารถทำรายการซ้ำได้");

                doc_ref.Tx_Id = (int)earnPointTansaction.Transaction.WL_LedgerId;
                doc_ref.Tx_Referance = earnPointTansaction.TXReference.ToString();
                doc_ref.Point = (int)earnPointTansaction.Transaction.Point;
                doc_ref.Spending = Convert.ToDecimal(req.Spending);
                doc_ref.BillingNo = req.BillingNo;
                doc_ref.Stack = stack;
                doc_ref.RejectMessage = string.Empty;
                doc_ref.DocumentRef_Stack = docRefStack;

                log.Stack = stack;
                log.DocumentRef_Stack = docRefStack;
                log.Remark = string.Empty;
                log.Point = doc_ref.Point;
                log.IsEarnPoint = true;

                entities.SaveChanges();

                EarnPointMessage earnPoint = new EarnPointMessage()
                {
                    Line_UserId = customer.Line_UserId,
                    Point = (int)earnPointTansaction.Transaction.Point
                };

                var approveFlexMessage = flexMessageBuilder.Build_Flex_Earn_Point(earnPoint);
                if (approveFlexMessage != null)
                {
                    await flex_Message_Client_Service.SendFlexMessage(approveFlexMessage);
                }

                int cumulative_Spending_Rate = 1200;

                if (earnPointTansaction.Transaction.Balance >= cumulative_Spending_Rate)
                {
                    CumulativeSpendingMessage cumulative = new CumulativeSpendingMessage()
                    {
                        Line_UserId = customer.Line_UserId,
                        Point = (int)earnPointTansaction.Transaction.Point
                    };

                    var cumulativeFlexMessage = flexMessageBuilder.Build_Flex_Cumulative_Spending(cumulative);

                    await flex_Message_Client_Service.SendFlexMessage(cumulativeFlexMessage);
                }

                return earnPointTansaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<CRM_Point_TX_Result> VoidTransaction(Document_Document doc, Document_Tx_Ref doc_ref, dynamic customer, Callback_Payload req, Document_Status_Log log)
        {
            try
            {
                CRM_Point_TX_Result voidTransaction_Resp = null;

                if (doc_ref.Tx_Referance != null)
                {
                    try
                    {
                        Transaction transaction = new Transaction()
                        {
                            TransactionId = doc_ref.Tx_Referance,
                            Transaction_Ref = doc.DocumentNo,
                            Transaction_Extra_Ref = req.Spending,
                            Point = Convert.ToInt32(req.Spending)
                        };

                        wallet_Repository.SetIdentityContext(apiRequestId: this.ApiRequestId, userIdentityContext: UserIdentityContext, appIdentityContext: AppIdentityContext);
                        var voidPointTask = wallet_Repository.VoidPoint_V2(customerId: customer.CustomerId,
                                                                        wallet_Alt_Ref: customer.CRM_Wallet_Alt_Ref,
                                                                        transaction: transaction);
                        var voidPointTansaction = await voidPointTask as CRM_Point_TX_Result;
                        voidTransaction_Resp = voidPointTansaction;

                        var updateBalanceTask = await wallet_Repository.UpdateBalance_V2(customerId: customer.CustomerId,
                                                                                         wallet_Alt_Ref: customer.CRM_Wallet_Alt_Ref);

                        if (voidPointTansaction.Is_Duplicate_TX) throw new Exception("ไม่สามารถทำรายการซ้ำได้");

                        doc_ref.Tx_Id = (int)voidPointTansaction.Transaction.WL_LedgerId;
                        doc_ref.Tx_Referance = voidPointTansaction.TXReference.ToString();
                    }
                    catch (Exception ex)
                    {
                        var errMessage = new
                        {
                            ex.Message,
                            ex.StackTrace,
                            EventTime = TxTimeStamp,
                            doc.DocumentRef,
                            customer.Identity_SRef
                        };

                        log.Remark = ex.Message;

                        if (ex.Message == "transaction already redeemed") throw new Exception(ex.Message);

                        //_logger.LogError("Void trans has error", errMessage);
                    }
                    finally
                    {

                        if (log.Remark == "transaction already redeemed")
                        {
                            doc.Remark_2 = "!!! SystemMessage: ไม่สามารถ Void Transaction ได้, เนื่องจากแต้มจาก Transaction นี้ ได้ถูกผู้ใชใข้งานไปแล้ว !!!";
                            entities.SaveChanges();
                        }
                        else
                        {
                            doc_ref.Point_Left = 0;
                            doc_ref.Point = 0;
                            doc_ref.Point_Left = 0;
                            doc_ref.Spending = 0;
                            doc_ref.BillingNo = req.BillingNo;
                            doc_ref.UpdateTime = TxTimeStamp;
                            doc_ref.RejectMessage = req.RejectMessage;

                            log.Point = 0;
                            log.IsVoidPoint = true;

                            entities.SaveChanges();
                        }
                    }

                }

                doc_ref.Point_Left = 0;
                doc_ref.Point = 0;
                doc_ref.Point_Left = 0;
                doc_ref.Spending = 0;
                doc_ref.BillingNo = req.BillingNo;
                doc_ref.UpdateTime = TxTimeStamp;
                doc_ref.RejectMessage = req.RejectMessage;

                log.Point = 0;
                log.IsVoidPoint = true;

                Document_DN_Form_Value billingValue = new Document_DN_Form_Value()
                {
                    ContainerId = 1,
                    DocumentId = (int)doc.DocumentId,
                    FieldId = 2,
                    FormId = 1,
                    Seq = 1,
                    DataType = 1,
                    Key = "BillingNo",
                    Value = req.BillingNo,
                    CreatedTime = TxTimeStamp
                };
                entities.SaveChanges();
                
                RejectMessage rejectMessage = new RejectMessage()
                {
                    Line_UserId = customer.Line_UserId,
                    Remark = req.RejectMessage
                };

                var rejectFlexMessage = flexMessageBuilder.Build_Flex_Reject_Message(rejectMessage: rejectMessage);
                if (rejectFlexMessage != null)
                {
                    await flex_Message_Client_Service.SendFlexMessage(rejectFlexMessage);
                }

                return voidTransaction_Resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
