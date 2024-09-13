using FlightsGrpcService.Protos;
using Grpc.Core;

namespace FlightsGrpcService.Services
{
    public interface IFlightService
    {
        Task<Flights> GetFlightList(Empty request, ServerCallContext context);
        Task<FlightDetail> GetFlight(GetFlightDetailRequest request, ServerCallContext context);
        Task<Flights> AddFlights(AddFlightDetailsRequest request, ServerCallContext context);
        Task<FlightDetail> UpdateFlight(UpdateFlightDetailRequest request, ServerCallContext context);
    }
}
