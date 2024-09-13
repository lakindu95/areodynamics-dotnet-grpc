using AutoMapper;
using FlightsGrpcService.Entities;
using FlightsGrpcService.Protos;

namespace FlightsGrpcService.AutoMapper
{
    public class FlightMapper : Profile
    {
        public FlightMapper()
        {
            CreateMap<FlightDetail, Flight>(MemberList.Source)
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.AircraftRegistrationNo, opt => opt.MapFrom(src => src.AircraftRegistrationNo))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dest => dest.NumberOfPassengers, opt => opt.MapFrom(src => src.NumberOfPassengers))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.FlightCost, opt => opt.MapFrom(src => src.FlightCost))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Flight, FlightDetail>(MemberList.Source)
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.AircraftRegistrationNo, opt => opt.MapFrom(src => src.AircraftRegistrationNo))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dest => dest.NumberOfPassengers, opt => opt.MapFrom(src => src.NumberOfPassengers))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.FlightCost, opt => opt.MapFrom(src => src.FlightCost))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
