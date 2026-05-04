using VehicleServiceBooking.Application.Services.Interfaces;

namespace VehicleServiceBooking.Api.Endpoints;

public static class ServiceTypeEndpoints
{
    public static void AddServiceTypeEndpoints(this WebApplication app)
    {
        app.MapGet("/api/servicetypes", async (IServiceTypeService service) =>
        {
            try
            {
                var items = await service.GetAllAsync();
                return Results.Ok(items);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        });
        
        app.MapGet("/api/servicetypes/{id:int}", async (int id, IServiceTypeService service) =>
        {
            try
            {
                var item = await service.GetByIdAsync(id);

                if (item is null)
                    return Results.NotFound($"Service type with ID {id} was not found.");

                return Results.Ok(item);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        });
    }
}