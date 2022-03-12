using System;
using System.Collections.Generic;
using System.Linq;
using dotnetserver.Models;


namespace dotnetserver
{
    public class TaskService
    {
        public static List<BoardTask> Tasks = new List<BoardTask>();

        public static IEnumerable<BoardTask> GetBoardTasks(string boardId)
        {
            return Tasks.Where(task => task.BoardId == int.Parse(boardId));
        }
    }
}