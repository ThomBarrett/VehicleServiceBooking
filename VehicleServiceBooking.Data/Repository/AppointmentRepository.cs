using Microsoft.EntityFrameworkCore;
using VehicleServiceBooking.Data.DbContexts;
using VehicleServiceBooking.Data.Models;
using VehicleServiceBooking.Data.Repository.Interfaces;

namespace VehicleServiceBooking.Data.Repository;

public class AppointmentRepository(DataContext db) : IAppointmentRepository
{
    
    private readonly DataContext _db = db;
    
    public Task<List<Appointment>> GetAllAsync(int page, int pageSize)
    {
        return _db.Appointments
            .Include(x => x.ServiceType)
            .OrderBy(x => x.ScheduledDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
        => await _db.Appointments.FindAsync(id);

    public Task<bool> IsSlotTakenAsync(int serviceTypeId, DateTime scheduledDateTime)
        => _db.Appointments.AnyAsync(x => 
            x.ServiceTypeId == serviceTypeId &&
            x.ScheduledDate == scheduledDateTime);

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment?> UpdateAsync(Appointment appointment)
    {
        var existing = await _db.Appointments.FindAsync(appointment.Id);
        if (existing is null)
            return null;

        _db.Entry(appointment).State = EntityState.Detached;
        _db.Entry(existing).CurrentValues.SetValues(appointment);

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.Appointments.FindAsync(id);
        if (existing is null)
            return false;

        _db.Appointments.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}