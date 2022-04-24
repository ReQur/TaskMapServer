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
        Task SetNewTaskPosition(BoardTask newTask);
        Task<IEnumerable<BoardTask>> GetBoardTasks(string boardId);
        Task AddNewTask(BoardTask newTask);
        Task DeleteTask(IBoardTask newTask);

    }
    public class TaskService: ITaskService
    {
        public static IConfiguration Configuration { get; set; }
        private static string connStr;

        private readonly ILogger<TaskService> _logger;
        public TaskService(ILogger<TaskService> logger, IConfiguration config)
        {
            _logger = logger;
            Configuration = config;
            connStr = Configuration.GetConnectionString("mysqlconn");
        }
        public async Task SetNewTaskPosition(BoardTask newTask)
        {
            var sql = @"UPDATE task SET 
                        boardId=@boardId,
                        taskLabel=@taskLabel,
                        taskText=@taskText,
                        taskColor=@color,
                        state=@state,
                        coordinates=@coordinates
                        WHERE taskId=@taskId";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, newTask);
            }
        }
        public async Task<IEnumerable<BoardTask>>GetBoardTasks(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = "SELECT taskId, boardId, userId, taskLabel, taskText, taskColor as color, state, coordinates FROM task WHERE boardId=@BoardId";
            using (var db = new MySqlConnection(connStr))
            {
                return await db.QueryAsync<BoardTask>(sql, parameters);
            }
        }

        public async Task AddNewTask(BoardTask newTask)
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
            using (var db = new MySqlConnection(connStr))
            {
                var taskId = await db.QueryAsync<uint>(sql, newTask);
                newTask.taskId = taskId.Last();
            }
        }

        public async Task DeleteTask(IBoardTask newTask)
        {
            var sql = @"DELETE FROM task 
                        WHERE
                        taskId = @taskId";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, newTask);
            }
        }

    }
}