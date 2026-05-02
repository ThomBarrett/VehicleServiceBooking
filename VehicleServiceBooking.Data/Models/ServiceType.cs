namespace VehicleServiceBooking.Data.Models;

public class ServiceType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Duration { get; set; }
}
