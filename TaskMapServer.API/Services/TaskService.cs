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
        Task<BoardTask> GetTask(string _taskId);
        Task<BoardTask[]> GetBoardTasks(string boardId);
        Task AddTask(BoardTask newTask);
        Task DeleteTask(string _taskId);
        Task UpdatePriority(uint taskToMove, uint posBefore, uint _boardId);

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

            var tasks = await DbQueryAsync<BoardTask>(sql, task, transaction: true);
            return tasks.Last();
        }
        public async Task<BoardTask[]>GetBoardTasks(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = "SELECT taskId, boardId, next_task_id, userId, createdDate, taskLabel, taskText, taskColor as color, state, x, y FROM task WHERE boardId=@BoardId";
            TaskList taskList = new TaskList(await DbQueryAsync<BoardTask>(sql, parameters));
            return taskList.GetTasks();
        }

        public async Task AddTask(BoardTask newTask)
        {
            var sql = @"INSERT INTO
                            task(boardId, userId, taskLabel, taskText, taskColor, state, x, y)
                            VALUES (@boardId, @userId, @taskLabel, @taskText, @color, @state, @x, @y);
                        UPDATE task
                            SET next_task_id = LAST_INSERT_ID()
                            WHERE boardId = @boardId AND next_task_id IS NULL AND taskId <> LAST_INSERT_ID();";
            await DbExecuteAsync(sql, newTask, transaction: true);
        }
        public async Task<BoardTask> GetTask(string _taskId)
        {
            var parameters = new { taskId = _taskId };
            var sql = @"DELETE * FROM task WHERE taskId = @taskId;";
            var tasks = await DbQueryAsync<BoardTask>(sql, parameters, transaction: true);
            return tasks.Last();
        }

        public async Task DeleteTask(string _taskId)
        {
            var parameters = new { taskId = _taskId };
            var sql = @"SELECT next_task_id INTO @_next_task_id FROM task WHERE taskId=@taskId;
                        UPDATE task SET next_task_id = @_next_task_id WHERE next_task_id=@taskId;
                        DELETE FROM task WHERE taskId = @taskId;";
            await DbExecuteAsync(sql, parameters, transaction: true);
        }

        public async Task UpdatePriority(uint _taskToMove, uint _posBefore, uint _boardId)
        {
            var parameters = new { taskToMove = _taskToMove, posBefore = _posBefore, boardId = _boardId };
            var sql = @"";
            if (_posBefore == 0)
                sql = @"SELECT next_task_id INTO @_next_task_id FROM task WHERE taskId=@taskToMove;
                UPDATE task SET next_task_id = @_next_task_id WHERE next_task_id=@taskToMove;
                UPDATE task SET boardId = @boardId WHERE taskId=@taskToMove;
                UPDATE task SET next_task_id = @taskToMove WHERE next_task_id IS NULL and boardId=@boardId;
                UPDATE task SET next_task_id = NULL WHERE taskId=@taskToMove;";
            else
                sql = @"SELECT next_task_id INTO @_next_task_id FROM task WHERE taskId=@taskToMove;
                UPDATE task SET boardId = @boardId WHERE taskId=@taskToMove;
                UPDATE task SET next_task_id = @_next_task_id WHERE next_task_id=@taskToMove;
                UPDATE task SET next_task_id = @taskToMove WHERE next_task_id=@posBefore;
                UPDATE task SET next_task_id = @posBefore WHERE taskId=@taskToMove;";
            await DbExecuteAsync(sql, parameters, transaction: true);

        }
    }
}