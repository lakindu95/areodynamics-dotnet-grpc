using FlightsGrpcService.Data;
using FlightsGrpcService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightsGrpcService.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightContext _context;

        public FlightRepository(FlightContext context)
        {
            _context = context;
        }

        public async Task<List<Flight>> GetFlightsAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task UpdateFlightAsync(Flight flight)
        {
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task AddFlightAsync(Flight flight)
        {
            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();
        }
    }
}
