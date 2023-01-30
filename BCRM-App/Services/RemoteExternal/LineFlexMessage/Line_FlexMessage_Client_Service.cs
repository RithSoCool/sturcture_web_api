using BCRM.Common.Services;
using BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using static BCRM.Common.Services.BCRM_Client_Service_Api.Response;
using BCRM_App;
using BCRM_App.Configs;

namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage
{
    public interface ILine_FlexMessage_Client_Service
    {
        public Task<Line_Response> SendFlexMessage(ISend_Flex_Message message);
    }

    public class Line_FlexMessage_Client_Service : BCRM_Client, ILine_FlexMessage_Client_Service
    {
        private readonly ILogger<Line_FlexMessage_Client_Service> _logger;

        public Line_FlexMessage_Client_Service(IHttpClientFactory clientFactory, ILogger<Line_FlexMessage_Client_Service> logger) : base(clientFactory)
        {
            _logger = logger;
        }

        public async Task<Line_Response> SendFlexMessage(ISend_Flex_Message message)
        {
            if (message.BuildMessage() == null) return null;

            var lineConfig = App_Setting.Brands.Main.Config;

            Line_Response result = new Line_Response();
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["Authorization"] = $"Bearer {lineConfig.Line_Flex_Message_Token}";
            headers["Content-Type"] = "application/json";

            result.Success = await RequestAsync(
                              Action: Line_SC_Constant.Service.Api.Action.Push,
                              ServicePath: Line_SC_Constant.Service.Api.Path.v1.Push,
                              Method: HttpMethod.Post,
                              Payload: message.BuildMessage(),
                              headers: headers,
                              OnSuccess: (resp) => { result.ResponseSuccess = resp; },
                              OnError: (respError) => { result.ResponseError = respError; });

            return result;
        }
    }
}
