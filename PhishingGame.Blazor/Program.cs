using Microsoft.EntityFrameworkCore;
using PhishingGame.Blazor;
using PhishingGame.Blazor.Components;
using PhishingGame.Data;
using PhishingGame.Core;
using Radzen;


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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAnonymousUserId();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();