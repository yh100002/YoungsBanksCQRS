using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace YoungsBanks.Filters
{
    public class BadRequestActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {                            
                actionContext.Result = new ValidationFailedResult(actionContext.ModelState);
            }            

            base.OnActionExecuting(actionContext);
        }
    }
}
