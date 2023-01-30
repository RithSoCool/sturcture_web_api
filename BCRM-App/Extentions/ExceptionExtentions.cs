using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BCRM_App.Extentions
{

    public static class ErrorException
    {
        public static object getErrorModelStage(this ModelStateDictionary modelState)
        {
            var ModelValue = modelState.Values.Select(value => value.Errors).FirstOrDefault();
            if (ModelValue == null)
            {
                return null;
            }
            return ModelValue[0].ErrorMessage;
        }
    }

    public static class ExtensionModel
    {
        public static string GetErrorModelStage(this ModelStateDictionary modelStage)
        {
            var modelValue = modelStage.Values.Select(value => value.Errors)
                            .Where(value => value.Count() > 0)
                            .FirstOrDefault();
            return modelValue[0].ErrorMessage;
        }

        public static Exception GetErrorExceptoin(this Exception exception)
        {
            if (exception.InnerException != null)
            {
                return exception.InnerException.GetErrorExceptoin();
            }
            return exception;
        }
    }
}
