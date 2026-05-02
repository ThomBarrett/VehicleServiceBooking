namespace VehicleServiceBooking.Data.Models;

public class Appointment
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string VehicleVin { get; set; } = null!;
    public int ServiceTypeId { get; set; }
    public ServiceType ServiceType { get; set; } = null!;
    public DateTime ScheduledDate { get; set; }
}
