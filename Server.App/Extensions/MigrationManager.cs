using Server.Migrations.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.Migrations.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    //databaseService.CreateDatabase("MigrationExample");

                    migrationService.ListMigrations();
                    migrationService.MigrateUp();

                    //migrationService.MigrateDown(202106280001);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return host;
        }
    }
}
