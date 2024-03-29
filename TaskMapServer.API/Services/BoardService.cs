﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using dotnetserver.Models;
using Microsoft.Extensions.Logging;


namespace dotnetserver
{
    public interface IBoardService
    {
        Task ChangeBoardInformation(Board newBoard);
        Task<IEnumerable<SharedInfoBoard>> GetBoards(string userId);
        Task<Board> AddNewBoard(Board newBoard, string userId);
        Task DeleteBoard(string boardId);
        Task<SharedInfoBoard> GetBoardInfo(string boardId, string userId);
        Task ShareBoard(ShareRequest request);
        Task<string> GetUserAccessLevel(string userId, uint boardId);
        Task SetSharedFlag(uint _boardId, bool _isShared);
    }
    public class BoardService: WithDbAccess, IBoardService
    {
        private readonly ILogger<BoardService> _logger;
        public BoardService(ILogger<BoardService> logger, ConnectionContext context) : base(context)
        {
            _logger = logger;
        }

        public async Task ChangeBoardInformation(Board newBoard)
        {
            var sql = @"UPDATE board SET 
                        boardName=@boardName,
                        boardDescription=@boardDescription
                        WHERE boardId=@boardId";
            await DbExecuteAsync(sql, newBoard);
        }
        public async Task SetSharedFlag(uint _boardId, bool _isShared)
        {
            var parameters = new { boardId = _boardId, isShared = _isShared };
            var sql = @"UPDATE board SET 
                        isShared=@isShared
                        WHERE boardId=@boardId;";
            await DbExecuteAsync(sql, parameters);
        }
        public async Task<IEnumerable<SharedInfoBoard>>GetBoards(string _userId)
        {
            var parameters = new { userId = _userId };
            var sql = @"SELECT board.*, utb.access_level as accessRights
                        FROM board LEFT JOIN user_to_board utb on board.boardId = utb.boardId 
                        WHERE utb.userId = @userId";
            var boards = await DbQueryAsync<SharedInfoBoard>(sql, parameters);
            if (boards == null)
            {
                throw new Exception($"User {_userId} have no any boards");
            }
            var filteredBoards = boards
                .Where(b => b != null && (b.userId.ToString() == _userId || b.isShared));
            return filteredBoards;
        }

        public async Task<SharedInfoBoard> GetBoardInfo(string _boardId, string _userId)
        {
            var parameters = new { boardId = _boardId, userId = _userId };
            var sql = @"SELECT board.*, utb.access_level as accessRights
                        FROM board LEFT JOIN user_to_board utb on board.boardId = utb.boardId 
                        WHERE board.boardId=@boardId AND utb.userId = @userId";
            var _board = await DbQueryAsync<SharedInfoBoard>(sql, parameters);
            return _board.First();
        }

        public async Task<Board> AddNewBoard(Board newBoard, string userId)
        {
            _logger.LogDebug($"User[{userId}] has started creation new board with: " +
                             $"newBoard.boardDescription - {newBoard.boardDescription}, " +
                             $"newBoard.boardName - {newBoard.boardName}");
            newBoard.userId = uint.Parse(userId);
            var sql = @"INSERT INTO board(userId, boardName,boardDescription) 
                            VALUES(@userId, @boardName,@boardDescription);
                        SELECT LAST_INSERT_ID() INTO @_boardId;
                        INSERT INTO user_to_board(userId, boardId,access_level)
                            VALUES (@userId, LAST_INSERT_ID(), 'administrating');
                        SELECT * FROM board WHERE boardId=@_boardId";
            var board = await DbQueryAsync<Board>(sql, newBoard, transaction: true);
            return board.FirstOrDefault();
        }

        public async Task DeleteBoard(string boardId)
        {
            var boardParameter = new { BoardId = boardId };
            var sqlSelectUsers = @"SELECT userId FROM user WHERE lastBoardId = @BoardId";
            var userIds = await DbQueryAsync<uint>(sqlSelectUsers, boardParameter);

            if (userIds.Count() > 0)
            {
                var updateUserLastBoard = new DynamicParameters();
                var updateUserLastBoardSql = new StringBuilder("");
                int index = 0;
                foreach (uint userId in userIds)
                {
                    updateUserLastBoardSql.Append($@"UPDATE 
                                        user 
                               SET lastBoardId = (SELECT MIN(boardId) 
                                                  FROM user_to_board 
                                                  WHERE userId = @userId{index} 
                                                    AND boardId <> @boardId{index})
                                WHERE userId = @userId{index};");
                    updateUserLastBoard.Add($"userId{index}", userId);
                    updateUserLastBoard.Add($"boardId{index}", boardId);
                    index++;
                }
                await DbExecuteAsync(updateUserLastBoardSql.ToString(), updateUserLastBoard, transaction: true);
            }

            var deleteBoardSql = @"DELETE FROM board WHERE boardId=@BoardId";
            await DbExecuteAsync(deleteBoardSql, boardParameter);
        }

        public async Task ShareBoard(ShareRequest request)
        {
            var sql = new StringBuilder("INSERT INTO user_to_board (userId, boardId, access_level) VALUES ");
            var parameters = new DynamicParameters();

            int index = 0;
            foreach (uint userId in request.userIdList)
            {
                sql.Append($"(@userId{index}, @boardId{index}, @accessRights{index}),");
                parameters.Add($"userId{index}", userId);
                parameters.Add($"boardId{index}", request.boardId);
                parameters.Add($"accessRights{index}", request.accessRights);
                index++;
            }

            // Remove the last comma
            sql.Length--;

            sql.Append(" ON DUPLICATE KEY UPDATE access_level = VALUES(access_level);");
            sql.Append("UPDATE board SET isShared=1 WHERE boardId=@boardId0;");

            await DbExecuteAsync(sql.ToString(), parameters, transaction: true);
        }

        public async Task<string> GetUserAccessLevel(string userId, uint boardId)
        {
            var parameters = new { UserId = userId, BoardId = boardId };
            var sql = @"SELECT access_level FROM user_to_board 
                WHERE userId = @UserId AND boardId = @BoardId";

            var accessLevel = await DbQueryAsync<string>(sql, parameters);

            return accessLevel.First();
        }
    }
}