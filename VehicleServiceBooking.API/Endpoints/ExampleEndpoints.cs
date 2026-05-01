namespace VehicleServiceBooking.Api.Endpoints;

public static class ExampleEndpoints
{
    public static void AddExampleEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!")
            .WithName("GetHelloWorld")
            .WithSummary("Returns Hello World!")
            .WithTags("Example");
    }
}