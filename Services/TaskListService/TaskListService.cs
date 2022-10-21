using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.Extensions.Logging;


namespace dotnetserver
{
    public interface ITaskListService
    {
    }
    public class TaskListService: ITaskListService
    {

        private readonly ILogger<TaskListService> _logger;
        private readonly ITaskService _taskService;

        private IEnumerable<IBoardTask> tasks;

        public TaskListService(ILogger<TaskListService> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

    }
}