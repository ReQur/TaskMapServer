using dotnetserver.Models;
using Xunit.Sdk;

namespace TaskMapServer.Tests
{
    public class TaskListTest
    {
        private BoardTask[] orderedTasks
        {
            get
            {
                var _tasks = new BoardTask[5];
                for (int index = 0; index < 5; index++)
                {
                    _tasks[index] = new BoardTask
                    {
                        taskId = (uint)index,
                        next_task_id = index < 4 ? (uint)(index + 1) : 0,
                        boardId = 0,
                        userId = 0,
                        createdDate = "",
                        taskLabel = "",
                        taskText = "",
                        color = "",
                        state = 0,
                        x = 0,
                        y = 0
                    };
                }

                return _tasks;
            }
        }

        [Fact]
        public void TestOrderedCollection()
        {
            IEnumerable<BoardTask> taskCollection = new BoardTask[] { };
            for (int index = 0; index < 5; index++)
            {
                taskCollection = taskCollection.Append(new BoardTask
                {
                    taskId = (uint)index,
                    next_task_id = index < 4 ? (uint)(index + 1) : 0,
                    boardId = 0,
                    userId = 0,
                    createdDate = "",
                    taskLabel = "",
                    taskText = "",
                    color = "",
                    state = 0,
                    x = 0,
                    y = 0
                });
            }

            var taskList = new TaskList(taskCollection);
            var tasks = taskList.GetTasks();
            for (int index = 0; index < 5; index++)
            {
                Assert.Equal(tasks[index].taskId, orderedTasks[index].taskId);
            }
        }
    }
}