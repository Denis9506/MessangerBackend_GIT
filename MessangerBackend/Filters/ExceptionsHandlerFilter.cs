using Microsoft.AspNetCore.Mvc.Filters;

namespace MessangerBackend.Filters
{
    public class ExceptionsHandlerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {context.ExceptionDispatchInfo?.SourceException.Message}");
            Console.ResetColor();
        }
    }
}
