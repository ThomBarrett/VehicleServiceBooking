using AutoMapper;
using VehicleServiceBooking.Application.DTOs;
using VehicleServiceBooking.Application.DTOs.Appointment;
using VehicleServiceBooking.Application.DTOs.Appointments;
using VehicleServiceBooking.Application.Services.Interfaces;
using VehicleServiceBooking.Data.Models;
using VehicleServiceBooking.Data.Repository.Interfaces;

namespace VehicleServiceBooking.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IServiceTypeRepository _serviceTypeRepo;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepo, IServiceTypeRepository serviceTypeRepo,  IMapper mapper)
    {
        _appointmentRepo = appointmentRepo;
        _serviceTypeRepo = serviceTypeRepo;
        _mapper = mapper;
    }
    
    public async Task<List<AppointmentDto>> GetAllAsync(int page, int pageSize)
    {
        var items = await _appointmentRepo.GetAllAsync(page, pageSize);
        return _mapper.Map<List<AppointmentDto>>(items);
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var entity = await _appointmentRepo.GetByIdAsync(id);
        return _mapper.Map<AppointmentDto?>(entity);
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentRequest request)
    {
        await ValidateBusinessRulesAsync(request);

        var entity = _mapper.Map<Appointment>(request);
        await _appointmentRepo.CreateAsync(entity);

        return _mapper.Map<AppointmentDto>(entity);
    }

    public async Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentRequest request)
    {
        var existing = await _appointmentRepo.GetByIdAsync(id);
        if (existing is null)
            return null;

        await ValidateBusinessRulesAsync(request, id);

        _mapper.Map(request, existing);
        await _appointmentRepo.UpdateAsync(existing);

        return _mapper.Map<AppointmentDto>(existing);
    }

    public async Task<bool> CancelAsync(int id)
    {
        return await _appointmentRepo.DeleteAsync(id);
    }
    
     private async Task ValidateBusinessRulesAsync(AppointmentRequestBase request, int? updatingId = null)
    {
        ValidateVin(request.VehicleVin);
        ValidateBusinessHours(request.ScheduledDate);
        ValidateExactHour(request.ScheduledDate);

        await ValidateServiceTypeExists(request.ServiceTypeId);
        await ValidateSlotNotTaken(request.ServiceTypeId, request.ScheduledDate, updatingId);
    }

    private static void ValidateVin(string vin)
    {
        if (vin.Length != 17)
            throw new ArgumentException("VIN must be exactly 17 characters.");
    }

    private static void ValidateBusinessHours(DateTime date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            throw new ArgumentException("Appointments must be Monday to Friday.");

        var hour = date.Hour;
        if (hour < 8 || hour >= 17)
            throw new ArgumentException("Appointments must be between 08:00 and 17:00.");
    }

    private static void ValidateExactHour(DateTime date)
    {
        if (date.Minute != 0 || date.Second != 0)
            throw new ArgumentException("Appointments must be on the hour (e.g. 09:00).");
    }

    private async Task ValidateServiceTypeExists(int serviceTypeId)
    {
        var exists = await _serviceTypeRepo.GetByIdAsync(serviceTypeId);
        if (exists is null)
            throw new ArgumentException("Service type does not exist.");
    }

    private async Task ValidateSlotNotTaken(int serviceTypeId, DateTime date, int? updatingId)
    {
        var taken = await _appointmentRepo.IsSlotTakenAsync(serviceTypeId, date);

        if (!taken)
            return;
        
        if (updatingId.HasValue)
        {
            var existing = await _appointmentRepo.GetByIdAsync(updatingId.Value);
            if (existing != null &&
                existing.ServiceTypeId == serviceTypeId &&
                existing.ScheduledDate == date)
            {
                return;
            }
        }

        throw new ArgumentException("This appointment slot is already taken.");
    }
}