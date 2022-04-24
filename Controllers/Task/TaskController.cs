using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace dotnetserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(string boardId)
        {
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            var res = await _taskService.GetBoardTasks(boardId);
            return Ok(res);
        }
    }

}