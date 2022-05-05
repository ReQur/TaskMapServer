using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using dotnetserver.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;


namespace dotnetserver
{
    public interface IBoardService
    {
        Task ChangeBoardInformation(Board newBoard);
        Task<IEnumerable<Board>> GetBoards(string userId);
        Task AddNewBoard(Board newBoard, string userId);
        Task DeleteBoard(string boardId);

    }
    public class BoardService: IBoardService
    {
        public static IConfiguration Configuration { get; set; }
        private static string connStr;
        private readonly ILogger<BoardService> _logger;
        public BoardService(ILogger<BoardService> logger, IConfiguration config)
        {
            _logger = logger;
            Configuration = config;
            connStr = Configuration.GetConnectionString("mysqlconn");
        }

        public async Task ChangeBoardInformation(Board newBoard)
        {
            var sql = @"UPDATE board SET 
                        boardName=@boardName,
                        boardDescription=@boardDescription,
                        state=@state
                        WHERE boardId=@boardId";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, newBoard);
            }
        }
        public async Task<IEnumerable<Board>>GetBoards(string userId)
        {
            var parameters = new { UserId = userId };
            var sql = "SELECT * FROM board WHERE userId=@UserId";
            using (var db = new MySqlConnection(connStr))
            {
                return await db.QueryAsync<Board>(sql, parameters);
            }
        }

        public async Task AddNewBoard(Board newBoard, string userId)
        {
            _logger.LogDebug($"User[{userId}] has started creation new board with: " +
                             $"newBoard.boardDescription - {newBoard.boardDescription}, " +
                             $"newBoard.boardName - {newBoard.boardName}, " +
                             $"newBoard.state - {newBoard.state}");
            newBoard.userId = uint.Parse(userId);
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

        public async Task DeleteBoard(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = @"DELETE FROM task 
                        WHERE
                        boardId = @BoardId;
                        DELETE FROM board 
                        WHERE
                        boardId = @BoardId";
            using (var db = new MySqlConnection(connStr))
            {
                await db.ExecuteAsync(sql, parameters);
            }
        }

    }
}