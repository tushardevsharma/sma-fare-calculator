namespace SMAFareCalculator.Tests;

internal class FareCalculatorTripsTestData : TheoryData<string, decimal>
    {
        public FareCalculatorTripsTestData()
        {
            this.CaseStudyTestCase();
            this.DailyCappingTestCase();
            this.FaresAcrossDifferentDaysTestCase();
            this.DailyCappingLimitFareTestCase();
            this.WeeklyCappingSingleWeekTestCase();
            this.WeeklyCappingMultipleWeeksTestCase();
        }
    };