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

        [HttpGet("get-boards")]
        public async Task<IActionResult> GetBoards()
        {
            var userId = _userService.GetUserId(User.Identity?.Name);
            _logger.LogInformation($"Receive get boards request from user with id {userId}");
            var res = await _boardService.GetBoards(userId);
            return Ok(res);
        }

        [HttpGet("delete-board")]
        public async Task<IActionResult> DeleteBoard(string boardId)
        {
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            await _boardService.DeleteBoard(boardId);
            return Ok();
        }

        [HttpPost("add-board")]
        public async Task<IActionResult> AddBoard(Board newBoard)
        {
            var userId = _userService.GetUserId(User.Identity?.Name);
            _logger.LogInformation($"Receive post request from {HttpContext.Request.Headers["origin"]}");
            await _boardService.AddNewBoard(newBoard, userId);
            return Ok(newBoard.boardId);
        }

        [HttpPost("change-board")]
        public async Task<IActionResult> ChangeBoardInformation(Board newBoard)
        {
            _logger.LogInformation($"Receive get request from {HttpContext.Request.Headers["origin"]}");
            await _boardService.ChangeBoardInformation(newBoard);
            return Ok();
        }
    }

}