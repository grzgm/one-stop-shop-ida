using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OneStopShopIdaBackend.Controllers;
using OneStopShopIdaBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add the CodeChallengeGeneratorService as a singleton service
builder.Services.AddSingleton<CodeChallengeGeneratorService>();

// Add the MicrosoftGraphApiService as a Scoped service
builder.Services.AddScoped<IMicrosoftGraphApiService, MicrosoftGraphApiService>();

// Add the SlackApiServices as a Scoped service
builder.Services.AddScoped<ISlackApiServices, SlackApiServices>();

// Add Data Protection and configure XML encryptor
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtectionKeys"));

// Add the Database connection as a Scoped service
builder.Services.AddDbContext<IDatabaseService, DatabaseService>(opt =>
    opt.UseMySQL(builder.Configuration["ConnectionStrings:MySqlConnection"]));

// Add the WeeklyTaskService as a Singleton service
// builder.Services.AddSingleton<IHostedService, WeeklyTaskController>();

// Add the LunchRecurringController as a Singleton service
builder.Services.AddSingleton<IHostedService, LunchRecurringTaskController>();

// Register HttpClient as a singleton service
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();

// Enable and configure in-memory sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// JWT setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});

// Using in memory cache
builder.Services.AddMemoryCache();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration["FrontendUri"])
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error");
}

// Enable CORS
app.UseCors(builder => builder
    .WithOrigins(app.Configuration["FrontendUri"])
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();

public partial class Program
{
}