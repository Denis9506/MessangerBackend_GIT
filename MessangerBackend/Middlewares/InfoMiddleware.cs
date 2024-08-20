namespace MessangerBackend.Middlewares
{
    public class InfoMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<InfoMiddleware> _logger;

        public InfoMiddleware(RequestDelegate requestDelegate, ILogger<InfoMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext ctx) {
            using var sr = new StreamReader(ctx.Request.Body);
            string info = $"Path:{ctx.Request.Path}{Environment.NewLine}Body:{sr.ReadToEnd()}";
            await _requestDelegate.Invoke(ctx);
            _logger.LogInformation(info);
        }
    }
    //public static class MiddlewareExtensione {
    //    public static IApplicationBuilder UseInfo(this IApplicationBuilder application) { 
        
    //    }
    //}

}
