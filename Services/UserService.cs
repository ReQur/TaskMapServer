using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using dotnetserver.Models;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace dotnetserver
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserId(string userName);
    }

    public class UserService : IUserService
    {
        private static string connStr = "server=localhost;user=root;port=3306;database=TaskMap;password=rootPassword;";

        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var parameters = new { userName, password };
            var sql = "SELECT * FROM user WHERE email=@userName, md5PasswordHash=@password";
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
    }

}