using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace dotnetserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost("log")]
        public async Task<IActionResult> SaveLog(ClientLog clientLog)
        {
            _logger.LogInformation($"Receive post request from {HttpContext.Request.Headers["origin"]}");
            _clientLogService.SaveUserLog(clientLog, HttpContext.Request.Headers["user-agent"]);
            return Ok();
        }

    }

}