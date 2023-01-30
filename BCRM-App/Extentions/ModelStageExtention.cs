using BCRM.Common.Factory;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace BCRM_App
{
    public static class ModelStageExtention
    {
        public static string GetErrorModelStage(this ModelStateDictionary modelStage)
        {
            var modelValue = modelStage.Values.Select(value => value.Errors)
                            .Where(value => value.Count() > 0)
                            .FirstOrDefault();
            return modelValue[0].ErrorMessage;
        }

        public static string ThrowErrorModelStage(this ModelStateDictionary modelStage, IBCRM_Exception_Factory _bcrm_Ex_Factory)
        {
            var modelValue = modelStage.Values.Select(value => value.Errors)
                            .Where(value => value.Count() > 0)
                            .FirstOrDefault();
            
            throw _bcrm_Ex_Factory.Build(errorId: 1000400, message: modelValue[0].ErrorMessage, title_th: modelValue[0].ErrorMessage, message_th: modelValue[0].ErrorMessage, title_en: "", message_en: "");
        }
    }
}
