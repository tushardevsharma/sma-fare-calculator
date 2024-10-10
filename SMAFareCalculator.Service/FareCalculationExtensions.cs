using SMAFareCalculator.Domain;
using SMAFareCalculator.Dto;

namespace SMAFareCalculator.Service;

public static class FareCalculationExtensions
{
    public static TripFare CalculateFare(this Trip trip, IEnumerable<FareRule> allRules, IEnumerable<Line> allLines,
        IEnumerable<PeakHour> peakHours)
    {
        var rules = GetFareRules(trip.FromLine, trip.ToLine, allRules, allLines);

        return IsTripInPeakHour(trip.TripDateTime, peakHours)
            ? new TripFare(trip, rules.PeakFare)
            : new TripFare(trip, rules.NonPeakFare);
    }

    public static IEnumerable<TripFare> ApplyDailyCap(this IEnumerable<TripFare> allTripFares, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines)
    {
        var a = allTripFares
            .GroupBy(tripFare => DateOnly.FromDateTime(tripFare.Trip.TripDateTime)) // segregate fares for each date/day
            .SelectMany(group => group.ToList().GetCappedFares(rulesThatMatchFromLine, allLines, rule => rule.DailyCap));

        return a;
    }
    
    public static IEnumerable<TripFare> ApplyWeeklyCap(this IEnumerable<TripFare> allFaresByTrip, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines)
    {
        var startDate = allFaresByTrip.Select(fareByTrip => fareByTrip.Trip.TripDateTime).Order().First();
        return allFaresByTrip
            .GroupBy(fareByDate => (fareByDate.Trip.TripDateTime - startDate).Days / 7) // create "rolling weeks" from the first trip date 
            .SelectMany(group => group.ToList().GetCappedFares(rulesThatMatchFromLine, allLines, rule => rule.WeeklyCap));
    }

    private static IEnumerable<TripFare> GetCappedFares(this IEnumerable<TripFare> tripFares,
        IEnumerable<FareRule> rulesThatMatchFromLine, IEnumerable<Line> allLines, Func<FareRule, decimal> fareCapAction)
    {
        return tripFares
            .GroupBy(tripFare => (tripFare.Trip.FromLine, tripFare.Trip.ToLine))
            .Select(group => group.ToList().GetLinePairCappedFare(rulesThatMatchFromLine, allLines, fareCapAction));
    }

    private static TripFare GetLinePairCappedFare(
        this IEnumerable<TripFare> tripFares,
        IEnumerable<FareRule> rulesThatMatchFromLine, IEnumerable<Line> allLines,
        Func<FareRule, decimal> fareCapAction)
    {
        return tripFares
            .Aggregate<TripFare, TripFare>(null!,
                (tripFareSoFar, tripFare) =>
                    DailyFareAccumulator(tripFareSoFar, tripFare, rulesThatMatchFromLine, allLines, fareCapAction));
    }

    private static TripFare DailyFareAccumulator(TripFare? aggregateFareByTrip, TripFare tripFare, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines, Func<FareRule, decimal> fareCapAction)
    {
        aggregateFareByTrip ??= tripFare with { FareValue = 0 };
        
        var matchingRule = GetFareRules(tripFare.Trip.FromLine, tripFare.Trip.ToLine, rulesThatMatchFromLine, allLines);
        var newAggregateFareByTrip = aggregateFareByTrip with
        {
            FareValue = Math.Min(fareCapAction(matchingRule), aggregateFareByTrip.FareValue + tripFare.FareValue)
        };
        
        return newAggregateFareByTrip;
    }
    
    private static FareRule GetFareRules(string fromLine, string toLine, IEnumerable<FareRule> allRules, IEnumerable<Line> allLines)
    {
        var fareRules = allRules.Where(rule =>
            rule.Ref_FromLine == GetLineId(fromLine, allLines) && rule.Ref_ToLine == GetLineId(toLine, allLines));
        return fareRules.First();
    }

    private static int GetLineId(string targetLineName, IEnumerable<Line> allLines)
        => allLines.Where(line => line.Name.Equals(targetLineName.Trim(), StringComparison.OrdinalIgnoreCase)).Select(line => line.Id).First();

    private static bool IsTripInPeakHour(DateTime tripDateTime, IEnumerable<PeakHour> peakHours)
        => peakHours.Any(peakHour =>
            peakHour.DayOfWeek == tripDateTime.DayOfWeek && 
            TimeOnly.FromDateTime(tripDateTime) >= peakHour.FromTime &&
            TimeOnly.FromDateTime(tripDateTime) <= peakHour.ToTime);
}