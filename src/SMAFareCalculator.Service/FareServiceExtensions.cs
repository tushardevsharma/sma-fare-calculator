using System.Linq.Expressions;
using SMAFareCalculator.Domain;
using SMAFareCalculator.Dto;
using SMAFareCalculator.Service.Interface;

namespace SMAFareCalculator.Service;

public static class FareServiceExtensions
{
    public static CalculateTotalFareResponse InternalServerError(this IFareService service) => new(0) { ResponseCode = (int)ServiceResponse.InternalServerError };
    
    public static CalculateTotalFareResponse Success(this IFareService service, decimal fareValue) => new(fareValue) { ResponseCode = (int)ServiceResponse.OK };

    public static Expression<Func<FareRule, bool>> GetRulesThatMatchFromLine
        (this CalculateTotalFareRequest request, IEnumerable<Line> allLines) =>
        rule => allLines
            .Where(line => request.Trips.Select(fare => fare.FromLine).Contains(line.Name.Trim(), StringComparer.OrdinalIgnoreCase))
            .Select(line => line.Id)
            .Contains(rule.Ref_FromLine);
}