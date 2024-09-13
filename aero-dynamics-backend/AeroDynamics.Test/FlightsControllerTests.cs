using System.Threading.Tasks;
using AreoDynamics.API.Controllers.v1;
using FlightsGrpcService.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AreoDynamics.Tests
{
    public class FlightControllerTests
    {
        private readonly Mock<FlightService.FlightServiceClient> _flightServiceClientMock;
        private readonly Mock<ILogger<FlightController>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly FlightController _controller;

        public FlightControllerTests()
        {
            _flightServiceClientMock = new Mock<FlightService.FlightServiceClient>();
            _loggerMock = new Mock<ILogger<FlightController>>();
            _configurationMock = new Mock<IConfiguration>();

            // Create a mock IConfigurationSection to return the desired value
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("http://localhost:5000");

            // Setup the configuration mock to return the mock section
            _configurationMock.Setup(config => config.GetSection("GrpcSettings:FlightsServiceUrl")).Returns(mockSection.Object);

            _controller = new FlightController(_configurationMock.Object, _loggerMock.Object);
        }

        private AsyncUnaryCall<T> CreateAsyncUnaryCall<T>(T response)
        {
            return new AsyncUnaryCall<T>(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { });
        }

        [Fact]
        public async Task GetFlightsAsync_ReturnsOkResult()
        {
            var expectedResponse = new Flights();
            var asyncUnaryCall = CreateAsyncUnaryCall(expectedResponse);

            _flightServiceClientMock
                .Setup(client => client.GetFlightListAsync(It.IsAny<Empty>(), null, null, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(asyncUnaryCall);

            var result = await _controller.GetFlightsAsync();

            var okResult = Assert.IsType<ActionResult<Flights>>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task GetFlightByIdAsync_ReturnsOkResult()
        {
            var flightId = 100;
            var expectedResponse = new FlightDetail { FlightId = flightId };
            var asyncUnaryCall = CreateAsyncUnaryCall(expectedResponse);

            _flightServiceClientMock
                .Setup(client => client.GetFlightAsync(It.IsAny<GetFlightDetailRequest>(), null, null, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(asyncUnaryCall);

            var result = await _controller.GetFlightByIdAsync(flightId);

            var okResult = Assert.IsType<ActionResult<FlightDetail>>(result);
            Assert.NotNull(okResult.Value);  // Ensure the result is not null
            var flightDetail = Assert.IsType<FlightDetail>(okResult.Value);
            Assert.Equal(flightId, flightDetail.FlightId);
        }

        [Fact]
        public async Task UpdateFlightAsync_ReturnsOkResult()
        {
            var flightId = 100;
            var flight = new API.Entities.Flight { FlightId = flightId, NumberOfPassengers = 100, Note = "Updated Note" };
            var expectedResponse = new FlightDetail { FlightId = flightId, NumberOfPassengers = 100, Note = "Updated Note" };
            var asyncUnaryCall = CreateAsyncUnaryCall(expectedResponse);

            _flightServiceClientMock
                .Setup(client => client.UpdateFlightAsync(It.IsAny<UpdateFlightDetailRequest>(), null, null, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(asyncUnaryCall);

            var result = await _controller.UpdateFlightAsync(flightId, flight);

            // Assert
            var okResult = Assert.IsType<ActionResult<FlightDetail>>(result);
            Assert.NotNull(okResult.Value);  // Ensure the result is not null
            var flightDetail = Assert.IsType<FlightDetail>(okResult.Value);
            Assert.Equal(flightId, flightDetail.FlightId);
            Assert.Equal(flight.NumberOfPassengers, flightDetail.NumberOfPassengers);
            Assert.Equal(flight.Note, flightDetail.Note);
        }
    }
}
