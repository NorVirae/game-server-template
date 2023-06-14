using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CyberSpawnsServer.DataAccess
{
    public abstract class Repository<T> where T : class
    {
        protected abstract List<T> Query(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract Task<List<T>> QueryAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract T QueryFirst(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract Task<T> QueryFirstAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract T QueryFirstOrDefault(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract Task<T> QueryFirstOrDefaultAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract T QuerySingle(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract Task<T> QuerySingleAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract T QuerySingleOrDefault(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract Task<T> QuerySingleOrDefaultAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract int Execute(string query, object parameters = null, CommandType? commandType = CommandType.Text);
        protected abstract Task<int> ExecuteAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text);
    }
}
