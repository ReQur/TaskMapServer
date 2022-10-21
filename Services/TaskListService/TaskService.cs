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
        Task<BoardTask[]> GetBoardTasks(string boardId);
        Task<BoardTask> AddTask(BoardTask newTask);
        Task DeleteTask(IBoardTask newTask);

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
                            x=@x, y=@y
                        WHERE taskId=@taskId;

                        SELECT *, taskColor as color 
                        FROM task 
                        WHERE userId=@userId 
                          AND taskId=@taskId";

            var tasks = await DbQueryAsync<BoardTask>(sql, task);
            return tasks.Last();
        }
        public async Task<BoardTask[]>GetBoardTasks(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = "SELECT taskId, boardId, userId, taskLabel, taskText, taskColor as color, state, x, y FROM task WHERE boardId=@BoardId";
            TaskList taskList = new TaskList(await DbQueryAsync<BoardTask>(sql, parameters));
            return taskList.GetTasks();
        }

        public async Task<BoardTask> AddTask(BoardTask newTask)
        {
            var sql = @"INSERT INTO
                            task(boardId, userId, taskLabel, taskText, taskColor, state, x, y)
                            VALUES (@boardId, @userId, @taskLabel', @taskText, @color, @state, @x, @y);
                        UPDATE task
                            SET next_task_id = LAST_INSERT_ID()
                            WHERE boardId = @boardId AND next_task_id IS NULL AND taskId <> LAST_INSERT_ID();";
            var tasks = await DbQueryAsync<BoardTask>(sql, newTask);
            return tasks.Last();
        }

        public async Task DeleteTask(IBoardTask newTask)
        {
            var sql = @"SELECT next_task_id INTO @_next_task_id FROM task WHERE taskId=@taskId;
                        UPDATE task SET next_task_id = @_next_task_id WHERE next_task_id=@taskId;
                        DELETE FROM task WHERE taskId = @taskId;";
            await DbExecuteAsync(sql, newTask);
        }

    }
}