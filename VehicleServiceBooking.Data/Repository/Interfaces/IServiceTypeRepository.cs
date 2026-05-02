using VehicleServiceBooking.Data.Models;

namespace VehicleServiceBooking.Data.Repository.Interfaces;

/**
 * <summary>Interface for <see cref="ServiceTypeRepository"/></summary>
 * <remarks>Defines methods for interacting with ServiceType entities</remarks>
 */
public interface IServiceTypeRepository
{
    Task<List<ServiceType>> GetAllAsync();
    Task<ServiceType?> GetByIdAsync(int id);
}