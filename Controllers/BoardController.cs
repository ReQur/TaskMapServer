using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Route("api/board")]
    [Authorize]
    public class BoardController : ControllerBase
    {
        private readonly ILogger<BoardController> _logger;
        private readonly IUserService _userService;
        private readonly IBoardService _boardService;

        public BoardController(ILogger<BoardController> logger, IUserService userService, IBoardService boardService)
        {
            _logger = logger;
            _userService = userService;
            _boardService = boardService;
        }

        /// <summary>
        /// Returns all boards of the authorized user.
        /// </summary>
        /// <remarks>
        /// Example of returnable list:
        /// 
        ///     [
        ///         {
        ///             "boardId": 3,
        ///             "userId": 2,
        ///             "createdDate": "05/16/2022 10:18:12",
        ///             "boardName": "Default",
        ///             "boardDescription": "Your first board"
        ///         }
        ///     ]
        /// 
        /// </remarks>
        /// <returns>List of boards of authorized user</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(IEnumerable<Board>), 200)]
        [HttpGet("get-boards")]
        public async Task<IActionResult> GetBoards()
        {
            var userId = await _userService.GetUserId(User.Identity?.Name);
            _logger.LogInformation($"Receive get boards request from user with id {userId}");
            var res = await _boardService.GetBoards(userId);
            return Ok(res);
        }

        /// <summary>
        /// Delete one board by given ID of the authorized user.
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="id" type="string" description="The ID of the board that need to be deleted"> </param>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [HttpDelete("delete-board")]
        public async Task<IActionResult> DeleteBoard(string boardId)
        {
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            await _boardService.DeleteBoard(boardId);
            return Ok();
        }

        /// <summary>
        /// Takes new board, creates it and returns new board ID
        /// </summary>
        /// <remarks>
        /// Accepted value example:
        /// Note that the boardId is zero and createdDate is empty.
        /// These values will be given by server
        /// 
        ///     
        ///         {
        ///             "boardId": 0,
        ///             "userId": 2,
        ///             "createdDate": "",
        ///             "boardName": "mynewboard",
        ///             "boardDescription": "I have just create it!"
        ///         }
        ///     
        /// 
        /// </remarks>
        /// <param name="newBoard"></param>
        /// <returns>ID of created board</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(int), 200)]
        [HttpPost("add-board")]
        public async Task<IActionResult> AddBoard([FromBody, Required] Board newBoard)
        {
            var userId = await _userService.GetUserId(User.Identity?.Name);
            _logger.LogInformation($"Receive post request from {HttpContext.Request.Headers["origin"]}");
            await _boardService.AddNewBoard(newBoard, userId);
            return Ok(newBoard.boardId);
        }

        /// <summary>
        /// Takes new board's data, change it in database.
        /// </summary>
        /// <remarks>
        /// Accepted value example:
        /// Note that only boardName and boardDescription can be changed
        /// 
        ///     
        ///         {
        ///             "boardId": 0,
        ///             "userId": 2,
        ///             "createdDate": "",
        ///             "boardName": "mynewboardname",
        ///             "boardDescription": "I have just change the name of my board"
        ///         }
        ///     
        /// 
        /// </remarks>
        /// <param name="newBoard"></param>
        /// <returns>Nothing</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [HttpPut("change-board")]
        public async Task<IActionResult> ChangeBoardInformation(Board newBoard)
        {
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            await _boardService.ChangeBoardInformation(newBoard);
            return Ok();
        }
    }

}