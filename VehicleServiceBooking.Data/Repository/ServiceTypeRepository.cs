using Microsoft.EntityFrameworkCore;
using VehicleServiceBooking.Data.DbContexts;
using VehicleServiceBooking.Data.Models;
using VehicleServiceBooking.Data.Repository.Interfaces;

namespace VehicleServiceBooking.Data.Repository;

/**
 * <see cref="ServiceTypeRepository"/> Class
 * Implements <see cref="IServiceTypeRepository"/>
 * Has methods for interacting with ServiceType entities:
 * <para>
 * - <see cref="GetAllAsync"/>
 *  <br/>
 * - <see cref="GetByIdAsync"/>
 * </para>
 */
public class ServiceTypeRepository(DataContext db) : IServiceTypeRepository
{
    /**
     * <summary>A private readonly field of type <see cref="DataContext"/></summary>
     */
    private readonly DataContext _db = db;
    
    /**
     * <summary>Returns all ServiceTypes</summary>
     * <returns>A list of <see cref="ServiceType"/>s</returns>
     */
    public Task<List<ServiceType>> GetAllAsync()
        => _db.ServiceTypes.ToListAsync();

    /**
     * <summary>Returns a <see cref="ServiceType"/> by its ID</summary>
     * <param name="id">The ID of the <see cref="ServiceType"/> to find</param>
     * <returns>A <see cref="ServiceType"/> or null if not found</returns>
     */
    public async Task<ServiceType?> GetByIdAsync(int id)
        => await _db.ServiceTypes.FindAsync(id);
}