using BCRM.Common.Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCRM_App.Filters
{
    public class ModelStateValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                IBCRM_Exception_Factory bcrm_Ex_Factory = (IBCRM_Exception_Factory)context.HttpContext.RequestServices.GetService(typeof(IBCRM_Exception_Factory));
                
                var modelValue = context.ModelState.Values.Select(value => value.Errors)
                .Where(value => value.Count() > 0)
                .FirstOrDefault();

                throw bcrm_Ex_Factory.Build(errorId: 1000400, message: modelValue[0].ErrorMessage, title_th: modelValue[0].ErrorMessage, message_th: modelValue[0].ErrorMessage, title_en: "", message_en: "");
            }

            base.OnActionExecuting(context);
        }
    }
}
