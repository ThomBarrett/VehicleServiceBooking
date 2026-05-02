using VehicleServiceBooking.Data.Models;

namespace VehicleServiceBooking.Data.Repository.Interfaces;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetAllAsync(int page, int pageSize);
    Task<Appointment?> GetByIdAsync(int id);
    Task<bool> IsSlotTakenAsync(int serviceTypeId, DateTime scheduledDateTime);
    Task<Appointment> CreateAsync(Appointment appointment);
    Task<Appointment?> UpdateAsync(Appointment appointment);
    Task<bool> DeleteAsync(int id);
}