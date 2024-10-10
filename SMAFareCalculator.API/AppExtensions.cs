using SMAFareCalculator.Repository;
using SMAFareCalculator.Service;

namespace SMAFareCalculator.API;

public static class AppExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRepo().AddService();
        return serviceCollection;
    }
}