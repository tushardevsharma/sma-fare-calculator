using SMAFareCalculator.Dto;

namespace SMAFareCalculator.Service.Interface;

public interface IFareService
{
    Task<CalculateTotalFareResponse> CalculateFare(CalculateTotalFareRequest request);
}