using System;
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

        protected async Task<IEnumerable<T>> DbQueryAsync<T>(string sql, object parameters, bool transaction=false)
        {
            using var db = _context.GenericConnection();
            if (transaction)
            {
                db.Open();
                using var _transaction = db.BeginTransaction();
                try
                {
                    
                    var result = await db.QueryAsync<T>(sql, parameters, transaction: _transaction);
                    _transaction.Commit();
                    return result;
                }
                catch
                {
                    _transaction.Rollback();
                    throw;
                }
            }
           else
            return await db.QueryAsync<T>(sql, parameters);
        }

        protected async Task DbExecuteAsync(string sql, object parameters, bool transaction = false)
        {
            using var db = _context.GenericConnection();
            if (transaction)
            {
                db.Open();
                using var _transaction = db.BeginTransaction();
                try
                {
                    await db.ExecuteAsync(sql, parameters);
                    _transaction.Commit();
                    return;
                }
                catch
                {
                    _transaction.Rollback();
                    throw;
                }
            }
            else
                await db.ExecuteAsync(sql, parameters);
        }
    }
}