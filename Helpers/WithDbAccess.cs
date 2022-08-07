using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;


namespace dotnetserver
{
    public class WithDbAccess
    {
        private readonly ConnectionContext _context;

        public WithDbAccess(ConnectionContext context)
        {
            _context = context;
        }

        protected async Task<IEnumerable<T>> DbQueryAsync<T>(string sql, object parameters)
        {
            using var db = _context.GenericConnection();
            return await db.QueryAsync<T>(sql, parameters);
        }

        protected async Task DbExecuteAsync(string sql, object parameters)
        {
            using var db = _context.GenericConnection();
            await db.QueryAsync(sql, parameters);
        }
    }
}