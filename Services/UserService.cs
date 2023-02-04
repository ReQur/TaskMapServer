﻿using System;
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
        Task<UserData> GetUserData(string userName);
        Task<uint> RegisterUser(SignUpUser user);
        Task SetLastBoardId(string boardId);
        Task SetAvatart(string userName, string avatar);

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
            var sql = "SELECT * FROM user WHERE username=@UserName and password=@Password";

            try
            {
                var user = await DbQueryAsync<UserData>(sql, parameters);
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
            var sql = "SELECT * FROM user WHERE username=@userName";
            try
            {
                var user = await DbQueryAsync<UserData>(sql, parameters);
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
            var sql = "SELECT userId FROM user WHERE username=@userName";
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

        public async Task<UserData> GetUserData(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT * FROM user WHERE username=@userName";
            try
            {
                var user = await DbQueryAsync<UserData>(sql, parameters);
                return user.First();

            }
            catch (Exception e)
            {
                throw (new Exception("Was try to get data of unknown user"));
            }
            
        }

        public async Task<uint> RegisterUser(SignUpUser user)
        {
            var sql = @"INSERT INTO user(username, firstName, lastName, password, avatar) 
                            VALUES(@username, @avatar, @firstName, @lastName, @password);
                        SELECT LAST_INSERT_ID() INTO @_userId;
                        INSERT INTO board(userId, boardName,  boardDescription) 
                            VALUES(@_userId, 'Default', 'Your first board');
                        UPDATE user
                            SET lastBoardId = (SELECT LAST_INSERT_ID())
                            WHERE userId = @_userId;
                        SELECT @_userId";
            try
            {
                var userId = await DbQueryAsync<uint>(sql, user);
                return userId.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected error while register new user {e}");
                return 0;
            }
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

        public async Task SetAvatart(string userName, string avatar)
        {
            var parameters = new { _userName = userName, _avatar = avatar};
            var sql = "UPDATE user SET avatar=@_avatar WHERE username=@_userName";
            try
            {
                await DbExecuteAsync(sql, parameters);
            }
            catch (Exception e)
            {
                throw (new Exception($"Was try to set avatar for user"));
            }
        }
    }

}