using Server;
using FluentMigrator.Runner;
using FluentMigrator.Postgres;
using System.Reflection;
using CyberspawnServer.Migrations.Extensions;
using CyberspawnServer.Migrations.Context;
using CyberspawnServer.Migrations.Migrations;
using Server.DataAccess;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<Database>();




IConfiguration Configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
        .AddFluentMigratorCore()
        .ConfigureRunner(c => c.AddPostgres11_0()
            .WithGlobalConnectionString(Configuration.GetConnectionString("Default"))
            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

var app = builder.Build();


var serverManager = new ServerManager( new DataService(Configuration.GetConnectionString("Default")));
serverManager.networkManager.StartServer("127.0.0.1", 1137);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MigrateDatabase();

app.UseAuthorization();

app.MapControllers();

app.Run();


