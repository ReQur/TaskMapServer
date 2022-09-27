using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.Extensions.Logging;


namespace dotnetserver
{
    public interface ITaskService
    {
        Task<BoardTask> EditTask(BoardTask newTask);
        Task<IEnumerable<BoardTask>> GetBoardTasks(string boardId);
        Task<BoardTask> AddTask(BoardTask newTask);
        Task DeleteTask(string _taskId);

    }
    public class TaskService: WithDbAccess, ITaskService
    {
        private readonly ILogger<TaskService> _logger;
        public TaskService(ILogger<TaskService> logger, ConnectionContext context) : base(context)
        {
            _logger = logger;
        }
        public async Task<BoardTask> EditTask(BoardTask task)
        {
            var sql = @"UPDATE task 
                        SET 
                            boardId=@boardId,
                            taskLabel=@taskLabel,
                            taskText=@taskText,
                            taskColor=@color,
                            state=@state,
                            coordinates=@coordinates
                        WHERE taskId=@taskId;

                        SELECT *, taskColor as color 
                        FROM task 
                        WHERE userId=@userId 
                          AND taskId=@taskId";

            var tasks = await DbQueryAsync<BoardTask>(sql, task);
            return tasks.Last();
        }
        public async Task<IEnumerable<BoardTask>>GetBoardTasks(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = "SELECT taskId, boardId, userId, taskLabel, taskText, taskColor as color, state, coordinates FROM task WHERE boardId=@BoardId";
            return await DbQueryAsync<BoardTask>(sql, parameters);
        }

        public async Task<BoardTask> AddTask(BoardTask newTask)
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
                        SELECT *, taskColor as color FROM task WHERE userId=@userId";
            var tasks = await DbQueryAsync<BoardTask>(sql, newTask);
            return tasks.Last();
        }

        public async Task DeleteTask(string _taskId)
        {
            var parameters = new { taskId = _taskId };
            var sql = @"DELETE FROM task 
                        WHERE
                        taskId = @taskId";
            await DbExecuteAsync(sql, parameters);
        }

    }
}