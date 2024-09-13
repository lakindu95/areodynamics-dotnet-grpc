using FlightsGrpcService.Entities;

namespace FlightsGrpcService.Repositories
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetFlightsAsync();
        Task<Flight> GetFlightByIdAsync(int id);
        Task UpdateFlightAsync(Flight flight);
        Task AddFlightAsync(Flight flight);

    }
}
