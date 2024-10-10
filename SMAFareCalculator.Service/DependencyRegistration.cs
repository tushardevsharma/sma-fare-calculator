using Microsoft.Extensions.DependencyInjection;
using SMAFareCalculator.Repository.Interface;
using SMAFareCalculator.Service.Interface;

namespace SMAFareCalculator.Service;

public static class DependencyRegistration
{
    public static IServiceCollection AddService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IFareService, FareService>();
        return serviceCollection;
    }
}