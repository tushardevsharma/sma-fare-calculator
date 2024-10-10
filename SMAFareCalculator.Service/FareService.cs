using SMAFareCalculator.Domain;
using SMAFareCalculator.Dto;
using SMAFareCalculator.Repository.Interface;
using SMAFareCalculator.Service.Interface;

namespace SMAFareCalculator.Service;

public class FareService(IRepository _repo) : IFareService
{
    public async Task<CalculateTotalFareResponse> CalculateFare(CalculateTotalFareRequest request)
    {
        var allLines = await GetAllLines();
        if (!allLines.Any())
            return this.InternalServerError();
        
        var fareRulesThatMatchFromLine = await _repo.Search(request.GetRulesThatMatchFromLine(allLines));
        if (!fareRulesThatMatchFromLine.Any())
            return this.InternalServerError();

        var allPeakHours = await GetPeakHours();
        return this.Success(GetTotalFare(request.Trips, fareRulesThatMatchFromLine, allLines, allPeakHours));
    }

    private static decimal GetTotalFare(IEnumerable<Trip> trips,
        IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines,
        IEnumerable<PeakHour> allPeakHours)
    {
        var allTripFares = trips.Select(trip => trip.CalculateFare(rulesThatMatchFromLine, allLines, allPeakHours));
        return allTripFares
            .ApplyDailyCap(rulesThatMatchFromLine, allLines)
            .ApplyWeeklyCap(rulesThatMatchFromLine, allLines)
            .Sum(fareByTrip => fareByTrip.FareValue);
    }

    private async Task<IEnumerable<Line>> GetAllLines()
        => await _repo.Search<Line>(_ => true);

    private async Task<IEnumerable<PeakHour>> GetPeakHours()
        => await _repo.Search<PeakHour>(_ => true);
}