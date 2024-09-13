using AreoDynamics.API.Entities;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using FlightsGrpcService.Protos;
using Grpc.Core;
using AreoDynamics.API.Resources;
using CsvHelper;
using Asp.Versioning;

namespace AreoDynamics.API.Controllers.v1
{
    [ApiController]
    //NOTE - Added API versioning for maintanance purposes
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]

    public class FlightController : ControllerBase
    {
        private readonly GrpcChannel _channel;
        private readonly FlightService.FlightServiceClient _flightServiceClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FlightController> _logger;
        public FlightController(IConfiguration configuration, ILogger<FlightController> logger)
        {
            _configuration = configuration;
            _channel =
                GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcSettings:FlightsServiceUrl"));
            _flightServiceClient = new FlightService.FlightServiceClient(_channel);
            _logger = logger;
        }

        [HttpGet("getflights")]
        public async Task<ActionResult<Flights>> GetFlightsAsync()
        {
            try
            {
                var response = await _flightServiceClient.GetFlightListAsync(new Empty { });

                return response;
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, $"RPC exception occurred in GetFlightsAsync");

                // Return a JSON response with the error details
                var errorResponse = new ErrorResponse(ex);
                return StatusCode((int)ex.StatusCode, errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred in GetFlightByIdAsync");

                // Return a generic error response for unexpected exceptions
                var errorResponse = new ErrorResponse("InternalServerError", ex.Message, ex.StackTrace);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("getflight/{flightId}")]
        [ServiceFilter(typeof(FlightIdActionFilterAttribute))]
        public async Task<ActionResult<FlightDetail>> GetFlightByIdAsync(int flightId)
        {
            try
            {
                var request = new GetFlightDetailRequest
                {
                    FlightId = flightId
                };

                var response = await _flightServiceClient.GetFlightAsync(request);

                return response;
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, $"RPC exception occurred in GetFlightByIdAsync for Flight ID: {flightId}");

                // Return a JSON response with the error details
                var errorResponse = new ErrorResponse(ex);
                return StatusCode((int)ex.StatusCode, errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred in GetFlightByIdAsync for Flight ID: {flightId}");

                // Return a generic error response for unexpected exceptions
                var errorResponse = new ErrorResponse("InternalServerError", ex.Message, ex.StackTrace);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("updateflight/{flightId}")]
        [ServiceFilter(typeof(FlightIdActionFilterAttribute))]
        public async Task<ActionResult<FlightDetail>> UpdateFlightAsync(int flightId, Flight flight)
        {
            try
            {
                var flightDetail = new FlightDetail
                {
                    FlightId = flightId,
                    NumberOfPassengers = flight.NumberOfPassengers,
                    Note = flight.Note
                };

                var response = await _flightServiceClient.UpdateFlightAsync(new UpdateFlightDetailRequest()
                {
                    Flight = flightDetail
                });

                return response;
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, $"RPC exception occurred in UpdateFlightAsync for Flight ID: {flight.FlightId}");

                // Return a JSON response with the error details
                var errorResponse = new ErrorResponse(ex);
                return StatusCode((int)ex.StatusCode, errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred in UpdateFlightAsync for Flight ID: {flight.FlightId}");

                // Return a generic error response for unexpected exceptions
                var errorResponse = new ErrorResponse("InternalServerError", ex.Message, ex.StackTrace);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFlightData()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null || file.Length == 0)
                    return BadRequest("File is not selected.");

                var flightsToAdd = new List<FlightDetail>();
                var flightIds = new HashSet<int>(); // To track unique FlightId values


                //NOTE: This part should be moved to domain business logic
                var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<FlightMap>();

                    var records = csv.GetRecords<Flight>().ToList();

                    foreach (var record in records)
                    {
                        if (!flightIds.Add(record.FlightId))
                        {
                            _logger.LogWarning($"Duplicate FlightId found: {record.FlightId}");
                            continue; // Skip duplicate records
                        }

                        flightsToAdd.Add(new FlightDetail
                        {
                            FlightId = record.FlightId,
                            AircraftRegistrationNo = record.AircraftRegistrationNo,
                            Destination = record.Destination,
                            NumberOfPassengers = record.NumberOfPassengers,
                            Note = record.Note
                        });
                    }
                }

                if (!flightsToAdd.Any())
                {
                    return BadRequest("No valid flight data found.");
                }

                var request = new AddFlightDetailsRequest
                {
                    Items = { flightsToAdd }
                };

                var response = await _flightServiceClient.AddFlightsAsync(request);

                return Ok(response);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, $"RPC exception occurred in UploadFlightData");

                // Return a JSON response with the error details
                var errorResponse = new ErrorResponse(ex);
                return StatusCode((int)ex.StatusCode, errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred in UploadFlightData");

                // Return a generic error response for unexpected exceptions
                var errorResponse = new ErrorResponse("InternalServerError", ex.Message, ex.StackTrace);
                return StatusCode(500, errorResponse);
            }
        }

    }
}
