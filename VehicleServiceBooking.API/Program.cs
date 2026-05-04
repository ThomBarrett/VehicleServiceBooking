using VehicleServiceBooking.Api;
using VehicleServiceBooking.Api.Endpoints;
using VehicleServiceBooking.Application;
using VehicleServiceBooking.Application.Mappings;
using VehicleServiceBooking.Application.Services;
using VehicleServiceBooking.Application.Services.Interfaces;
using VehicleServiceBooking.Data;
using VehicleServiceBooking.Data.Repository;
using VehicleServiceBooking.Data.Repository.Interfaces;
using FluentValidation;
using VehicleServiceBooking.Application.DTOs.Appointments;
using VehicleServiceBooking.Application.Validations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();
builder.Services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


builder.Services.AddValidatorsFromAssembly(typeof(CreateAppointmentRequest).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(UpdateAppointmentRequest).Assembly);

// var appAssembly = AppDomain.CurrentDomain
//     .GetAssemblies()
//     .First(a => a.GetName().Name == "VehicleServiceBooking.Application");
//
// builder.Services.AddValidatorsFromAssembly(appAssembly);



var provider = builder.Services.BuildServiceProvider();
var test = provider.GetService<IValidator<CreateAppointmentRequest>>();

Console.WriteLine(test == null ? "Validator NOT registered" : "Validator IS registered");


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