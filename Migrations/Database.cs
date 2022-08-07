using Dapper;


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