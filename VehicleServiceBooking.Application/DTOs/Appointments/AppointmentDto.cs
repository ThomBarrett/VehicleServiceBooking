using VehicleServiceBooking.Application.DTOs.ServiceType;

namespace VehicleServiceBooking.Application.DTOs;

public class AppointmentDto
{ 
    public int Id { get; set; } 
    public string CustomerName { get; set; } 
    public string Email { get; set; } 
    public string Phone { get; set; } 
    public string VehicleVin { get; set; } 
    public ServiceTypeDto ServiceType { get; set; } 
    public DateTime ScheduledDate { get; set; }
}