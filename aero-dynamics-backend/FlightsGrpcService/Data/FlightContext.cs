using Microsoft.EntityFrameworkCore;
using FlightsGrpcService.Entities;

namespace FlightsGrpcService.Data
{
    public class FlightContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }

        public FlightContext(DbContextOptions<FlightContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>().HasData(
                new Flight { FlightId = 100, AircraftRegistrationNo = "AUD12", Destination = "Adelaide", NumberOfPassengers = 8, Note = "test" },
                new Flight { FlightId = 101, AircraftRegistrationNo = "AUD13", Destination = "Melbourne", NumberOfPassengers = 50, Note = "" },
                new Flight { FlightId = 102, AircraftRegistrationNo = "AUD13", Destination = "Melbourne", NumberOfPassengers = 27, Note = "test" },
                new Flight { FlightId = 103, AircraftRegistrationNo = "NZD11", Destination = "Auckland", NumberOfPassengers = 18, Note = "test" }
            );
        }

    }
}
