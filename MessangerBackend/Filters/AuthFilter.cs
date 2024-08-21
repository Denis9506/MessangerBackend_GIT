using Microsoft.AspNetCore.Mvc.Filters;

namespace MessangerBackend.Filters
{
    public class AuthFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var token = context.HttpContext.Request.Headers.Authorization.Count;
            if (token == 0) {
                context.HttpContext.Abort();
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
