using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using dotnetserver.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace dotnetserver
{
    public interface ITaskService
    {
        Task EditTask(BoardTask newTask);
        Task<IEnumerable<BoardTask>> GetBoardTasks(string boardId);
        Task AddTask(BoardTask newTask);
        Task DeleteTask(IBoardTask newTask);

    }
    public class TaskService: WithDbAccess, ITaskService
    {
        private readonly ILogger<TaskService> _logger;
        public TaskService(ILogger<TaskService> logger, ConnectionContext context) : base(context)
        {
            _logger = logger;
        }
        public async Task EditTask(BoardTask newTask)
        {
            var sql = @"UPDATE task SET 
                        boardId=@boardId,
                        taskLabel=@taskLabel,
                        taskText=@taskText,
                        taskColor=@color,
                        state=@state,
                        coordinates=@coordinates
                        WHERE taskId=@taskId";
            await DbExecuteAsync(sql, newTask);
        }
        public async Task<IEnumerable<BoardTask>>GetBoardTasks(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = "SELECT taskId, boardId, userId, taskLabel, taskText, taskColor as color, state, coordinates FROM task WHERE boardId=@BoardId";
            return await DbQueryAsync<BoardTask>(sql, parameters);
        }

        public async Task AddTask(BoardTask newTask)
        {
            var sql = @"INSERT INTO task(
                        boardId, userId, 
                        taskLabel, taskText,
                        taskColor, state,
                        coordinates) 
                        VALUES(
                        @boardId, @userId, 
                        @taskLabel, @taskText,
                        @color, @state,
                        @coordinates);
                        SELECT taskId FROM task WHERE userId=@userId";
            var taskId = await DbQueryAsync<uint>(sql, newTask);
            newTask.taskId = taskId.Last();
        }

        public async Task DeleteTask(IBoardTask newTask)
        {
            var sql = @"DELETE FROM task 
                        WHERE
                        taskId = @taskId";
            await DbExecuteAsync(sql, newTask);
        }

    }
}