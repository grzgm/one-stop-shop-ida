using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Controllers;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

var builder = WebApplication.CreateBuilder(args);
var SlackClientSecret = builder.Configuration["Slack:SlackClientSecret"];
var SlackAccessToken = builder.Configuration["Slack:SlackAccessToken"];

// Add services to the container.
builder.Services.AddControllers();

// Add the CodeChallengeGeneratorService as a singleton service
builder.Services.AddSingleton<CodeChallengeGeneratorService>();

// Add the MicrosoftGraphApiService as a Scoped service
builder.Services.AddScoped<MicrosoftGraphApiService>();

// Add the SlackApiServices as a Scoped service
builder.Services.AddScoped<SlackApiServices>();

// Add the Database connection as a Scoped service
builder.Services.AddDbContext<DatabaseService>(opt =>
    opt.UseMySQL(builder.Configuration["ConnectionStrings:MySqlConnection"]));

// Add the DailyTaskService as a Singleton service
builder.Services.AddSingleton<IHostedService, MidnightTaskService>();

// Add the WeeklyTaskService as a Singleton service
builder.Services.AddSingleton<IHostedService, WeeklyTaskService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors(builder => builder
    .WithOrigins("http://localhost:5173")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();

public partial class Program { }