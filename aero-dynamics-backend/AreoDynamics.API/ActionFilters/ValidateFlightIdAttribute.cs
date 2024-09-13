using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class FlightIdActionFilterAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.TryGetValue("flightId", out object flightIdObj))
        {
            if (flightIdObj is int flightId)
            {
                if (flightId < 0)
                {
                    context.Result = new BadRequestObjectResult("FlightId must be positive.");
                    return;
                }

                Console.WriteLine($"FlightId {flightId} accessed.");

                await next();
            }
            else
            {
                context.Result = new BadRequestObjectResult("Invalid FlightId type.");
            }
        }
        else
        {
            context.Result = new BadRequestObjectResult("FlightId is required.");
        }
    }
}
