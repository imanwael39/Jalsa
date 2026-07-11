using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jalsa.Infrastructure.Data.Seeding;

/// <summary>
/// Wiring for the development data seeder. Kept free of any ASP.NET Core dependency so the
/// Infrastructure project stays a plain class library — the host calls these on
/// <see cref="IServiceProvider"/>.
/// </summary>
public static class SeedExtensions
{
    /// <summary>Registers the <see cref="DatabaseSeeder"/> in DI.</summary>
    public static IServiceCollection AddDatabaseSeeder(this IServiceCollection services)
    {
        services.AddScoped<DatabaseSeeder>();
        return services;
    }

    /// <summary>
    /// Applies any pending EF migrations and, when <paramref name="seed"/> is true, runs the
    /// idempotent development seeder. Intended to be called once at startup in Development.
    /// </summary>
    public static async Task MigrateAndSeedAsync(
        this IServiceProvider services,
        bool seed,
        CancellationToken ct = default)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var db = sp.GetRequiredService<JalsaDbContext>();
        await db.Database.MigrateAsync(ct);

        if (seed)
        {
            var seeder = sp.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync(ct);
        }
    }
}
