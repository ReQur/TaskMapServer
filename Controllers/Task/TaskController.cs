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
    [Authorize]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        /// <summary>
        /// Returns all tasks from one board.
        /// </summary>
        /// <returns>List of tasks of asked board</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(IEnumerable<BoardTask>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetTasks(string boardId)
        {
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            var res = await _taskService.GetBoardTasks(boardId);
            return Ok(res);
        }
    }

}