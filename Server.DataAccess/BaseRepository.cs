using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataAccess
{
    public delegate void Log(object message);


    public abstract class BaseRepository<T> : Repository<T> where T : class
    {

        protected readonly string m_TableName;
        protected IConnectionFactory connectionFactory;
        protected Log logger;
        protected string table;

        public BaseRepository(string connetionstring, Log log)
        {
            this.connectionFactory = new ConnectionFactory(connetionstring);
            table = DataUtils.GetTableName<T>();
            logger += log;
        }

        public async Task<bool> BulkInsert( string query, IEnumerable<object> items)
        {
            using(IDbConnection conn = connectionFactory.GetConnection)
            {
                using (var tran = conn.BeginTransaction())
                {
                    
                    try
                    {
                        await conn.ExecuteAsync(query, items, tran);
                        tran.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        public async Task<bool> Upsert(string query, object parameters)
        {
            using (IDbConnection conn = connectionFactory.GetConnection)
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        await conn.ExecuteAsync(query, parameters, tran);
                        tran.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        protected override int Execute(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    int affected = conn.Execute(query, parameters, commandType : commandType);
                    
                    return affected;
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                logger?.Invoke(ex);
                return 0;
            }
        }

        protected override async Task<int> ExecuteAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    int affected = await conn.ExecuteAsync(query, parameters, commandType: commandType);
                    return affected;
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                Console.WriteLine("Unable to execute " + ex.Message);
                logger?.Invoke(ex);
                return 0;
            }
        }

        protected async Task<T> ExecuteScalarAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    
                    T affected = await conn.ExecuteScalarAsync<T>(query, parameters, commandType : commandType);
                    return affected;
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                logger?.Invoke(ex);
                return null;
            }
        }

        protected override List<T> Query(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return conn.Query<T>(query, parameters, commandType : commandType).ToList();
                }
            }
            catch (Exception ex)
            {
                
                logger?.Invoke(ex);
                return new List<T>();
            }
        }

        protected override async Task<List<T>> QueryAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    var result = await conn.QueryAsync<T>(query, parameters, commandType : commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return new List<T>();
            }
        }

        protected async Task<List<TResult>> QueryAsync<TFirst, TSecond, TResult>(string query, Func<TFirst, TSecond, TResult> options, object param, string _splitOn = "", CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    var result = await conn.QueryAsync<TFirst, TSecond, TResult>(query, options,param: param, splitOn : _splitOn, commandType : commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return new List<TResult>();
            }
        }

        protected async Task<List<TResult>> QueryAsync<TFirst, TSecond, TThird, TResult>(string query, Func<TFirst, TSecond, TThird, TResult> options, object param, string _splitOn = "", CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    var result = await conn.QueryAsync<TFirst, TSecond, TThird, TResult>(query, options, param: param, splitOn: _splitOn, commandType : commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return new List<TResult>();
            }
        }

        protected async Task<List<TResult>> QueryAsync<TFirst, TSecond, TThird, TFourth, TResult>(string query, Func<TFirst, TSecond, TThird, TFourth, TResult> options, object param, string _splitOn = "", CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    var result = await conn.QueryAsync<TFirst, TSecond, TThird, TFourth, TResult >(query, options, param: param, splitOn: _splitOn, commandType: commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return new List<TResult>();
            }
        }

        protected override T QueryFirst(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return conn.QueryFirst<T>(query, parameters, commandType : commandType);
                }

            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        protected override async Task<T> QueryFirstAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    var result = await conn.QueryFirstAsync<T>(query, parameters, commandType : commandType);
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        protected override T QueryFirstOrDefault(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return conn.QueryFirstOrDefault<T>(query, parameters, commandType : commandType);
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        protected override async Task<T> QueryFirstOrDefaultAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {

                    return await conn.QueryFirstOrDefaultAsync<T>(query, parameters, commandType : commandType);
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        protected override T QuerySingle(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return conn.QuerySingle<T>(query, parameters, commandType : commandType);
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        protected override async Task<T> QuerySingleAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return await conn.QuerySingleAsync<T>(query, parameters, commandType : commandType);
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                Console.WriteLine(ex.Message, "QUILA");
                return default;
            }
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return await conn.QueryMultipleAsync(query, parameters, commandType : commandType);
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        protected override T QuerySingleOrDefault(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return conn.QuerySingleOrDefault<T>(query, parameters, commandType : commandType);
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        protected override async Task<T> QuerySingleOrDefaultAsync(string query, object parameters = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection conn = connectionFactory.GetConnection)
                {
                    return await conn.QuerySingleOrDefaultAsync<T>(query, parameters, commandType : commandType);
                }
            }
            catch (Exception ex)
            {
                logger?.Invoke(ex);
                return default;
            }
        }

        
    }
}
