using dotnetserver.Controllers;
using dotnetserver.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Crypto.Tls;
using System.Net;
using System.Threading.Tasks;
using Ubiety.Dns.Core;

namespace dotnetserver.BoardNotificationHub
{
    [Authorize]
    public class BoardNotificationsHub : Hub
    {
        private readonly IUserService _userService;
        private readonly IBoardService _boardService;
        private readonly ILogger<BoardNotificationsHub> _logger;

        public BoardNotificationsHub(IUserService userService, IBoardService boardService, ILogger<BoardNotificationsHub> logger)
        {
            _userService = userService;
            _boardService = boardService;
            _logger= logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Got new connection with ID: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public async Task BoardTaskListChangedNotification(uint boardId)
        {
            _logger.LogInformation($"New task list event on: {boardId}");
            await Clients.OthersInGroup(boardId.ToString()).SendAsync("TaskListChangedNotification", boardId.ToString());
        }

        public async Task BoardChangedNotification(uint boardId)
        {
            _logger.LogInformation($"New board event on: {boardId}");
            await Clients.OthersInGroup(boardId.ToString()).SendAsync("BoardChangedNotification", boardId.ToString());
        }

        public async Task ShareBoardNotification(uint[] userIds)
        {
            _logger.LogInformation($"New board shared to users {userIds}");
            var strUserIds = Array.ConvertAll<uint, string>(userIds, input => input.ToString());
            await Clients.Users(strUserIds).SendAsync("ShareBoardNotification");
        }

        /// <summary>
        /// Joins a user to a specific board.
        /// </summary>
        /// <remarks>
        /// This endpoint allows a user to join a specific board. The user will then receive updates and notifications related to the board.
        ///
        /// Access to a board is restricted based on the user's access level for that board. If the user does not have read permissions for the board, they will be unable to join.
        ///
        /// A cookie named "boardGroup" will be set in the user's browser. This cookie will be used to keep track of the user's current board.
        ///
        /// The server will also join the user to the board's SignalR group. This enables the server to send real-time updates to the user about the board.
        ///
        /// This method is idempotent - it can be called multiple times without changing the result. However, it is not recommended to call this method unnecessarily, as it may cause unnecessary server load.
        ///
        /// </remarks>
        /// <param name="boardId">The ID of the board the user wishes to join.</param>
        /// <returns>An HTTP 200 OK status code if the operation was successful.</returns>
        /// <response code="200">Join successful.</response>
        /// <response code="403">If the user does not have read permissions for the board.</response>
        /// <response code="500">If an internal server error occurs.</response>
        public async Task JoinBoard(string groupName)
        {
            var userId = await _userService.GetUserId(Context.User?.Identity?.Name);
            try
            {
                // Проверка прав доступа пользователя
                var userAccessLevel = await _boardService.GetUserAccessLevel(userId, uint.Parse(groupName));
                if (!BoardPermissions.canRead(userAccessLevel))
                {
                    throw new HubException("Insufficient permissions");
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                _logger.LogInformation($"User: {userId} with connectionID: {Context.ConnectionId} joined to board {groupName}");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new HubException("Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Allows a user to leave a previously joined board.
        /// </summary>
        /// <remarks>
        /// This method is used when a user wants to stop receiving notifications from a specific board.
        /// It uses a cookie named "boardGroup" to identify the board which the user wants to leave.
        /// If the cookie is not found or the operation fails, an error message is returned.
        /// </remarks>
        /// <returns>
        /// Returns a status of 200 (OK) if the operation is successful.
        /// Returns a status of 500 (Internal Server Error) if an unexpected error occurs.
        /// </returns>
        /// <response code="200">Successfully left the board.</response>
        /// <response code="500">Internal server error. Please try again later.</response>
        public async Task LeaveBoard(string groupName)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                _logger.LogInformation($"User with connectionID: {Context.ConnectionId} left grom the board {groupName}");


            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new HubException("Internal server error. Please try again later.");
            }
        }
    }
}

