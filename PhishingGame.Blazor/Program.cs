using Radzen;
using PhishingGame.Blazor.Components;
using Microsoft.EntityFrameworkCore;
using PhishinGame.Core.Models;
using PhishingGame.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<PhishingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




// Add services to the container.
builder.Services.AddRazorComponents()
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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhishingDbContext>();
    db.Database.Migrate(); // zorgt dat migraties worden toegepast

    if (!db.Emails.Any())
    {
        db.Emails.AddRange(
            new Email
            {
                Sender = "training@veiligmail.nl",
                Subject = "Welkom bij de masterclass phishing",
                Message = "Beste deelnemer,\n\nWelkom bij de masterclass over phishing. Deze mail is een voorbeeld van een legitieme e-mail.\n\nMet vriendelijke groet,\nHet Trainingsteam",
                IsPhishing = false
            },
            new Email
            {
                Sender = "support@bank-veiligheid.com",
                Subject = "Beveilig uw account onmiddellijk",
                Message = "Uw account is tijdelijk vergrendeld. Klik op deze link om het te herstellen.",
                IsPhishing = true
            },
            new Email
            {
                Sender = "hr@bedrijf.nl",
                Subject = "Belangrijk: update uw personeelsgegevens",
                Message = "Hallo, om je salarisbetaling te garanderen, vul a.u.b. je gegevens opnieuw in via het portaal.",
                IsPhishing = true
            }
        );

        db.SaveChanges();
        Console.WriteLine("? Test e-mails toegevoegd aan de database.");
    }
    else
    {
        Console.WriteLine("?? Database bevat al e-mails — geen seeding nodig.");
    }
}

app.Run();
