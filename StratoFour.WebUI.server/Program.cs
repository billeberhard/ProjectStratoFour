using Microsoft.AspNetCore.Authentication.Cookies;
using StratoFour.Infrastructure.Data;
using StratoFour.Infrastructure.DbAccess;
using StratoFour.WebUI.server.Components;
using Microsoft.AspNetCore.ResponseCompression;
using StratoFour.WebUI.server.Hubs;
using StratoFour.Application.UserMatching;
using StratoFour.Domain;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Add SQL Data Access
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();

//Add User and Game Data Services
builder.Services.AddTransient<IUserData, UserData>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IGameData, GameData>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddTransient<IRobotData, RobotData>();
builder.Services.AddScoped<IRobotService, RobotService>();

//Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthService>();

builder.Services.AddHostedService<BackGroundWorkerService>();
builder.Services.AddSingleton<BackGroundWorkerService>();

builder.Services.AddHttpClient();

//Add SignalR
builder.Services.AddSignalR();

// Add Response Compression
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});

//Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddLogging();

// Build app
var app = builder.Build();


app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
//app.UseAuthorization();

//Map Razor Compontnts
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//Map Hubs
app.MapHub<GameHub>("/gamehub");

app.Run();
