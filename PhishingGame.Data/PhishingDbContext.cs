using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhishinGame.Core.Models;
using System.Reflection;

namespace PhishingGame.Data;

/// <summary>
/// Entity Framework database context for storing tables related to the phishing game and authentication thereof.
/// </summary>
/// <typeparam name="TBaseModel">
/// Base class of the targeted models. The generic parameter is used to register these models dynamically.
/// </typeparam>
/// <param name="options">
/// Configuration options for the database context.
/// </param>
public class PhishingDbContext<TBaseModel>(DbContextOptions<PhishingDbContext<TBaseModel>> options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);



        AddModels(builder);
    }

    private void AddModels(ModelBuilder builder)
    {
        var assembly = Assembly.GetAssembly(typeof(TBaseModel))
            ?? throw new NullReferenceException("Assembly containing database models could not be found");

        var types = GetModelTypes(assembly);

        foreach (var type in types)
        {
            builder.Entity(type);
        }
    }

    private IEnumerable<Type> GetModelTypes(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(
                type => 
                type.IsAssignableTo(typeof(TBaseModel)));
    }
}
