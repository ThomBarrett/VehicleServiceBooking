using Microsoft.EntityFrameworkCore;
using VehicleServiceBooking.Data.DbContexts;
using VehicleServiceBooking.Data.Models;

namespace VehicleServiceBooking.Data.Factories;

public class AppointmentFactory : FactoryBase<Appointment>
{
    protected override DbSet<Appointment> GetDbSet(DataContext db)
        => db.Appointments;

    public override Appointment Create(dynamic data)
    {
        return new Appointment()
        {
            CustomerName = data.CustomerName,
            Email = data.Email,
            Phone = data.Phone,
            VehicleVin = data.VehicleVin,
            ScheduledDate = data.ScheduledDate
        };
    }
}