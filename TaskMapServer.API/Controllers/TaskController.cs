using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace dotnetserver.Controllers
{

    [ApiController]
    [Route("api/task")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<BoardController> _logger;
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<BoardController> logger, IUserService userService, ITaskService taskService)
        {
            _logger = logger;
            _userService = userService;
            _taskService = taskService;
        }

        /// <summary>
        /// Returns all tasks from the board
        /// </summary>
        /// <returns>List of tasks of board</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(IEnumerable<BoardTask>), 200)]
        [HttpGet("{_boardId}")]
        public async Task<IActionResult> GetTasks(uint _boardId)
        {
            string boardId = _boardId.ToString();
            _logger.LogDebug($"User has open board {boardId}");
            try
            {
                var res = await _taskService.GetBoardTasks(boardId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(detail: "Exception due joining the board:\n" + ex.Message, statusCode: 500);
            }
        }


        /// <summary>
        /// Takes new verion of the task and replace all inforamtion of this task by taskId.
        /// </summary>
        /// <returns>Edited task</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(BoardTask), 200)]
        [HttpPut()]
        public async Task<IActionResult> EditTask([FromBody, Required] BoardTask task)
        {
            try
            {
                var editedTask = await _taskService.EditTask(task);
                return Ok(editedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(detail: "Exception due editing task:\n" + ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Takes new task without taskId/creationDate and returns task with completed fields
        /// </summary>
        /// <returns>Added task with set Id</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(BoardTask), 200)]
        [HttpPost()]
        public async Task<IActionResult> AddTask([FromBody, Required] BoardTask newTask)
        {
            Console.WriteLine(newTask);
            try
            {
                var userId = await _userService.GetUserId(User.Identity?.Name);
                newTask.userId = UInt32.Parse(userId);
                await _taskService.AddTask(newTask);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(detail: "Exception due adding task:\n" + ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Takes task and deletes it
        /// </summary>
        /// <returns>200/500 on success/error</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(Boolean), 200)]
        [HttpDelete("{taskId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteTask(uint taskId)
        {
            try
            {
                await _taskService.DeleteTask(taskId.ToString());
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(detail: "Exception due deleting task:\n" + ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Takes task and move it to new position in list
        /// </summary>
        /// <returns>200/500 on success/error</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(Boolean), 200)]
        [HttpPut("list/{taskToMove}&{posBefore}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateTaskPriority(uint taskToMove, uint posBefore)
        {
            try
            {
                await _taskService.UpdatePriority(taskToMove, posBefore);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(detail: "Exception due deleting task:\n" + ex.Message, statusCode: 500);
            }
        }

    }

}