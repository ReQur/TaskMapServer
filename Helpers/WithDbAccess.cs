using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using dotnetserver.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;


namespace dotnetserver
{
    public class WithDbAccess
    {
        private static string _connStr;
        public WithDbAccess(IConfiguration config)
        {
            var Configuration = config;
            _connStr = Configuration.GetConnectionString("mysqlconn");
        }

        protected async Task<IEnumerable<T>> DbQueryAsync<T>(string sql, object parameters)
        {
            await using var db = new MySqlConnection(_connStr);
            return await db.QueryAsync<T>(sql, parameters);
        }

        protected async Task DbExecuteAsync(string sql, object parameters)
        {
            await using var db = new MySqlConnection(_connStr);
            await db.QueryAsync(sql, parameters);
        }
    }
}