using Condominiums.Api.Models.DTOs.Residents;
using Condominiums.Api.Services;
using Condominiums.Api.Stores;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ResidentsProfile));

        #region Stores
        services.AddScoped<IResidentStore, ResidentStore>();
        #endregion

        #region Services
        services.AddScoped<IResidentService, ResidentService>();
        #endregion

        return services;
    }
}
