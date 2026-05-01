using Microsoft.EntityFrameworkCore;
using VehicleServiceBooking.Data.DbContexts;
using VehicleServiceBooking.Data.Models;

namespace VehicleServiceBooking.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(DataContext context)
    {

        //TODO: Add your data seeds here
        
      
        await context.SaveChangesAsync();
    }
}
