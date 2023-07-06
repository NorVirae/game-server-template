using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnServer.Migrations.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_configuration.GetConnectionString("SqlConnection"));
        public NpgsqlConnection CreateMasterConnection()
        {
            Console.WriteLine(_configuration.GetConnectionString("MasterConnection"), "CHECK THIS");

            var conn = new NpgsqlConnection(_configuration.GetConnectionString("MasterConnection"));
            return conn;
        }
    }
}
