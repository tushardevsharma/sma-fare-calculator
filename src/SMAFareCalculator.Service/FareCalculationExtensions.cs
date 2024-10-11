using SMAFareCalculator.Domain;
using SMAFareCalculator.Dto;

namespace SMAFareCalculator.Service;

public static class FareCalculationExtensions
{
    public static TripFare CalculateFare(this Trip trip, IEnumerable<FareRule> allRules, IEnumerable<Line> allLines,
        IEnumerable<PeakHour> peakHours)
    {
        var rules = GetFareRules(trip, allRules, allLines);

        return IsTripInPeakHour(trip.TripDateTime, peakHours)
            ? new TripFare(trip, rules.PeakFare)
            : new TripFare(trip, rules.NonPeakFare);
    }

    public static IEnumerable<TripFare> AggregateFareWithDailyCap(this IEnumerable<TripFare> allTripFares, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines)
    {
        return allTripFares
            .GroupBy(tripFare => DateOnly.FromDateTime(tripFare.Trip.TripDateTime)) // segregate fares for each date/day
            .SelectMany(group => group.ToList().GetAggregateFare(rulesThatMatchFromLine, allLines, rule => rule.DailyCap));
    }
    
    public static IEnumerable<TripFare> AggregateFareWithWeeklyCap(this IEnumerable<TripFare> allFaresByTrip, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines)
    {
        var startDate = allFaresByTrip.Select(fareByTrip => fareByTrip.Trip.TripDateTime).Order().First();
        return allFaresByTrip
            .GroupBy(fareByDate => (fareByDate.Trip.TripDateTime - startDate).Days / 7) // create "rolling weeks" from the first trip date 
            .SelectMany(group => group.ToList().GetAggregateFare(rulesThatMatchFromLine, allLines, rule => rule.WeeklyCap));
    }

    private static IEnumerable<TripFare> GetAggregateFare(this IEnumerable<TripFare> tripFares,
        IEnumerable<FareRule> rulesThatMatchFromLine, IEnumerable<Line> allLines, Func<FareRule, decimal> fareCapAction)
    {
        return tripFares
            .GroupBy(tripFare => (tripFare.Trip.FromLine, tripFare.Trip.ToLine)) // segregate based on specific [fromLine, toLine] pairs
            .Select(group => group.ToList().GetAggregateFarePerLinePair(rulesThatMatchFromLine, allLines, fareCapAction));
    }

    private static TripFare GetAggregateFarePerLinePair(
        this IEnumerable<TripFare> tripFares,
        IEnumerable<FareRule> rulesThatMatchFromLine, IEnumerable<Line> allLines,
        Func<FareRule, decimal> fareCapAction)
    {
        return tripFares
            .Aggregate<TripFare, TripFare>(null!,
                (tripFareSoFar, tripFare) =>
                    AggregateFares(tripFareSoFar, tripFare, rulesThatMatchFromLine, allLines, fareCapAction));
    }

    private static TripFare AggregateFares(TripFare? aggregateFareByTrip, TripFare newTripFare,
        IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines, Func<FareRule, decimal> fareCapAction)
    {
        aggregateFareByTrip ??= newTripFare with { FareValue = 0 };
        
        var matchingRule = GetFareRules(newTripFare.Trip, rulesThatMatchFromLine, allLines);
        var newAggregateFareByTrip = aggregateFareByTrip with
        {
            FareValue = Math.Min(fareCapAction(matchingRule), aggregateFareByTrip.FareValue + newTripFare.FareValue)
        };
        
        return newAggregateFareByTrip;
    }
    
    private static FareRule GetFareRules(Trip trip, IEnumerable<FareRule> allRules, IEnumerable<Line> allLines)
    {
        var fareRules = allRules.Where(rule => rule.TripMatchesRule(trip, allLines));
        return fareRules.First();
    }

    private static int GetLineId(string targetLineName, IEnumerable<Line> allLines)
        => allLines
            .Where(line => line.Name.Equals(targetLineName.Trim(), StringComparison.OrdinalIgnoreCase))
            .Select(line => line.Id).First();

    private static bool IsTripInPeakHour(DateTime tripDateTime, IEnumerable<PeakHour> peakHours)
        => peakHours.Any(peakHour =>
            peakHour.DayOfWeek == tripDateTime.DayOfWeek && 
            TimeOnly.FromDateTime(tripDateTime) >= peakHour.FromTime &&
            TimeOnly.FromDateTime(tripDateTime) <= peakHour.ToTime);

    // In the future filter based on more parameters like from/to stations, age-range, gender, etc.
    private static bool TripMatchesRule(this FareRule rule, Trip trip, IEnumerable<Line> allLines)
        => rule.Ref_FromLine == GetLineId(trip.FromLine, allLines) && 
           rule.Ref_ToLine == GetLineId(trip.ToLine, allLines);
}