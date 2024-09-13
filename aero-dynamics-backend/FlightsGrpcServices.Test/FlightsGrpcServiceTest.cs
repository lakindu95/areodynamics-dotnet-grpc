using AutoMapper;
using FlightsGrpcService.AutoMapper;
using FlightsGrpcService.Entities;
using FlightsGrpcService.Protos;
using FlightsGrpcService.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moq;


namespace FlightsGrpcService.Tests
{
    public class FlightServiceTests
    {
        private readonly Mock<IFlightRepository> _flightRepositoryMock;
        private readonly Mock<ILogger<Services.FlightService>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly Services.FlightService _flightService;

        public FlightServiceTests()
        {
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _loggerMock = new Mock<ILogger<Services.FlightService>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FlightMapper>();
            });
            _mapper = config.CreateMapper();

            _flightService = new FlightsGrpcService.Services.FlightService(
                _flightRepositoryMock.Object,
                _loggerMock.Object,
                _mapper);
        }

        //NOTE: It would be ideal to have seperate files for each method 
        // Test cases for GetFlightList method
        [Fact]
        public async Task GetFlightList_ReturnsFlights()
        {
            var expectedFlights = new List<Flight>
            {
                new Flight { FlightId = 10, NumberOfPassengers = 100 },
                new Flight { FlightId = 20, NumberOfPassengers = 150 }
            };

            _flightRepositoryMock.Setup(repo => repo.GetFlightsAsync())
                .ReturnsAsync(expectedFlights);


            var result = await _flightService.GetFlightList(new Empty(), It.IsAny<ServerCallContext>());

            Assert.NotNull(result);
            Assert.Equal(expectedFlights.Count, result.Items.Count);
            Assert.Equal(expectedFlights[0].FlightId, result.Items[0].FlightId);
            Assert.Equal(expectedFlights[1].FlightId, result.Items[1].FlightId);
        }

        [Fact]
        public async Task GetFlightList_ThrowsRpcExceptionOnRepositoryError()
        {
            _flightRepositoryMock.Setup(repo => repo.GetFlightsAsync())
                .ThrowsAsync(new Exception("Simulated repository error"));

            await Assert.ThrowsAsync<RpcException>(() => _flightService.GetFlightList(new Empty(), It.IsAny<ServerCallContext>()));
        }

        // Test cases for GetFlight method
        [Fact]
        public async Task GetFlight_ReturnsFlightDetail()
        {
            var flightId = 11;
            var expectedFlight = new Flight { FlightId = flightId, NumberOfPassengers = 100 };

            _flightRepositoryMock.Setup(repo => repo.GetFlightByIdAsync(flightId))
                .ReturnsAsync(expectedFlight);

            var request = new GetFlightDetailRequest { FlightId = flightId };

            var result = await _flightService.GetFlight(request, It.IsAny<ServerCallContext>());

            Assert.NotNull(result);
            Assert.Equal(expectedFlight.FlightId, result.FlightId);
        }

        [Fact]
        public async Task GetFlight_ThrowsRpcExceptionOnNotFoundException()
        {
            var flightId = 1;

            _flightRepositoryMock.Setup(repo => repo.GetFlightByIdAsync(flightId))
                .ReturnsAsync((Flight) null); // Simulating flight not found

            var request = new GetFlightDetailRequest { FlightId = flightId };

            var exception = await Assert.ThrowsAsync<RpcException>(() => _flightService.GetFlight(request, It.IsAny<ServerCallContext>()));
            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
        }

        // Test cases for AddFlights method
        [Fact]
        public async Task AddFlights_ReturnsAddedFlights()
        {
            var request = new AddFlightDetailsRequest
            {
                Items =
                {
                    new FlightDetail { FlightId = 1 },
                    new FlightDetail { FlightId = 2 }
                }
            };

            var flightsToAdd = new List<Flight>();

            _flightRepositoryMock.Setup(repo => repo.AddFlightAsync(It.IsAny<Flight>()))
                .Callback<Flight>(flight => flightsToAdd.Add(flight));

            var result = await _flightService.AddFlights(request, It.IsAny<ServerCallContext>());

            Assert.NotNull(result);
            Assert.Equal(request.Items.Count, result.Items.Count);
            Assert.Equal(request.Items[0].FlightId, result.Items[0].FlightId);
            Assert.Equal(request.Items[1].FlightId, result.Items[1].FlightId);

            Assert.Equal(request.Items.Count, flightsToAdd.Count);
            Assert.Equal(request.Items[0].FlightId, flightsToAdd[0].FlightId);
            Assert.Equal(request.Items[1].FlightId, flightsToAdd[1].FlightId);
        }

        // Test cases for UpdateFlight method
        [Fact]
        public async Task UpdateFlight_ReturnsUpdatedFlightDetail()
        {
            var request = new UpdateFlightDetailRequest
            {
                Flight = new FlightDetail { FlightId = 100, NumberOfPassengers = 150, Note = "Updated note" }
            };

            var existingFlight = new Flight
            {
                FlightId = 100,
                NumberOfPassengers = 100,
                Note = "Old note",
                AircraftRegistrationNo = "XYZ456",
                Destination = "Los Angeles",
                FlightCost = 3000.0
            };

            _flightRepositoryMock.Setup(repo => repo.GetFlightByIdAsync(request.Flight.FlightId))
                .ReturnsAsync(existingFlight);

            _flightRepositoryMock.Setup(repo => repo.UpdateFlightAsync(It.IsAny<Flight>()))
                .Returns(Task.CompletedTask);

            var result = await _flightService.UpdateFlight(request, It.IsAny<ServerCallContext>());

            Assert.NotNull(result);
            Assert.Equal(request.Flight.FlightId, result.FlightId);
            Assert.Equal(request.Flight.NumberOfPassengers, result.NumberOfPassengers);
            Assert.Equal(request.Flight.Note, result.Note);
        }

        [Fact]
        public async Task UpdateFlight_ThrowsRpcExceptionOnNotFoundException()
        {
            var request = new UpdateFlightDetailRequest
            {
                Flight = new FlightDetail { FlightId = 1 }
            };

            _flightRepositoryMock.Setup(repo => repo.GetFlightByIdAsync(request.Flight.FlightId))
                .ReturnsAsync((Flight) null); // Simulating flight not found

            var exception = await Assert.ThrowsAsync<RpcException>(() => _flightService.UpdateFlight(request, It.IsAny<ServerCallContext>()));
            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task UpdateFlight_ThrowsRpcExceptionOnRepositoryError()
        {
            var request = new UpdateFlightDetailRequest
            {
                Flight = new FlightDetail { FlightId = 1 }
            };

            _flightRepositoryMock.Setup(repo => repo.GetFlightByIdAsync(request.Flight.FlightId))
                .ReturnsAsync(new Flight());

            _flightRepositoryMock.Setup(repo => repo.UpdateFlightAsync(It.IsAny<Flight>()))
                .ThrowsAsync(new Exception("Simulated repository error"));

            await Assert.ThrowsAsync<RpcException>(() => _flightService.UpdateFlight(request, It.IsAny<ServerCallContext>()));
        }
    }
}