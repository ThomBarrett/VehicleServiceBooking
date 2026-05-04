using AutoMapper;
using VehicleServiceBooking.Application.DTOs;
using VehicleServiceBooking.Application.DTOs.Appointment;
using VehicleServiceBooking.Application.DTOs.Appointments;
using VehicleServiceBooking.Application.DTOs.ServiceType;
using VehicleServiceBooking.Data.Models;

namespace VehicleServiceBooking.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ServiceType, ServiceTypeDto>();
            
        CreateMap<Appointment, AppointmentDto>();
        CreateMap<AppointmentRequestBase, Appointment>();
        CreateMap<CreateAppointmentRequest, Appointment>().IncludeBase<AppointmentRequestBase, Appointment>();
        CreateMap<UpdateAppointmentRequest, Appointment>().IncludeBase<AppointmentRequestBase, Appointment>();

    }
}