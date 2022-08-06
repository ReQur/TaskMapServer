using System;
using System.Collections.Generic;
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
    public class Database 
    {
        private readonly ConnectionContext _context;
        public Database(ConnectionContext context)
        {
            _context = context;
        }
        public void CreateDatabase()
        {
            var query = "CREATE DATABASE IF NOT EXISTS TaskMap;";
            using var connection = _context.MasterConnection();
            connection.Execute(query);
        }
    }
}