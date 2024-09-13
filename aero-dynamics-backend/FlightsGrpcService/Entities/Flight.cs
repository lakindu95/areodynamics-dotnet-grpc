namespace FlightsGrpcService.Entities
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string AircraftRegistrationNo { get; set; }
        public string Destination { get; set; }
        public int NumberOfPassengers { get; set; }
        public string Note { get; set; }
        public double FlightCost { get; set; }
    }
}
