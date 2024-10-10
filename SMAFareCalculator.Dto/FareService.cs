using System.Text.Json.Serialization;

namespace SMAFareCalculator.Dto;

public record CalculateTotalFareRequest(IEnumerable<Trip> Trips);
public record Trip(string FromLine, string ToLine, DateTime TripDateTime);

public record CalculateTotalFareResponse(decimal TotalFare)
{
    [JsonIgnore] public int ResponseCode { get; init; }
}
public record TripFare(Trip Trip, decimal FareValue);