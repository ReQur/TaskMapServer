using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using dotnetserver.Models;
using MySql.Data.MySqlClient;


namespace dotnetserver
{
    public class TaskService
    {
        private static string connStr = "server=localhost;user=root;port=3306;database=TaskMap;password=rootPassword;";

        //public static List<BoardTask> Tasks = new List<BoardTask>()
        //{
        //    new BoardTask(0, 0, 0, new Dictionary<string, int>()
        //    {
        //        {"x", 0},
        //        {"y", 0}
        //    }),
        //    new BoardTask(1, 0, 0, new Dictionary<string, int>()
        //    {
        //        {"x", 100},
        //        {"y", 100}
        //    })
        //};
        //public static IEnumerable<BoardTask> GetBoardTasks(string boardId)
        //{
        //    return Tasks.Where(task => task.boardId == int.Parse(boardId));
        //}

        //public static void SetNewTaskPosition(string _taskCoordinates)
        //{
        //    dynamic taskCoordinates = Newtonsoft.Json.JsonConvert.DeserializeObject(_taskCoordinates);
        //    var index = Tasks.FindIndex(f => f.taskId == taskCoordinates[0].taskId);
        //    Tasks[index].coordinates["x"] = taskCoordinates[0].coordinates["x"];
        //    Tasks[index].coordinates["y"] = taskCoordinates[0].coordinates["y"];
        //}

        public static async Task<IEnumerable<BoardTask>>GetBoardTasks(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = "SELECT * FROM task WHERE boardId=@BoardId";
            using (var db = new MySqlConnection(connStr))
            {
                return await db.QueryAsync<BoardTask>(sql, parameters);
            }
        }

    }
}