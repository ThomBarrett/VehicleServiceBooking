using VehicleServiceBooking.Application.DTOs.ServiceType;

namespace VehicleServiceBooking.Application.Services.Interfaces;

public interface IServiceTypeService
{
    Task<IEnumerable<ServiceTypeDto>> GetAllAsync();
    Task<ServiceTypeDto?> GetByIdAsync(int id);
}