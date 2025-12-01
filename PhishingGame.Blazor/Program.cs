using Radzen;
using PhishingGame.Blazor.Components;
using PhishingGame.Blazor.States;
using PhishingGame.Core;
using Microsoft.EntityFrameworkCore;
using PhishingGame.Core.Models;
using PhishingGame.Data;

using PhishingGame.Blazor;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddGameStates()
    .AddDbContext<PhishingDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhishingDbContext>();
    db.Database.Migrate(); 


}



app.Run();
