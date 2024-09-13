using AutoMapper;
using Grpc.Core;
using FlightsGrpcService.Entities;
using FlightsGrpcService.Protos;
using FlightsGrpcService.Repositories;
using FlightServices = FlightsGrpcService.Protos.FlightService;
using FlightsGrpcService.Exceptions;

namespace FlightsGrpcService.Services
{
    public class FlightService : FlightServices.FlightServiceBase, IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ILogger<FlightService> _logger;
        private readonly IMapper _mapper;

        public FlightService(IFlightRepository flightRepository, ILogger<FlightService> logger, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async override Task<Flights> GetFlightList(Empty request, ServerCallContext context)
        {
            try
            {
                var flightData = await _flightRepository.GetFlightsAsync();

                Flights response = new Flights();
                foreach (Flight flight in flightData)
                {
                    response.Items.Add(_mapper.Map<FlightDetail>(flight));
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetFlightList: Unexpected error in GetFlightList method");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        public async override Task<FlightDetail> GetFlight(GetFlightDetailRequest request, ServerCallContext context)
        {
            try
            {
                var flight = await _flightRepository.GetFlightByIdAsync(request.FlightId);

                if (flight == null) 
                {
                    throw new NotFoundException();
                }

                var flightDetail = _mapper.Map<FlightDetail>(flight);
                return flightDetail;
            }

            catch (NotFoundException ex)
            {
                _logger.LogError(ex, $"GetFlight: Flight ID not found: {request.FlightId}");
                throw new RpcException(new Status(StatusCode.NotFound, "Flight ID not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetFlight: Unexpected error in GetFlight method for Flight ID {request.FlightId}");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        public async override Task<Flights> AddFlights(AddFlightDetailsRequest request, ServerCallContext context)
        {
            try
            {
                var flights = new Flights();

                foreach (var flightDetail in request.Items)
                {
                    var flight = _mapper.Map<Flight>(flightDetail);

                    // Perform initial cost calculation based on the number of passengers
                    flight.FlightCost = CalculateCost(flight.NumberOfPassengers);

                    await _flightRepository.AddFlightAsync(flight);

                    flights.Items.Add(_mapper.Map<FlightDetail>(flight));
                }
                return flights;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AddFlights: Unexpected error in AddFlights");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        public async override Task<FlightDetail> UpdateFlight(UpdateFlightDetailRequest request, ServerCallContext context)
        {
            try
            {
                var flight = _mapper.Map<Flight>(request.Flight);

                var flightInDatabase = await _flightRepository.GetFlightByIdAsync(flight.FlightId);

                if (flightInDatabase == null)
                {
                    // Handle case where flight with given FlightId does not exist
                    throw new NotFoundException();
                }

                // Update only the properties that can be edited (NumberOfPassengers and Note)
                flightInDatabase.NumberOfPassengers = flight.NumberOfPassengers;
                flightInDatabase.Note = flight.Note;

                // Recalculate the flight cost based on the updated number of passengers
                flightInDatabase.FlightCost = CalculateCost(flightInDatabase.NumberOfPassengers);

                await _flightRepository.UpdateFlightAsync(flightInDatabase);

                var flightDetail = _mapper.Map<FlightDetail>(flightInDatabase);
                return flightDetail;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, $"UpdateFlight: Flight ID not found: {request.Flight.FlightId}");
                throw new RpcException(new Status(StatusCode.NotFound, "Flight ID not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateFlight: Unexpected error in UpdateFlight method for Flight ID {request.Flight.FlightId}");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }

        }
        //NOTE: Should create constants
        private double CalculateCost(int numberOfPassengers)
        {
            if (numberOfPassengers <= Constants.THRESHOLD_1) return numberOfPassengers * Constants.COST_1;
            if (numberOfPassengers <= Constants.THRESHOLD_2) return numberOfPassengers * Constants.COST_2;
            if (numberOfPassengers <= Constants.THRESHOLD_3) return numberOfPassengers * Constants.COST_3;
            return numberOfPassengers * Constants.COST_4;
        }
    }
}
