using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace Server.DataAccess
{
    public class ConnectionFactory : IConnectionFactory
    {

        //public static IConfiguration Configuration;

        private readonly string connectionString;

        public ConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection GetConnection
        {
            get
            {
                var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
            }
        }

        public IDbConnection CreateConnection(string connection)
        {
            var conn = new NpgsqlConnection(connection);
            conn.Open();
            return conn;
        }
    }
}
