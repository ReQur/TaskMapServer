using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
        ///             "accessRights": "administrating",
        ///             "isShared": false,
        ///         }
        ///     ]
        /// 
        /// </remarks>
        /// <returns>List of boards of authorized user</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(IEnumerable<SharedInfoBoard>), 200)]
        [HttpGet()]
        public async Task<IActionResult> GetBoards()
        {
            var userId = await _userService.GetUserId(User.Identity?.Name);
            _logger.LogInformation($"Receive get boards request from user with id {userId}");
            var res = await _boardService.GetBoards(userId);
            return Ok(res);
        }

        /// <summary>
        /// Takes board ID and retruns information about it
        /// </summary>
        /// <returns>Information about board</returns>
        /// <param name="boardId" type="string" description="The ID of the board that need to be deleted"> </param>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(SharedInfoBoard), 200)]
        [HttpGet("{boardId}")]
        public async Task<IActionResult> GetBoardInfo(string boardId)
        {
            var userId = await _userService.GetUserId(User.Identity?.Name);
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            var res = await _boardService.GetBoardInfo(boardId, userId);
            return Ok(res);
        }

        /// <summary>
        /// Delete one board by given ID of the authorized user.
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="boardId" type="string" description="The ID of the board that need to be deleted"> </param>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [HttpDelete("{boardId}")]
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
        [HttpPost]
        public async Task<IActionResult> AddBoard([FromBody, Required] Board newBoard)
        {
            var userId = await _userService.GetUserId(User.Identity?.Name);
            _logger.LogInformation($"Receive post request from {HttpContext.Request.Headers["origin"]}");
             await _boardService.AddNewBoard(newBoard, userId);
            return Ok();
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
        /// <param name="board"></param>
        /// <returns>Nothing</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [HttpPut]
        public async Task<IActionResult> ChangeBoardInformation([FromBody, Required] Board board)
        {
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            await _boardService.ChangeBoardInformation(board);
            return Ok();
        }

        /// <summary>
        /// Shares the board with the specified users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/board/share
        ///     {
        ///         "boardId": 1,
        ///         "accessRights": "edit-access",
        ///         "userIdList": [2, 3, 4]
        ///     }
        ///
        /// Note that `accessRights` can be 'read-only', 'edit-access', 'administrating'.
        /// </remarks>
        /// <param name="request">The share request containing boardId, accessRights, and userIdList.</param>
        /// <returns>An IActionResult indicating the result of the share operation.</returns>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="200">If the board is successfully shared.</response>
        /// <response code="500">If there is an internal server error.</response>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("share")]
        public async Task<IActionResult> ShareBoard([FromBody] ShareRequest request)
        {
            if (request == null || request.userIdList == null || string.IsNullOrWhiteSpace(request.accessRights))
            {
                return BadRequest("Invalid request.");
            }

            var userId = await _userService.GetUserId(User.Identity?.Name);
            if (request.userIdList.Contains(uint.Parse(userId)))
            {
                return BadRequest("Your couldn't change your own access rights");
            }
            try
            {
                // Проверка прав доступа пользователя
                var userAccessLevel = await _boardService.GetUserAccessLevel(userId, request.boardId);
                if (userAccessLevel != "administrating")
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, "Insufficient permissions");
                }

                await _boardService.ShareBoard(request);
                await _boardService.SetSharedFlag(request.boardId, true);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }

}