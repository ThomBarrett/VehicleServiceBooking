using VehicleServiceBooking.Application.DTOs;
using VehicleServiceBooking.Application.Services.Interfaces;
using VehicleServiceBooking.Data.Repository.Interfaces;
using AutoMapper;
using VehicleServiceBooking.Application.DTOs.ServiceType;

namespace VehicleServiceBooking.Application.Services;

public class ServiceTypeService(IServiceTypeRepository repo, IMapper mapper) : IServiceTypeService
{
    private readonly IServiceTypeRepository _repo = repo;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ServiceTypeDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<ServiceTypeDto>>(items);
    }

    public async Task<ServiceTypeDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return _mapper.Map<ServiceTypeDto?>(entity);
    }
}