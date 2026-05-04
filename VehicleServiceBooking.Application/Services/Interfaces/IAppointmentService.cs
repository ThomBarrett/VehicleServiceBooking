using VehicleServiceBooking.Application.DTOs;
using VehicleServiceBooking.Application.DTOs.Appointment;
using VehicleServiceBooking.Application.DTOs.Appointments;

namespace VehicleServiceBooking.Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAsync(int page, int pageSize);
    Task<AppointmentDto?> GetByIdAsync(int id);

    Task<AppointmentDto> CreateAsync(CreateAppointmentRequest request);

    Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentRequest request);

    Task<bool> CancelAsync(int id);
}