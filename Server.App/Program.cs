using Server;

var builder = WebApplication.CreateBuilder(args);

var Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
string connectionStrings = builder.Configuration.GetConnectionString("Default");
var serverManager = new ServerManager(connectionStrings);
serverManager.networkManager.StartServer("127.0.0.1", 1137);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();


