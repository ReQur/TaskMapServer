using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using dotnetserver.Models;


namespace dotnetserver
{
    public class TaskService
    {
        public static List<BoardTask> Tasks = new List<BoardTask>()
        {
            new BoardTask(0, 0, 0, (0, 0)),
            new BoardTask(1, 0, 0, (100, 100)),
        };
        public static IEnumerable<BoardTask> GetBoardTasks(string boardId)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Tasks));
            return Tasks.Where(task => task.BoardId == int.Parse(boardId));
        }

        public static void SetNewTaskPosition(string _taskCoordinates)
        {
            dynamic taskCoordinates = Newtonsoft.Json.JsonConvert.DeserializeObject(_taskCoordinates);
            var index = Tasks.FindIndex(f => f.TaskId == taskCoordinates[0].taskId);
            Tasks[index].Coordinates = taskCoordinates[0].coordinates;
        }
    }
}