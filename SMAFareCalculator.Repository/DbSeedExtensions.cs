using SMAFareCalculator.Domain;
using SMAFareCalculator.Repository.Interface;

namespace SMAFareCalculator.Repository;

public static class DbSeedExtensions
{
    public static async Task InitDb(this IRepository repo)
    {
        // Fake data; in the real world this will come from an actual database
        Line[] metroLines =
        [
            new(001, "Green"),
            new(002, "Red"),
        ];

        FareRule[] fareRules =
        [
            new(001, 001, 001, 2, 1, 8, 55),
            new(002, 002, 002, 3, 2, 12, 70),
            new(003, 001, 002, 4, 3, 15, 90),
            new(004, 002, 001, 3, 2, 15, 90),
        ];

        PeakHour[] peakHours =
        [
            new (001, DayOfWeek.Monday, TimeOnly.Parse("8:00"), TimeOnly.Parse("10:00")),
            new (002, DayOfWeek.Tuesday, TimeOnly.Parse("8:00"), TimeOnly.Parse("10:00")),
            new (002, DayOfWeek.Wednesday, TimeOnly.Parse("8:00"), TimeOnly.Parse("10:00")),
            new (002, DayOfWeek.Thursday, TimeOnly.Parse("8:00"), TimeOnly.Parse("10:00")),
            new (002, DayOfWeek.Friday, TimeOnly.Parse("8:00"), TimeOnly.Parse("10:00")),
            
            new (001, DayOfWeek.Monday, TimeOnly.Parse("16:30"), TimeOnly.Parse("19:00")),
            new (002, DayOfWeek.Tuesday, TimeOnly.Parse("16:30"), TimeOnly.Parse("19:00")),
            new (002, DayOfWeek.Wednesday, TimeOnly.Parse("16:30"), TimeOnly.Parse("19:00")),
            new (002, DayOfWeek.Thursday, TimeOnly.Parse("16:30"), TimeOnly.Parse("19:00")),
            new (002, DayOfWeek.Friday, TimeOnly.Parse("16:30"), TimeOnly.Parse("19:00")),
            
            new (002, DayOfWeek.Saturday, TimeOnly.Parse("10:00"), TimeOnly.Parse("14:00")),
            new (002, DayOfWeek.Saturday, TimeOnly.Parse("18:00"), TimeOnly.Parse("23:00")),
            
            new PeakHour(002, DayOfWeek.Sunday, TimeOnly.Parse("18:00"), TimeOnly.Parse("23:00")),
        ];

        await repo.BulkInsert(metroLines);
        await repo.BulkInsert(fareRules);
        await repo.BulkInsert(peakHours);
        
        // In the real world, we will need two more tables/collections -
        // - Trip: for history of trips for a customer/rider
        // - Customer: for information about a customer/rider (uniquely identified by a social security number/aadhar)
        //             so that daily/weekly caps can be appropriately applied/credited
    }
}