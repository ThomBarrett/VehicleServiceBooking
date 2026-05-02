using Microsoft.EntityFrameworkCore;
using VehicleServiceBooking.Data.DbContexts;
using VehicleServiceBooking.Data.Models;

namespace VehicleServiceBooking.Data.Factories;

public class ServiceTypeFactory : FactoryBase<ServiceType>
{
    protected override DbSet<ServiceType> GetDbSet(DataContext db)
        => db.ServiceTypes;
    
    public override ServiceType Create(dynamic data)
    {
        return new ServiceType()
        {
            Name = data.Name,
            Description = data.Description,
            Duration = data.Duration
        };
    }
}