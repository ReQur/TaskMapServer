using System.Collections.Generic;
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
        Task AddNewBoard(Board newBoard, string userId);
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
                        WHERE board.userId=@userId AND utb.userId = @userId";
            return await DbQueryAsync<SharedInfoBoard>(sql, parameters);
        }

        public async Task<SharedInfoBoard> GetBoardInfo(string _boardId, string _userId)
        {
            var parameters = new { boardId = _boardId, userId = _userId };
            var sql = @"SELECT board.*, utb.access_level as accessRights
                        FROM board LEFT JOIN user_to_board utb on board.boardId = utb.boardId 
                        WHERE board.userId=@userId AND board.boardId=@boardId AND utb.userId = @userId";
            var _board = await DbQueryAsync<SharedInfoBoard>(sql, parameters);
            return _board.First();
        }

        public async Task AddNewBoard(Board newBoard, string userId)
        {
            _logger.LogDebug($"User[{userId}] has started creation new board with: " +
                             $"newBoard.boardDescription - {newBoard.boardDescription}, " +
                             $"newBoard.boardName - {newBoard.boardName}");
            newBoard.userId = uint.Parse(userId);
            var sql = @"INSERT INTO board(userId, boardName,boardDescription) 
                            VALUES(@userId, @boardName,@boardDescription);
                        INSERT INTO user_to_board(userId, boardId,access_level)
                            VALUES (@userId, LAST_INSERT_ID(), 'administrating');";
            await DbExecuteAsync(sql, newBoard, transaction: true);
        }

        public async Task DeleteBoard(string boardId)
        {
            var parameters = new { BoardId = boardId };
            var sql = @"DELETE FROM board WHERE boardId = @BoardId";
            await DbExecuteAsync(sql, parameters);
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

            await DbExecuteAsync(sql.ToString(), parameters);
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