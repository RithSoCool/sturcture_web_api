using System.Linq;
using BCRM_App.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using BCRM_App.Services.RemoteExternal.LineFlexMessage.Constants;
using BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Configs;
using BCRM_App.Constants;

namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models
{
    public class FlexMessageBuilder
    {

        private BCRM_Line_Flex_Template BuildFlexTemplate<T>(string templateName, T data)
        {
            BCRM_Line_Flex_Template template = null;
            var DutchmillOptionsBuilder = new DbContextOptionsBuilder<BCRM_36_Entities>().UseSqlServer(AppConstants.Database.ConnectionString.Dutchmill);
            using (BCRM_36_Entities DutchmillContext = new BCRM_36_Entities(DutchmillOptionsBuilder.Options))
            {

                template = (from tp in DutchmillContext.BCRM_Line_Flex_Templates
                            where ((tp.Valid_From == null || tp.Valid_Though == null) || (tp.Valid_From > DateTime.Now && tp.Valid_Though < DateTime.Now))
                                  && tp.Status == LineFlexConstants.Template.Status.Active
                                  && tp.SP_Mapped == templateName
                            select tp).FirstOrDefault();

                var templateDetails = (from details in DutchmillContext.BCRM_Line_Flex_Template_Replaces
                                       where details.TemplateId == template.TemplateId
                                       select details).ToList();

                if (templateDetails == null) return template;

                int index = 0;

                foreach (var replaceDetail in templateDetails)
                {
                    if (replaceDetail.Type == LineFlexConstants.Template_Replace.MappedType.MapByKey)
                    {
                        var value = data.GetValueFromProperty_By_Key(replaceDetail.Value);

                        template.JsonTemplate = template.JsonTemplate.Replace(replaceDetail.Key, value);
                    }
                    else if (replaceDetail.Type == LineFlexConstants.Template_Replace.MappedType.MapByIndex)
                    {
                        if (index == 0) continue;

                        var value = data.GetValueFromProperty_By_Index(index);

                        template.JsonTemplate = template.JsonTemplate.Replace(replaceDetail.Key, value);
                    }

                    index++;
                }
            }

            return template;

        }

        public ISend_Flex_Message Build_Flex_Earn_Point(EarnPointMessage earnPoint)
        {
            try
            {
                string templateName = "EarnPointMessage";

                var jsonTemplate = BuildFlexTemplate<EarnPointMessage>(templateName, earnPoint);
                if (jsonTemplate == null) return null;

                FlexMessage flexMessage = new FlexMessage()
                {
                    JsonTemplate = jsonTemplate.JsonTemplate,
                    LineId = earnPoint.Line_UserId,
                    Titile = jsonTemplate.Titile
                };

                return flexMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ISend_Flex_Message Build_Flex_Cumulative_Spending(CumulativeSpendingMessage cumulativeMessage)
        {
            try
            {
                string templateName = "CumulativeSpendingMessage";

                var jsonTemplate = BuildFlexTemplate<CumulativeSpendingMessage>(templateName, cumulativeMessage);
                if (jsonTemplate == null) return null;

                FlexMessage flexMessage = new FlexMessage()
                {
                    JsonTemplate = jsonTemplate.JsonTemplate,
                    LineId = cumulativeMessage.Line_UserId,
                    Titile = jsonTemplate.Titile
                };

                return flexMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ISend_Flex_Message Build_Flex_Reject_Message(RejectMessage rejectMessage)
        {
            try
            {
                string templateName = "RejectMessage";

                var jsonTemplate = BuildFlexTemplate<RejectMessage>(templateName, rejectMessage);
                if (jsonTemplate == null) return null;

                FlexMessage flexMessage = new FlexMessage()
                {
                    JsonTemplate = jsonTemplate.JsonTemplate,
                    LineId = rejectMessage.Line_UserId,
                    Titile = jsonTemplate.Titile
                };

                return flexMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
