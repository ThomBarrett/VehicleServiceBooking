using Swashbuckle.AspNetCore.SwaggerUI;
using VehicleServiceBooking.Api;
using VehicleServiceBooking.Api.Endpoints;
using VehicleServiceBooking.Application;
using VehicleServiceBooking.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ApiModule.AddApiModule(builder);
ApplicationModule.AddApplicationModule(builder.Services, builder.Configuration);
DataModule.AddDataModule(builder.Services, builder.Configuration);

var app = builder.Build();

await DataModule.SeedDb(app.Services.CreateScope());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/openapi/v1.json", "VehicleServiceBooking.API v1");
        opt.DisplayOperationId();
    });
}

app.UseHttpsRedirection();

app.AddExampleEndpoints();
app.AddServiceTypeEndpoints();
app.AddAppointmentEndpoints();

app.Run();