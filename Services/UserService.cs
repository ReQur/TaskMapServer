using System;
using System.Linq;
using System.Threading.Tasks;
using dotnetserver.Models;
using Microsoft.Extensions.Logging;

namespace dotnetserver
{
    public interface IUserService
    {
        Task<bool> IsAnExistingUser(string userName);
        Task<bool> IsValidUserCredentials(string userName, string password);
        Task<string> GetUserId(string userName);
        Task<TbUser> GetUserData(string userName);
        Task<bool> RegisterUser(TbUser user);
        Task SetLastBoardId(string boardId);

    }

    public class UserService : WithDbAccess, IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, ConnectionContext context) : base(context)
        {
            _logger = logger;
        }

        public async Task<bool> IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}] with password [{password}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var parameters = new { UserName = userName, Password = password };
            var sql = "SELECT * FROM user WHERE email=@UserName and md5PasswordHash=@Password";

            try
            {
                var user = await DbQueryAsync<TbUser>(sql, parameters);
                var _ = user.First();
            }
            catch (Exception e)
            {
                return false;
            }
            
            
            return true;

        }

        public async Task<bool> IsAnExistingUser(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT * FROM user WHERE email=@userName";
            try
            {
                var user = await DbQueryAsync<TbUser>(sql, parameters);
                var _ = user.First();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<string> GetUserId(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT userId FROM user WHERE email=@userName";
            try
            {
                 var user = await DbQueryAsync<string>(sql, parameters);
                 return user.First();

            }
            catch (Exception e)
            {
                throw(new Exception("Was try to get userId of unknown user"));
            }
            
        }

        public async Task<TbUser> GetUserData(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT * FROM user WHERE email=@userName";
            try
            {
                var user = await DbQueryAsync<TbUser>(sql, parameters);
                return user.First();

            }
            catch (Exception e)
            {
                throw (new Exception("Was try to get data of unknown user"));
            }
            
        }

        public async Task<bool> RegisterUser(TbUser user)
        {
            var sql = @"CALL RegisterUser_proc(@email, @firstName, @lastName, @md5PasswordHash)";
            try
            {
                var newUserId = await DbQueryAsync<uint>(sql, user);
                user.userId = newUserId.First();
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected error while register new user {e}");
                return false;
            }
            
            return true;
        }

        public async Task SetLastBoardId(string boardId)
        {
            var parameters = new { boardId };
            var sql = "UPDATE user SET lastBoardId=@boardId WHERE userId=(SELECT userId FROM board WHERE boardId=@boardId)";
            try
            {
                await DbExecuteAsync(sql, parameters);
            }
            catch (Exception e)
            {
                throw (new Exception($"Was try to set lastboardId={boardId} for user"));
            }
        }
    }

}