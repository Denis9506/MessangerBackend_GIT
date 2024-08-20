using System.Text.RegularExpressions;
using System.Text;

namespace MessangerBackend.Middlewares
{
    public class MessageFilteringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MessageFilteringMiddleware> _logger;
        private readonly List<string> _forbiddenWords = new List<string> { "russia", "war", "moscow", "kremlin", "putin", "violence", "racism", "terrorism", "drugs", "weapon", "abuse", "murder", "crime", "explosives", "bomb", "assault", "kill", "fight", "nuke", "genocide", "blood", "death", "slavery" };

        public MessageFilteringMiddleware(RequestDelegate next, ILogger<MessageFilteringMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/message") && context.Request.Method == "POST")
            {
                context.Request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                var requestBody = Encoding.UTF8.GetString(buffer);
                context.Request.Body.Position = 0;

                foreach (var forbiddenWord in _forbiddenWords)
                {
                    if (requestBody.Contains(forbiddenWord, StringComparison.OrdinalIgnoreCase))
                    {
                        requestBody = Regex.Replace(requestBody, forbiddenWord, "***", RegexOptions.IgnoreCase);
                        _logger.LogInformation($"Заборонене слово '{forbiddenWord}' було знайдено і замінено в повідомленні.");
                    }
                }

                var newRequestBody = Encoding.UTF8.GetBytes(requestBody);
                context.Request.Body = new MemoryStream(newRequestBody);

                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            await _next(context);
        }
    }

}
