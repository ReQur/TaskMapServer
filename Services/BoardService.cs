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
    public class BoardService
    {
        private static string connStr = "server=localhost;user=root;port=3306;database=TaskMap;password=rootPassword;";

        public static async Task ChangeBoardInformation(Board newBoard)
        {
            var sql = @"UPDATE task SET 
                        boardName=@boardName,
                        boardDescription=@boardDescription,
                        state=@state,
                        WHERE boardId=@boardId";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, newBoard);
            }
        }
        public static async Task<IEnumerable<Board>>GetBoards(string userId)
        {
            var parameters = new { UserId = userId };
            var sql = "SELECT * FROM board WHERE userId=@UserId";
            using (var db = new MySqlConnection(connStr))
            {
                return await db.QueryAsync<Board>(sql, parameters);
            }
        }

        public static async Task AddNewBoard(Board newBoard)
        {
            var sql = @"INSERT INTO board(
                        userId, boardName,
                        boardDescription,
                        state) 
                        VALUES(
                        @userId, @boardName,
                        @boardDescription,
                        @state);
                        SELECT boardId FROM board WHERE userId=@userId";
            using (var db = new MySqlConnection(connStr))
            {
                var boardId = await db.QueryAsync<uint>(sql, newBoard);
                newBoard.boardId = boardId.Last();
            }
        }

        public static async Task DeleteBoard(string boardId)
        {
            var sql = @"DELETE FROM board 
                        WHERE
                        boardId = @boardId";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, boardId);
            }
        }

    }
}