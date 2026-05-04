namespace VehicleServiceBooking.Application.DTOs.Appointment;

public abstract class AppointmentRequestBase
{
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string VehicleVin { get; set; } = string.Empty;
    public int ServiceTypeId { get; set; }
    public DateTime ScheduledDate { get; set; }
}