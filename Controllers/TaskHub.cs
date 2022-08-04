using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace dotnetserver.Controllers
{
    public class TaskHub: Hub
    {
        private readonly ILogger<TaskHub> _logger;
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public TaskHub(ILogger<TaskHub> logger, IUserService userService, ITaskService taskService)
        {
            _logger = logger;
            _userService = userService;
            _taskService = taskService;
        }

        private static readonly Dictionary<string, string> ConnectionsGroup = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnectionsGroup.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConnectionsGroup[Context.ConnectionId]);
                ConnectionsGroup.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task<IEnumerable<BoardTask>> JoinBoard(uint _boardId)
        {
            string boardId = _boardId.ToString();
            _logger.LogDebug($"User has connected to {boardId}");
            if (ConnectionsGroup.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConnectionsGroup[Context.ConnectionId]);
                ConnectionsGroup.Remove(Context.ConnectionId);
            }
            ConnectionsGroup.Add(Context.ConnectionId, boardId);
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);
            try
            {
                var res = await _taskService.GetBoardTasks(boardId);
                await _userService.SetLastBoardId(boardId);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new HubException(ex.Message);
            }

        }
        public async Task<BoardTask> EditTask(BoardTask task) 
        {
            try
            {
                await _taskService.SetNewTaskPosition(task);
                await Clients.OthersInGroup(task.boardId.ToString()).SendAsync("editTask", task);
                return task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new HubException(ex.Message);
            }
        }
        public async Task<BoardTask> AddNewTask(BoardTask newTask)
        {
            Console.WriteLine(newTask);
            try
            {
                await _taskService.AddNewTask(newTask);
                await Clients.OthersInGroup(newTask.boardId.ToString()).SendAsync("newTask", newTask);
                return newTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new HubException(ex.Message);
            }
        }
        public async Task<bool> DeleteTask(IBoardTask newTask)
        {
            try
            {
                await _taskService.DeleteTask(newTask);
                await Clients.OthersInGroup(newTask.boardId.ToString()).SendAsync("deleteTask", newTask);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new HubException(ex.Message);
            }
        }

    }
}