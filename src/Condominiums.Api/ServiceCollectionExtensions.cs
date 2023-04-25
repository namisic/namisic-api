using Condominiums.Api.Stores;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<IResidentStore, ResidentStore>();
        return services;
    }
}
