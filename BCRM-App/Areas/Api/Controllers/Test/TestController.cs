using BCRM.Common.Factory;
using BCRM.Common.Filters.Action;
using BCRM.Common.Services;
using BCRM_App.Areas.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static BCRM.Common.Constants.BCRM_Core_Const.Api.Filter;
using System.Collections.Generic;
using BCRM_App.Services.RemoteInternal.Authentication;
using BCRM_App.Constants;
using BCRM_App.Filters;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM_App.Areas.Api.Services.Repository.Customer;
using Microsoft.EntityFrameworkCore;
using BCRM.Common.Services.Data;
using BCRM_App.Configs;
using BCRM.Common.Context;

using Org.BouncyCastle.Ocsp;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage;
using BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Areas.Api.Services.Repository.Wallet;
using BCRM_App.Areas.Api.Controllers.Test.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BCRM_App.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiVersion("1.0")]
    public class TestController : API_BCRM_Controller
    {
        private readonly ICustomer_Service customerService;
        private readonly Line_Repository lineRepository;
        private readonly BCRM_36_Entities entities;
        private readonly ILine_FlexMessage_Client_Service line_FlexMessage_Client_Service;
        private readonly FlexMessageBuilder flexMessageBuilder;
        private readonly IBCRM_Tag_Service tag_Service;
        private readonly Wallet_Repository wallet_Repository;

        public TestController(ILogger<TestController> logger,
                                  IBCRM_Exception_Factory bcrm_Ex_Factory,
                                  Line_Repository lineRepository,
                                  BCRM_36_Entities entities,
                                  ILine_FlexMessage_Client_Service line_FlexMessage_Client_Service,
                                  FlexMessageBuilder flexMessageBuilder,
                                  IBCRM_Tag_Service tag_Service,
                                  Wallet_Repository wallet_Repository,
                                  IHttpContextAccessor httpContext_Accessor) : base(logger, bcrm_Ex_Factory, httpContext_Accessor)
        {
            this.lineRepository = lineRepository;
            this.entities = entities;
            this.line_FlexMessage_Client_Service = line_FlexMessage_Client_Service;
            this.flexMessageBuilder = flexMessageBuilder;
            this.tag_Service = tag_Service;
            this.wallet_Repository = wallet_Repository;
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public IActionResult GetLine(string name)
        {
            try
            {
                var lineInfo = lineRepository.Query(it => it.Name == name).FirstOrDefault();

                var res = new
                {
                    Params = name,
                    Info = lineInfo
                };

                return Ok(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public IActionResult DeleteAccount(string mobileNo)
        {
            try
            {
                var customerProfile = entities.sp_BCRM_DeleteAccount.FromSqlRaw("sp_BCRM_DeleteAccount @MobileNo={0}", mobileNo).AsEnumerable().FirstOrDefault();

                if (customerProfile == null) return BadRequest("Account not found.");

                return Ok($"{mobileNo} has deleted.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        public async Task<IActionResult> SendFlexMessage([FromBody] RejectMessage earnPointMessage)
        {
            try
            {
                var flexMessage = flexMessageBuilder.Build_Flex_Reject_Message(earnPointMessage);

                var res = await line_FlexMessage_Client_Service.SendFlexMessage(flexMessage);

                return Ok(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Post)]
        public IActionResult EarnPointForTest([FromBody] EarnPointForTest_Req req ) 
        {
            try
            {
                var customer = (from crm in entities.CRM_Customers
                                join bcrm in entities.BCRM_Customers on crm.CustomerId equals bcrm.CRM_CustomerId
                                where crm.MobileNo == req.MobileNo
                                select bcrm).FirstOrDefault();
                if (customer == null) throw new Exception("Customer not found.");

                var earnPointResp = wallet_Repository.EarnPointForTest(customerId: customer.CRM_CustomerId, point: req.Point, wallet_Alt_Ref: customer.Wallet_Alt_Ref);
                return Ok(earnPointResp);
            }
            catch (Exception ex)
            {
                throw ex;
         
            }
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public IActionResult Debug()
        {
            try
            {
                string respPayload = "\"{\"request_id\":\"0038c19a-ff70-40a8-a16a-b7e67413a353\",\"status\":\"success\",\"data\":{\"transaction\":{\"status\":1,\"is_duplicate_tx\":false,\"ledger\":{\"ledgerid\":48,\"ref_ledgerid\":null,\"main_void_ledgerid\":null,\"redeemid\":null,\"assetid\":1,\"walletid\":47,\"wl_appid\":null,\"crm_customerid\":47,\"wl_identityid\":2853,\"main_tx\":1,\"txreference\":\"4ba4a630-7dcf-4793-aa09-b941c5d3c71f\",\"ext_transactionid\":\"ecd3452b-d7ec-459a-a854-11f95a9c9a2a_3\",\"tx_type\":1,\"tx_type_desc\":\"I\",\"tx_redeem_mode\":0,\"status\":1,\"status_desc\":\"A\",\"pre_balance\":300,\"tx_amount\":100,\"remaining\":100,\"balance\":400,\"reference\":\"D00047-1108-QjiPT\",\"extra_ref\":\"100\",\"extra_ref_2\":null,\"tx_exp_data\":null,\"tx_exp_dt\":null,\"tx_req_identityid\":null,\"tx_time\":\"2022-11-08T10:53:25.9189534+07:00\",\"updated_dt\":\"2022-11-08T10:53:25.9189534+07:00\"},\"txreference\":\"4ba4a630-7dcf-4793-aa09-b941c5d3c71f\",\"ext_transactionid\":\"ecd3452b-d7ec-459a-a854-11f95a9c9a2a_3\",\"tx_time\":\"2022-11-08T10:53:25.9189534+07:00\"}},\"error\":null}";
                JObject respData = null;
                 respData = (JObject)JsonConvert.DeserializeObject(respPayload, new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });

                return Ok(respData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BCRM_AcceptVerb(BCRM_HttpMethods.Get)]
        public IActionResult Test()
        {
            return Ok("Server Found.");
        }
    }
}
