using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleServiceBooking.Data.DbContexts;
using VehicleServiceBooking.Data.Seed;

namespace VehicleServiceBooking.Data;

public static class DataModule
{
    public static void AddDataModule(IServiceCollection services, ConfigurationManager builderConfiguration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlite(builderConfiguration.GetConnectionString("DefaultConnection"))
        );
    }

    public static async Task SeedDb(IServiceScope scope)
    {
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            await db.Database.MigrateAsync();
            await DataSeeder.SeedAsync(db);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error seeding database: " + ex.Message + "");
        }
    }
}