using backend.data;
using backend.interfaces;
using backend.models;
using backend.repositories;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

Env.Load();

var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000";

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
var connectionSring = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "";
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "";
var signingKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY") ?? "";

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
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(signingKey))
{
    throw new InvalidOperationException("JWT configuration missing in environment variables.");
}


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<AppDBContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = builder.Configuration[audience],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(signingKey ?? throw new InvalidOperationException("JWT SigningKey is not configured."))
        )
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
