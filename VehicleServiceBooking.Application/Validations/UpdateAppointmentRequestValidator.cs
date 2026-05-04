using FluentValidation;
using VehicleServiceBooking.Application.DTOs.Appointment;
using VehicleServiceBooking.Application.DTOs.Appointments;

namespace VehicleServiceBooking.Application.Validations;

public class UpdateAppointmentRequestValidator : AbstractValidator<UpdateAppointmentRequest>
{
    public UpdateAppointmentRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty();

        RuleFor(x => x.VehicleVin)
            .Length(17)
            .WithMessage("VIN must be exactly 17 characters long.");

        RuleFor(x => x.ScheduledDate)
            .GreaterThan(_ => DateTime.UtcNow)
            .WithMessage("Scheduled date must be in the future.");
    }
}