using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnetserver.Models
{
    public class ITaskList
    {
        protected BoardTask[] tasks;
    }

    public class TaskList : ITaskList
    {
        public TaskList(IEnumerable<BoardTask> _tasks)
        {
            var count = _tasks.Count();
            tasks = new BoardTask[count];
            
            if (count == 0)
            {
                return;
            }
            
            BoardTask t = _tasks.Single(x => x.next_task_id == 0);
            tasks[0] = t;
            for (int i = 1; i < count; i++)
            {
                t = _tasks.Single(x => x.next_task_id == t.taskId);
                tasks[i] = t;
            }
        }

        public BoardTask[] GetTasks() => tasks;
        
    }

}