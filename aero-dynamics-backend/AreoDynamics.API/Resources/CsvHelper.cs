using AreoDynamics.API.Entities;
using CsvHelper.Configuration;

namespace AreoDynamics.API.Resources
{
    public sealed class FlightMap : ClassMap<Flight>
    {
        public FlightMap()
        {
            Map(m => m.FlightId).Index(0);
            Map(m => m.AircraftRegistrationNo).Index(1);
            Map(m => m.Destination).Index(2);
            Map(m => m.NumberOfPassengers).Index(3);
            Map(m => m.Note).Index(4);
        }
    }

}

