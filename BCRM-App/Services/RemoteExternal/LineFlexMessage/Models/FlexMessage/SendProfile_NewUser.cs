using System.Collections.Generic;
using Newtonsoft.Json;

namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models
{
    public class FlexMessage : ISend_Flex_Message
    {

        public string JsonTemplate { get; set; }
        public string LineId { get; set; }
        public string Titile { get; set; }
        public string BrandName { get; set; }

        public object BuildMessage()
        {

            var messageObject = new
            {
                to = LineId,
                messages = new List<object>
                {
                  new
                  {
                    type = "flex",
                    altText = Titile,
                    contents = JsonConvert.DeserializeObject<object>(JsonTemplate)
                  }
                },
            };

            return messageObject;
        }
    }
}
