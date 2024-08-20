public class UserStatisticsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly UserStatisticsService _statisticsService;
    private readonly ILogger<UserStatisticsMiddleware> _logger;

    public UserStatisticsMiddleware(RequestDelegate next, UserStatisticsService statisticsService, ILogger<UserStatisticsMiddleware> logger)
    {
        _next = next;
        _statisticsService = statisticsService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/users/search") && context.Request.Method == "GET")
        {
            var name = context.Request.RouteValues["nickname"]?.ToString();
            if (!string.IsNullOrEmpty(name))
            {
                _statisticsService.IncrementCount(name);

                _logger.LogInformation($"User search executed: {name}. Current count: {_statisticsService.GetStatistics()[name]}");
            }
        }

        await _next.Invoke(context);
    }
}
