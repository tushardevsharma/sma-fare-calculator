using SMAFareCalculator.Domain;
using SMAFareCalculator.Dto;

namespace SMAFareCalculator.Service;

public static class FareCalculationExtensions
{
    public static FareByTrip CalculateFare(this Trip trip, IEnumerable<FareRule> allRules, IEnumerable<Line> allLines,
        IEnumerable<PeakHour> peakHours)
    {
        var rules = GetFareRules(trip.FromLine, trip.ToLine, allRules, allLines);

        return IsTripInPeakHour(trip.TripDateTime, peakHours)
            ? new FareByTrip(rules.PeakFare, trip)
            : new FareByTrip(rules.NonPeakFare, trip);
    }

    public static IEnumerable<FareByTrip> ApplyDailyCap(this IEnumerable<FareByTrip> allFaresByTrip, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines)
    {
        var a = allFaresByTrip
            .GroupBy(fareByTrip => DateOnly.FromDateTime(fareByTrip.Trip.TripDateTime)) // segregate fares for each date/day
            .SelectMany(group => group.ToList().GetCappedFares(rulesThatMatchFromLine, allLines, rule => rule.DailyCap));

        return a;
    }
    
    public static IEnumerable<FareByTrip> ApplyWeeklyCap(this IEnumerable<FareByTrip> allFaresByTrip, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines)
    {
        var startDate = allFaresByTrip.Select(fareByTrip => fareByTrip.Trip.TripDateTime).Order().First();
        return allFaresByTrip
            .GroupBy(fareByDate => (fareByDate.Trip.TripDateTime - startDate).Days / 7) // create "rolling weeks" from the first trip date 
            .SelectMany(group => group.ToList().GetCappedFares(rulesThatMatchFromLine, allLines, rule => rule.WeeklyCap));
    }

    private static IEnumerable<FareByTrip> GetCappedFares(this IEnumerable<FareByTrip> faresByTripPerDay,
        IEnumerable<FareRule> rulesThatMatchFromLine, IEnumerable<Line> allLines, Func<FareRule, decimal> fareCapAction)
    {
        return faresByTripPerDay
            .GroupBy(fareByTripPerDay => (fareByTripPerDay.Trip.FromLine, fareByTripPerDay.Trip.ToLine))
            .Select(group => group.ToList().GetLinePairCappedFare(rulesThatMatchFromLine, allLines, fareCapAction));
    }

    private static FareByTrip GetLinePairCappedFare(
        this IEnumerable<FareByTrip> faresByTripPerDay,
        IEnumerable<FareRule> rulesThatMatchFromLine, IEnumerable<Line> allLines,
        Func<FareRule, decimal> fareCapAction)
    {
        return faresByTripPerDay
            .Aggregate<FareByTrip, FareByTrip>(null!,
                (fareByTripPerDaySoFar, fareByTripPerDay) =>
                    DailyFareAccumulator(fareByTripPerDaySoFar, fareByTripPerDay, rulesThatMatchFromLine, allLines, fareCapAction));
    }

    private static FareByTrip DailyFareAccumulator(FareByTrip? aggregateFareByTrip, FareByTrip fareByTrip, IEnumerable<FareRule> rulesThatMatchFromLine,
        IEnumerable<Line> allLines, Func<FareRule, decimal> fareCapAction)
    {
        aggregateFareByTrip ??= fareByTrip with { FareValue = 0 };
        
        var matchingRule = GetFareRules(fareByTrip.Trip.FromLine, fareByTrip.Trip.ToLine, rulesThatMatchFromLine, allLines);
        var newAggregateFareByTrip = aggregateFareByTrip with
        {
            FareValue = Math.Min(fareCapAction(matchingRule), aggregateFareByTrip.FareValue + fareByTrip.FareValue)
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