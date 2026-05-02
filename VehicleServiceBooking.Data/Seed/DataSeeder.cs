using VehicleServiceBooking.Data.DbContexts;
using VehicleServiceBooking.Data.Factories;

namespace VehicleServiceBooking.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(DataContext context)
    {

        //TODO: Add your data seeds here
        var serviceTypeFactory = new ServiceTypeFactory();
        
        var serviceType0 = serviceTypeFactory.Create(new { Name = "15 Minute Car Wash",
                                                                Description = "A Car Wash service with a 15 minute duration",
                                                                Duration = 15 });
        
        serviceType0 = await serviceTypeFactory.BuildAsync(context, serviceType0);
        
        if (serviceType0 == null)
        {
            throw new InvalidOperationException("Failed to Build service type");
        }
        
        var appointmentFactory = new AppointmentFactory();
        var appointmentDetails = new
        {
            CustomerName = "Duckford Pond", Email = "person@exmaple.com", Phone = "1234567890", VehicleVin = "1234567890",
            ScheduledDate = new DateTime(2026, 5, 2)
        };
        var appointment0 = appointmentFactory.Create(appointmentDetails);
        
        appointment0.ServiceTypeId = serviceType0.Id;
        
        await appointmentFactory.BuildAsync(context, appointment0);
        
        //await context.SaveChangesAsync();
    }
}
