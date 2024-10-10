using System.Globalization;
using System.Net;
using SMAFareCalculator.Dto;
using SMAFareCalculator.Service.Interface;

namespace SMAFareCalculator.API;

public static class FareCalculationEndpoints
{
    internal const string TotalFareEndPoint = "/sma/total-fare";
    
    public static IEndpointRouteBuilder RegisterFareEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(TotalFareEndPoint, GetTotalFare);
        return app;
    }
    
    private static async Task<IResult> GetTotalFare(TotalFareEndpointRequest request, IFareService service)
    {
        var fareServiceRequest = request.ToCalculateTotalFareRequest();
        if (fareServiceRequest is null)
            return TypedResults.BadRequest(UserFacingMessages.BadRequestMessage);
        
        var result = await service.CalculateFare(fareServiceRequest);
        if (result.ResponseCode.Equals(HttpStatusCode.InternalServerError))
            return TypedResults.UnprocessableEntity();
            
        return TypedResults.Ok(result);
    }
}

static class FareCalculationEndpointExtn
{
    public static CalculateTotalFareRequest? ToCalculateTotalFareRequest(this TotalFareEndpointRequest request)
    {
        try
        {
            var trips = request.Trips
                .Split([Environment.NewLine, ";", "\n", "\r"], StringSplitOptions.RemoveEmptyEntries) // Split lines by new line
                .Select(line => line.Split(',')) // Split each line by comma
                .Where(parts => parts.Length == 3)
                .Select(parts => new Trip(
                    parts[0].Trim(),  // FromLine
                    parts[1].Trim(),  // ToLine
                    DateTime.Parse(parts[2].Trim(), null, DateTimeStyles.RoundtripKind) // Parse TripDateTime as DateTime
                ))
                .ToList();

            return new CalculateTotalFareRequest(trips);
        }
        catch
        {
            // ignore all parsing exceptions and return null; the caller needs to handle
            return null;
        }
    }
}