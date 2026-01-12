using Microsoft.EntityFrameworkCore;
using PhishingGame.Blazor;
using PhishingGame.Blazor.Components;
using Radzen;
using PhishingGame.Data;
using PhishingGame.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGameStates()
    .AddDbContext<PhishingDbContext>(ConfigureOptions)
    .AddRadzenComponents()
    .AddHttpContextAccessor()
    .AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAnonymousUserId();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

void ConfigureOptions(DbContextOptionsBuilder options)
{
    var providerString = builder.Configuration["DatabaseEngine"];
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    switch (providerString) 
    {
        case "MySql":
            options.UseMySQL(connectionString);
            break;
        case "SqlServer":
            options.UseSqlServer(connectionString);
            break;
    }
}