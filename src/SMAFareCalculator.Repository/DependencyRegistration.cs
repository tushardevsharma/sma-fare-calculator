using Microsoft.Extensions.DependencyInjection;
using SMAFareCalculator.Repository.Interface;

namespace SMAFareCalculator.Repository;

public static class DependencyRegistration
{
    public static IServiceCollection AddRepo(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IRepository, InMemoryRepository>();
        return serviceCollection;
    }
}