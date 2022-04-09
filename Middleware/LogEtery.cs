using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace dotnetserver.Middleware
{
    public class LogEtery
    {
        private readonly RequestDelegate _nextMiddleware;

        public LogEtery (RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }

        public async Task Invoke(HttpContext context, ILogger<LogEtery> logger)
        {
            logger.LogInformation($"Taken request from {context.Request.Headers["origin"]}");
            await _nextMiddleware.Invoke(context);
        }
    }
}