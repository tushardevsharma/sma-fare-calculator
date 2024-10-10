using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SMAFareCalculator.API;
using SMAFareCalculator.Dto;

namespace SMAFareCalculator.Tests;

public class FareCalculatorTests(WebApplicationFactory<Program> _factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Theory]
    [ClassData(typeof(FareCalculatorTripsTestData))]
    public async Task Trips_ReturnsCorrectFare(string trips, decimal expectedFare)
    {
        var client = _factory.CreateClient();
        TotalFareEndpointRequest request = new(trips);

        var response = await client.PostAsJsonAsync(FareCalculationEndpoints.TotalFareEndPoint, request);

        response.EnsureSuccessStatusCode();
        var fareResponse = await response.Content.ReadFromJsonAsync<CalculateTotalFareResponse>();
        fareResponse.Should().NotBeNull();
        fareResponse.TotalFare.Should().Be(expectedFare);
    }
}