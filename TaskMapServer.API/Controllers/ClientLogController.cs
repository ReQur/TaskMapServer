using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace dotnetserver.Controllers
{
    [ApiController]
    [Route("api/send-log")]
    public class ClientLogController : ControllerBase
    {
        private readonly ILogger<ClientLogController> _logger;
        private readonly IUserService _userService;
        private readonly IClientLogService _clientLogService;

        public ClientLogController(ILogger<ClientLogController> logger, IUserService userService, IClientLogService clientLogService)
        {
            _logger = logger;
            _userService = userService;
            _clientLogService = clientLogService;
        }
        /// <summary>
        /// Takes critical/error logs from client and write it to log file
        /// </summary>
        /// <remarks>
        /// That logs creating automaticly by logger
        /// </remarks>
        /// <returns>Noting</returns>
        /// <response code="200">Success</response>
        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> SaveLog(ClientLog clientLog)
        {
            _logger.LogInformation($"Receive post request from {HttpContext.Request.Headers["origin"]}");
            _clientLogService.SaveUserLog(clientLog, HttpContext.Request.Headers["user-agent"]);
            return Ok();
        }

    }

}