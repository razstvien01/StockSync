using backend.data;
using backend.interfaces;
using backend.repositories;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load();

var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000";

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
var connectionSring = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(urls);
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseNpgsql(connectionSring);
});
builder.Services.AddControllers();

// * In order to DI works
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
