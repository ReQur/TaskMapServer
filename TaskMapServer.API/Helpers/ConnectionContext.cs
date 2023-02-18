using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


namespace dotnetserver
{
    public class ConnectionContext
    {
        private readonly IConfiguration _configuration;
        public ConnectionContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection GenericConnection()
            => new MySqlConnection(_configuration.GetConnectionString("GenericConnection"));
        public IDbConnection MasterConnection()
            => new MySqlConnection(_configuration.GetConnectionString("MasterConnection"));
    }
}