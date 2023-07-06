using Server.Core;
using Server.DataAccess.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Server.DataAccess
{

    public class GenericArrayHandler<T> : SqlMapper.TypeHandler<T[]>
    {
        public override void SetValue(IDbDataParameter parameter, T[] value)
        {
            parameter.Value = value;
        }
        public override T[] Parse(object value) => (T[])value;
    }

    public class StringListTypeHandler<T> : SqlMapper.TypeHandler<List<string>>
    {
        public override List<string> Parse(object value)
        {
            return ((string[])value).ToList();
        }

        public override void SetValue(IDbDataParameter parameter, List<string> value)
        {
            parameter.Value = value.ToArray();
        }
    }


    public class DataService : IDataService
    {
        public string connectionString;
        public DataService(string connectionString)
        {
            this.connectionString = connectionString;
            SqlMapper.AddTypeHandler(new StringListTypeHandler<List<string>>());
            Init();
        }

        private UserRepository user;



        public UserRepository User => user;

 
        void Init()
        {
            user = new UserRepository(this);
        }

        public void Log(object message)
        {
            Logger.LogInfo(message);
        }

        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}
