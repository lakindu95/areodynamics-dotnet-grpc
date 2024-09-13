using FlightsGrpcService.AutoMapper;
using FlightsGrpcService.Data;
using FlightsGrpcService.Repositories;
using FlightsGrpcService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

builder.Services.AddDbContext<FlightContext>(options =>
           options.UseInMemoryDatabase("FlightsDatabase"));

builder.Services.AddScoped<FlightContext>();

builder.Services
  .AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(FlightMapper));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FlightContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<FlightService>();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
    });
});
app.Run();
