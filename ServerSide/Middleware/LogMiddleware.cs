using System.Text;
namespace ServerSide.Middleware
{
	public class LogMiddleware
	{
		private readonly ILogger<LogMiddleware> _logger;
		private readonly RequestDelegate _next;
		public LogMiddleware(ILogger<LogMiddleware> logger, RequestDelegate next)
		{
			_logger = logger;
			_next = next;
		}
		public async Task Invoke(HttpContext context)
		{
			_logger.LogInformation($"Запрос : {context.Request.Method} {context.Request.Path}");
			// Возможность доп обработки лога
			await _next(context);
			if (context.Response.StatusCode != StatusCodes.Status200OK)
			{
				if (context.Request.ContentLength.HasValue && context.Request.ContentLength > 0)
				{
					context.Request.EnableBuffering(); // Включаем возможность повторного чтения потока
					using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
					{
						var requestBody = await reader.ReadToEndAsync();
						_logger.LogInformation($"Request Body: {requestBody}");
						context.Request.Body.Position = 0; 
					}
				}
			}
			else
			{
				_logger.LogInformation($"Ответ с кодом: {context.Response.StatusCode}");
			}
		}
	}
}
