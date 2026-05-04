using VehicleServiceBooking.Application.DTOs;
using VehicleServiceBooking.Application.DTOs.Appointments;
using VehicleServiceBooking.Application.DTOs.ServiceType;
using VehicleServiceBooking.Application.Services;
using VehicleServiceBooking.Data.Models;
using VehicleServiceBooking.Data.Repository.Interfaces;
using AutoMapper;
using Moq;

namespace VehicleServiceBooking.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }
    
    /**
     * <summary>Fails if invalid VIN doesn't throw exception</summary>
     * <remarks>A valid VIN must be 17 characters long</remarks>
     */
    [Test]
    public void CreateAsync_ShouldThrow_WhenVINIsNot17Characters()
    {
        var repo = new Mock<IAppointmentRepository>();
        var serviceTypeRepo = new Mock<IServiceTypeRepository>();
        var mapper = new Mock<IMapper>();

        var service = new AppointmentService(
            repo.Object,
            serviceTypeRepo.Object,
            mapper.Object
        );

        var request = new CreateAppointmentRequest
        {
            CustomerName = "John Doe",
            Email = "john@example.com",
            Phone = "123456789",
            VehicleVin = "123",
            ScheduledDate = DateTime.Today.AddDays(1),
            ServiceTypeId = 1
        };

        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(request)
        );

        Assert.That(ex.Message, Is.EqualTo("VIN must be exactly 17 characters."));
    }
    

    /**
     * <summary>Fails if invalid Slot doesn't throw exception</summary>
     * <remarks>A valid slot must be within business hours (8am-5pm)</remarks>
     */
    [Test]
    public void CreateAsync_ShouldThrow_WhenSlotIsOutsideBusinessHours()
    {
        var repo = new Mock<IAppointmentRepository>();
        var serviceTypeRepo = new Mock<IServiceTypeRepository>();
        var mapper = new Mock<IMapper>();

        var service = new AppointmentService(
            repo.Object,
            serviceTypeRepo.Object,
            mapper.Object
        );

        var request = new CreateAppointmentRequest
        {
            CustomerName = "John Doe",
            Email = "john@example.com",
            Phone = "123456789",
            VehicleVin = "AAAAAAABBBBBBBBBB",
            ScheduledDate = new DateTime(2099, 5, 4, 1, 0, 0),
            ServiceTypeId = 1
        };

        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(request)
        );

        Assert.That(ex.Message, Is.EqualTo("Appointments must be between 08:00 and 17:00."));
    }

    /**
     * <summary>Fails if invalid Slot doesn't throw exception</summary>
     * <remarks>A valid slot must be on a weekday</remarks>
     */
    [Test]
    public void CreateAsync_ShouldThrow_WhenSlotIsOnAWeekend()
    {
        var repo = new Mock<IAppointmentRepository>();
        var serviceTypeRepo = new Mock<IServiceTypeRepository>();
        var mapper = new Mock<IMapper>();

        var service = new AppointmentService(
            repo.Object,
            serviceTypeRepo.Object,
            mapper.Object
        );

        var request = new CreateAppointmentRequest
        {
            CustomerName = "John Doe",
            Email = "john@example.com",
            Phone = "123456789",
            VehicleVin = "AAAAAAABBBBBBBBBB",
            ScheduledDate = new DateTime(2099, 5, 3, 16, 0, 0),
            ServiceTypeId = 1
        };

        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(request)
        );

        Assert.That(ex.Message, Is.EqualTo("Appointments must be Monday to Friday."));
    }

    /**
     * <summary>Fails if invalid Slot doesn't throw exception</summary>
     * <remarks>A valid slot must not yet be taken</remarks>
     */
    [Test]
    public void CreateAsync_ShouldThrow_WhenSlotIsAlreadyTaken()
    {
        var repo = new Mock<IAppointmentRepository>();
        var serviceTypeRepo = new Mock<IServiceTypeRepository>();
        var mapper = new Mock<IMapper>();

        repo.Setup(r => r.IsSlotTakenAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        serviceTypeRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ServiceType { Id = 1, Name = "Test" });

        var service = new AppointmentService(
            repo.Object,
            serviceTypeRepo.Object,
            mapper.Object
        );

        var request = new CreateAppointmentRequest
        {
            CustomerName = "John Doe",
            Email = "john@example.com",
            Phone = "123456789",
            VehicleVin = "12345678901234567",
            ScheduledDate = new DateTime(2099, 5, 4, 16, 0, 0),
            ServiceTypeId = 1
        };

        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(request)
        );

        Assert.That(ex.Message, Does.Contain("slot is already taken").IgnoreCase);
    }

    /**
     * <summary>Passes if a valid request returns correctly formed AppointmentDto</summary>
     * <remarks>A valid request should return an AppointmentDto</remarks>
     */
    [Test]
    public async Task CreateAsync_ShouldReturnAppointmentDto_WhenRequestIsValid()
    {
    var repo = new Mock<IAppointmentRepository>();
    var serviceTypeRepo = new Mock<IServiceTypeRepository>();
    var mapper = new Mock<IMapper>();

    var request = new CreateAppointmentRequest
    {
        CustomerName = "John Doe",
        Email = "john@example.com",
        Phone = "123456789",
        VehicleVin = "12345678901234567",
        ScheduledDate = DateTime.UtcNow.Date.AddHours(10),
        ServiceTypeId = 1
    };

    var savedAppointment = new Appointment
    {
        Id = 42,
        CustomerName = request.CustomerName,
        Email = request.Email,
        Phone = request.Phone,
        VehicleVin = request.VehicleVin,
        ScheduledDate = request.ScheduledDate,
        ServiceTypeId = request.ServiceTypeId
    };

    var expectedDto = new AppointmentDto
    {
        Id = 42,
        CustomerName = request.CustomerName,
        Email = request.Email,
        Phone = request.Phone,
        VehicleVin = request.VehicleVin,
        ScheduledDate = request.ScheduledDate,
        ServiceType = new ServiceTypeDto
        {
            Id = request.ServiceTypeId,
            Name = "Test"
        }
    };

    serviceTypeRepo.Setup(r => r.GetByIdAsync(1))
        .ReturnsAsync(new ServiceType { Id = 1, Name = "Test" });

    repo.Setup(r => r.IsSlotTakenAsync(1, request.ScheduledDate))
        .ReturnsAsync(false);

    repo.Setup(r => r.CreateAsync(It.IsAny<Appointment>()))
        .ReturnsAsync(savedAppointment);

    mapper.Setup(m => m.Map<AppointmentDto>(It.IsAny<Appointment>()))
        .Returns(expectedDto);


    var service = new AppointmentService(
        repo.Object,
        serviceTypeRepo.Object,
        mapper.Object
    );

    var result = await service.CreateAsync(request);

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Id, Is.EqualTo(42));
    Assert.That(result.CustomerName, Is.EqualTo(request.CustomerName));
    Assert.That(result.VehicleVin, Is.EqualTo(request.VehicleVin));
    Assert.That(result.ScheduledDate, Is.EqualTo(request.ScheduledDate));
    }

    /**
     * <summary>Should return false when Appointment does not exist</summary>
     * <remarks>Fails if true is returned</remarks>
     */
    [Test]
    public async Task CancelAsync_ShouldReturnFalse_WhenAppointmentDoesNotExist()
    {
        var repo = new Mock<IAppointmentRepository>();
        var serviceTypeRepo = new Mock<IServiceTypeRepository>();
        var mapper = new Mock<IMapper>();

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Appointment)null);

        var service = new AppointmentService(
            repo.Object,
            serviceTypeRepo.Object,
            mapper.Object
        );

        var result = await service.CancelAsync(999);

        Assert.That(result, Is.False);
    }

    /**
     * <summary>Should return true when Appointment exists</summary>
     * <remarks>Fails if false is returned</remarks>
     */
    [Test]
    public async Task CancelAsync_ShouldReturnTrue_WhenAppointmentExists()
    {
        var repo = new Mock<IAppointmentRepository>();
        var serviceTypeRepo = new Mock<IServiceTypeRepository>();
        var mapper = new Mock<IMapper>();

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Appointment
            {
                Id = 123,
                CustomerName = "John Doe",
                Email = "john@example.com",
                Phone = "123456789",
                VehicleVin = "12345678901234567",
                ScheduledDate = DateTime.UtcNow,
                ServiceTypeId = 1
            });
        
        repo.Setup(r => r.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(true);


        var service = new AppointmentService(
            repo.Object,
            serviceTypeRepo.Object,
            mapper.Object
        );

        var result = await service.CancelAsync(123);

        Assert.That(result, Is.True);
    }
}