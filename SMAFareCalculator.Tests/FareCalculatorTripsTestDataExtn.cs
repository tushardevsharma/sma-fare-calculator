namespace SMAFareCalculator.Tests;

internal static class FareCalculatorTripsTestDataExtn
{
    public static void CaseStudyTestCase(this FareCalculatorTripsTestData theoryData)
    {
        theoryData.Add("""
                       Green,Green,2021-03-24T07:58:30
                       Green,Red,2021-03-24T09:58:30
                       Red,Red,2021-03-25T11:58:30
                       """,
            7);
    }

    public static void DailyCappingTestCase(this FareCalculatorTripsTestData theoryData)
    {
        theoryData.Add("""
                       Green,Green,2024-10-07T11:58:30
                       Green,Green,2024-10-07T07:58:30
                       Green,Green,2024-10-07T09:58:30
                       Green,Green,2024-10-07T11:58:30
                       Green,Green,2024-10-07T07:58:30
                       Green,Green,2024-10-07T09:58:30
                       Green,Green,2024-10-07T11:58:30
                       """,
            8);
    }

    public static void FaresAcrossDifferentDaysTestCase(this FareCalculatorTripsTestData theoryData)
    {
        theoryData.Add("""
                       Green,Green,2024-10-07T07:30:00
                       Green,Red,2024-10-07T08:30:00
                       Red,Red,2024-10-07T16:45:00
                       Red,Green,2024-10-07T19:15:00
                       Green,Green,2024-10-08T07:59:00
                       Green,Red,2024-10-08T09:45:00
                       Red,Red,2024-10-08T17:45:00
                       """,
            18);
    }

    public static void DailyCappingLimitFareTestCase(this FareCalculatorTripsTestData theoryData)
    {
        // daily capping limiting the total fare to just 24 instead of weekly capping of 55
        theoryData.Add("""
                       Green,Green,2024-10-07T09:58:30
                       Green,Green,2024-10-07T09:58:30
                       Green,Green,2024-10-07T09:58:30
                       Green,Green,2024-10-07T09:58:30
                       Green,Green,2024-10-07T09:58:30
                       Green,Green,2024-10-07T09:58:30

                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30
                       Green,Green,2024-10-08T09:58:30

                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       Green,Green,2024-10-09T09:58:30
                       """,
            24);
    }

    public static void WeeklyCappingSingleWeekTestCase(this FareCalculatorTripsTestData theoryData)
    {
        // single week: weekly capping limiting the fare to 90 instead of 105
        theoryData.Add("""
                       Green,Red,2024-10-07T09:58:30
                       Green,Red,2024-10-07T09:58:30
                       Green,Red,2024-10-07T09:58:30
                       Green,Red,2024-10-07T10:58:30

                       Green,Red,2024-10-08T09:58:30
                       Green,Red,2024-10-08T09:58:30
                       Green,Red,2024-10-08T09:58:30
                       Green,Red,2024-10-08T10:58:30

                       Green,Red,2024-10-09T09:58:30
                       Green,Red,2024-10-09T09:58:30
                       Green,Red,2024-10-09T09:58:30
                       Green,Red,2024-10-09T10:58:30

                       Green,Red,2024-10-10T09:58:30
                       Green,Red,2024-10-10T09:58:30
                       Green,Red,2024-10-10T09:58:30
                       Green,Red,2024-10-10T10:58:30

                       Green,Red,2024-10-11T09:58:30
                       Green,Red,2024-10-11T09:58:30
                       Green,Red,2024-10-11T09:58:30
                       Green,Red,2024-10-11T10:58:30

                       Green,Red,2024-10-12T10:58:30
                       Green,Red,2024-10-12T10:58:30
                       Green,Red,2024-10-12T10:58:30
                       Green,Red,2024-10-12T09:58:30

                       Green,Red,2024-10-13T10:58:30
                       Green,Red,2024-10-13T18:58:30
                       Green,Red,2024-10-13T18:58:30
                       Green,Red,2024-10-13T18:58:30
                       """,
            90);
    }

    public static void WeeklyCappingMultipleWeeksTestCase(this FareCalculatorTripsTestData theoryData)
    {
        // multiple weeks: weekly capping limiting the fare to 180 instead of 210
        theoryData.Add("""
                       Green,Red,2024-10-07T09:58:30
                       Green,Red,2024-10-07T09:58:30
                       Green,Red,2024-10-07T09:58:30
                       Green,Red,2024-10-07T10:58:30

                       Green,Red,2024-10-08T09:58:30
                       Green,Red,2024-10-08T09:58:30
                       Green,Red,2024-10-08T09:58:30
                       Green,Red,2024-10-08T10:58:30

                       Green,Red,2024-10-09T09:58:30
                       Green,Red,2024-10-09T09:58:30
                       Green,Red,2024-10-09T09:58:30
                       Green,Red,2024-10-09T10:58:30

                       Green,Red,2024-10-10T09:58:30
                       Green,Red,2024-10-10T09:58:30
                       Green,Red,2024-10-10T09:58:30
                       Green,Red,2024-10-10T10:58:30

                       Green,Red,2024-10-11T09:58:30
                       Green,Red,2024-10-11T09:58:30
                       Green,Red,2024-10-11T09:58:30
                       Green,Red,2024-10-11T10:58:30

                       Green,Red,2024-10-12T10:58:30
                       Green,Red,2024-10-12T10:58:30
                       Green,Red,2024-10-12T10:58:30
                       Green,Red,2024-10-12T09:58:30

                       Green,Red,2024-10-13T10:58:30
                       Green,Red,2024-10-13T18:58:30
                       Green,Red,2024-10-13T18:58:30
                       Green,Red,2024-10-13T18:58:30

                       Green,Red,2024-10-14T09:58:30
                       Green,Red,2024-10-14T09:58:30
                       Green,Red,2024-10-14T09:58:30
                       Green,Red,2024-10-14T10:58:30

                       Green,Red,2024-10-15T09:58:30
                       Green,Red,2024-10-15T09:58:30
                       Green,Red,2024-10-15T09:58:30
                       Green,Red,2024-10-15T10:58:30

                       Green,Red,2024-10-16T09:58:30
                       Green,Red,2024-10-16T09:58:30
                       Green,Red,2024-10-16T09:58:30
                       Green,Red,2024-10-16T10:58:30

                       Green,Red,2024-10-17T09:58:30
                       Green,Red,2024-10-17T09:58:30
                       Green,Red,2024-10-17T09:58:30
                       Green,Red,2024-10-17T10:58:30

                       Green,Red,2024-10-18T09:58:30
                       Green,Red,2024-10-18T09:58:30
                       Green,Red,2024-10-18T09:58:30
                       Green,Red,2024-10-18T10:58:30

                       Green,Red,2024-10-19T10:58:30
                       Green,Red,2024-10-19T10:58:30
                       Green,Red,2024-10-19T10:58:30
                       Green,Red,2024-10-19T09:58:30

                       Green,Red,2024-10-20T10:58:30
                       Green,Red,2024-10-20T18:58:30
                       Green,Red,2024-10-20T18:58:30
                       Green,Red,2024-10-20T18:58:30
                       """,
            180);
    }
}