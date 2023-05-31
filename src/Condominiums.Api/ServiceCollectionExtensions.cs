using System.Reflection;
using Condominiums.Api.Models.DTOs.Residents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
        services.AddAutoMapper(typeof(ResidentsProfile));

        // Register dependencies dynamically.
        Type[] allClasses = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && (t.Name.EndsWith("Store") || t.Name.EndsWith("Service")))
            .ToArray();

        foreach (Type classType in allClasses)
        {
            string interfaceName = $"I{classType.Name}";
            Type? interfaceType = classType.GetInterface(interfaceName);
            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, classType);
            }
        }

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        string? isServerUrl = configuration.GetValue<string>("IdServerUrl");

        if (string.IsNullOrEmpty(isServerUrl))
        {
            Console.WriteLine("Please configure the IdServerUrl.");
            Environment.Exit(1);
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(config =>
        {
            config.Authority = isServerUrl;
            config.MapInboundClaims = false;
            config.RequireHttpsMetadata = true;
            config.SaveToken = true;
        });

        services.AddAuthorization(config =>
        {
            config.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}
