using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace dotnetserver.Middleware
{
    public class EntryLog
    {
        private readonly RequestDelegate _nextMiddleware;

        public EntryLog (RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }

        public async Task Invoke(HttpContext context, ILogger<EntryLog> logger)
        {
            logger.LogTrace($"Taken request from {context.Request.Headers["origin"]}");
            await _nextMiddleware.Invoke(context);
        }
    }
}