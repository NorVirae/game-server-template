using CyberspawnServer.Migrations.Context;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnServer.Migrations.Migrations
{
    public class Database
    {
        private readonly DapperContext _context;

        public Database(DapperContext context)
        {
            _context = context;
        }

        public async void CreateDatabase(string dbName)
        {
            var query = $"SELECT * FROM pg_database WHERE name = {dbName}";
            var parameters = new DynamicParameters();
            parameters.Add("name", dbName);

            using (var conn = _context.CreateMasterConnection())
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        var result = command.ExecuteScalar();
                        conn.Close();

                        if (result != null && result.ToString().Equals(dbName))
                        {
                            Console.WriteLine("Db exist");
                        } //always 'true' (if it exists) or 'null' (if it doesn't)
                        else
                        {
                            await conn.ExecuteAsync($"CREATE DATABASE {dbName}");
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " FROM ERROR " + dbName);
                        CreateDatabase(dbName);
                    }

                }


            }
        }
    }
}
