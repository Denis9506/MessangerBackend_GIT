using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.CompilerServices;

namespace MessangerBackend.Filters
{
    public class LoggingFilter : Attribute, IActionFilter
    {
        private readonly string _methodname;
        public LoggingFilter([CallerMemberName]string? methodName = null) { 
            _methodname = methodName;
        }
        //before
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"Before action :{context.HttpContext.Request.Path.Value} {_methodname}");
        }

        //after
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"After action :{context.HttpContext.Request.Path.Value} {_methodname}");
        }
    }
}
