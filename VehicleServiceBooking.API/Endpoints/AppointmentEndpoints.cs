using FluentValidation;
using VehicleServiceBooking.Application.DTOs.Appointment;
using VehicleServiceBooking.Application.DTOs.Appointments;
using VehicleServiceBooking.Application.Services.Interfaces;

namespace VehicleServiceBooking.Api.Endpoints;

public static class AppointmentEndpoints
{
    public static void AddAppointmentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/appointments");
        
        group.MapPost("/", async (
            CreateAppointmentRequest request,
            IValidator<CreateAppointmentRequest> validator,
            IAppointmentService service) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Results.BadRequest(validation.ToDictionary());

            try
            {
                var created = await service.CreateAsync(request);
                return Results.Created($"/appointments/{created.Id}", created);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
        
        group.MapPut("/{id:int}", async (
            int id,
            UpdateAppointmentRequest request,
            IValidator<UpdateAppointmentRequest> validator,
            IAppointmentService service) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Results.BadRequest(validation.ToDictionary());

            try
            {
                var updated = await service.UpdateAsync(id, request);

                if (updated is null)
                    return Results.NotFound($"Appointment with ID {id} was not found.");

                return Results.Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}
