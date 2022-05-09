using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using dotnetserver.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace dotnetserver
{
    public interface IUserService
    {
        Task<bool> IsAnExistingUser(string userName);
        Task<bool> IsValidUserCredentials(string userName, string password);
        Task<string> GetUserId(string userName);
        Task<TbUser> GetUserData(string userName);
        Task<bool> RegisterUser(TbUser user);

    }

    public class UserService : IUserService
    {
        public static IConfiguration Configuration { get; set; }
        private static string connStr;
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, IConfiguration config)
        {
            _logger = logger;
            Configuration = config;
            connStr = Configuration.GetConnectionString("mysqlconn");
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
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                    var user = await db.QueryAsync<TbUser>(sql, parameters);
                    var first = user.First();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            
            return true;

        }

        public async Task<bool> IsAnExistingUser(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT * FROM user WHERE email=@userName";
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                    var user = await db.QueryAsync<TbUser>(sql, parameters);
                    user.First();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<string> GetUserId(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT userId FROM user WHERE email=@userName";
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                     var user = await db.QueryAsync<string>(sql, parameters);
                     return user.First();

                }
                catch (Exception e)
                {
                    throw(new Exception("Was try to get userId of unknown user"));
                }
            }
        }

        public async Task<TbUser> GetUserData(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT * FROM user WHERE email=@userName";
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                    var user = await db.QueryAsync<TbUser>(sql, parameters);
                    return user.First();

                }
                catch (Exception e)
                {
                    throw (new Exception("Was try to get data of unknown user"));
                }
            }
        }

        public async Task<bool> RegisterUser(TbUser user)
        {
            var sql = @"INSERT INTO user(email, firstName, lastName, md5PasswordHash) 
                        VALUES(@email, @firstName, @lastName, @md5PasswordHash);
                        SELECT userId FROM user WHERE email=@email";
            var sqlCreateBoard = @"INSERT INTO board(userId, boardName, boardDescription) 
                        VALUES(@userId, 'Default', 'Your first board');";
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                    var newUserId = await db.QueryAsync<uint>(sql, user);
                    user.userId = newUserId.First();
                    await db.ExecuteAsync(sqlCreateBoard, user);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Unexpected error while register new user {e}");
                    return false;
                }
            }
            return true;
        }
    }

}