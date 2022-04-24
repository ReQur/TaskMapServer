using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using dotnetserver.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace dotnetserver
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserId(string userName);
        public bool RegisterUser(TbUser user);

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

        public bool IsValidUserCredentials(string userName, string password)
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
                    var user = db.Query<TbUser>(sql, parameters);
                    var first = user.First();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            
            return true;

        }

        public bool IsAnExistingUser(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT * FROM user WHERE email=@userName";
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                    TbUser user = db.Query<TbUser>(sql, parameters).First();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetUserId(string userName)
        {
            var parameters = new { userName };
            var sql = "SELECT userId FROM user WHERE email=@userName";
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                     return db.Query<string>(sql, parameters).First();
                }
                catch (Exception e)
                {
                    throw(new Exception("Was try to get userId of unknown user"));
                }
            }
        }

        public bool RegisterUser(TbUser user)
        {
            var sql = @"INSERT INTO user(email, firstName, lastName, md5PasswordHash) 
                        VALUES(@email, @firstName, @lastName, @md5PasswordHash);
                        SELECT userId FROM user WHERE email=@userName";
            using (var db = new MySqlConnection(connStr))
            {
                try
                {
                     user.userId = db.Query<uint>(sql, user).First();
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