using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.AspNetCore.SignalR;

namespace dotnetserver.Controllers
{
    public class TaskHub: Hub
    {
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
        public async Task JoinBoard(uint _boardId)
        {
            string boardId = _boardId.ToString();
            if (ConnectionsGroup.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConnectionsGroup[Context.ConnectionId]);
                ConnectionsGroup.Remove(Context.ConnectionId);
            }
            ConnectionsGroup.Add(Context.ConnectionId, boardId);
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);
        }
        public async Task NewTaskPosition(BoardTask task)
        {
            await TaskService.SetNewTaskPosition(task);
            await Clients.OthersInGroup(task.boardId.ToString()).SendAsync("newTaskPosition", task);
        }
        public async Task AddNewTask(BoardTask newTask)
        {
            Console.WriteLine(newTask);
            try
            {
                await TaskService.AddNewTask(newTask);
                //await Clients.Caller.SendAsync("additionSuccess");
                await Clients.Group(newTask.boardId.ToString()).SendAsync("newTask", newTask);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("additionError", ex.Message);
            }
        }
        public async Task DeleteTask(IBoardTask newTask)
        {
            try
            {
                await TaskService.DeleteTask(newTask);
                //await Clients.Caller.SendAsync("deletionSuccess");
                await Clients.Group(newTask.boardId.ToString()).SendAsync("deleteTask", newTask);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("deletionError", ex.Message);
            }
        }

    }
}