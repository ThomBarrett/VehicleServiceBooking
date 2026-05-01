using Microsoft.EntityFrameworkCore;
using VehicleServiceBooking.Data.Models;

namespace VehicleServiceBooking.Data.DbContexts;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions opt) : base(opt)
    {
    }
    
    //TODO: Add DbSet's here
}
