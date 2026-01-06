using Microsoft.EntityFrameworkCore;
using PhishingGame.Blazor;
using PhishingGame.Blazor.Components;
using Radzen;
using PhishingGame.Data;
using PhishingGame.Core;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddGameStates()
    .AddDbContext<PhishingDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
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