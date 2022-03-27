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

        public static List<BoardTask> Tasks = new List<BoardTask>();
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

        public static async void SetNewTaskPosition(IBoardTask newTask)
        {
            var sql = "UPDATE task SET coordinates=@coordinates WHERE taskId=@taskId";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, newTask);
            }
        }
        public static IEnumerable<BoardTask>GetBoardTasks(string boardId)
        {
            //var parameters = new { BoardId = boardId };
            //var sql = "SELECT * FROM task WHERE boardId=@BoardId";
            //using (var db = new MySqlConnection(connStr))
            //{
            //    return await db.QueryAsync<BoardTask>(sql, parameters);
            //}
            return Tasks.ToArray();
        }

        public static async Task AddNewTask(BoardTask newTask)
        {
            //var sql = @"INSERT INTO task(
            //            boardId, userId, 
            //            taskLabel, taskText,
            //            taskColor, state,
            //            coordinates) 
            //            VALUES(
            //            @boardId, @userId, 
            //            @taskLabel, @taskText,
            //            @taskColor, @state,
            //            @coordinates)";
            //using (var db = new MySqlConnection(connStr))
            //{
            //     await db.ExecuteAsync(sql, newTask);
            //}
            Tasks.Add(newTask);
        }

        public static async Task DeleteTask(IBoardTask newTask)
        {
            var sql = @"DELETE * FROM task 
                        WHERE
                        taskId = @taskId)";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, newTask);
            }
        }

    }
}