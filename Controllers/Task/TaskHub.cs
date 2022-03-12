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
        public async Task JoinBoard(string boardId)
        {
            if (ConnectionsGroup.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConnectionsGroup[Context.ConnectionId]);
                ConnectionsGroup.Remove(Context.ConnectionId);
            }
            ConnectionsGroup.Add(Context.ConnectionId, boardId);
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);
        }
        public async Task NewTaskPosition(string boardId, string taskCoordinates)
        {
            TaskService.SetNewTaskPosition(taskCoordinates);
            await Clients.OthersInGroup(boardId).SendAsync("newTaskPosition", taskCoordinates);
        }
        
    }
}