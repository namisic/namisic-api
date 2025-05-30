/*
API de Nami SIC, la aplicación de código abierto que permite administrar Condominios fácilmente.
Copyright (C) 2025  Oscar David Díaz Fortaleché  lechediaz@gmail.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System.Reflection;
using System.Security.Claims;
using Condominiums.Api.Auth;
using Condominiums.Api.Auth.Handlers;
using Condominiums.Api.Constants;
using Condominiums.Api.Models.DTOs.Residents;
using Condominiums.Api.Options;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ResidentsProfile));

        // Register dependencies dynamically.
        Type[] allClasses = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && (
                t.Name.EndsWith("Store")
                || t.Name.EndsWith("Service")
                || t.Name.EndsWith("Farmer")
                || t.Name.EndsWith("Seed")
            ))
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

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        services.Configure<RoleNameOptions>(configuration.GetSection("RoleNames"));

        string? isServerUrl = configuration.GetValue<string>(Condominiums.Api.Constants.ConfigurationSection.IdServerUrl);

        if (string.IsNullOrEmpty(isServerUrl))
        {
            Console.WriteLine("Please configure the '{0}'.", isServerUrl);
            Environment.Exit(1);
        }

        string? clientId = configuration.GetValue<string>(Condominiums.Api.Constants.ConfigurationSection.ClientId);

        if (string.IsNullOrEmpty(clientId))
        {
            Console.WriteLine("Please configure the '{0}'.", clientId);
            Environment.Exit(1);
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(config =>
        {
            config.Authority = isServerUrl;
            config.MapInboundClaims = false;
            config.RequireHttpsMetadata = true;
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimName.Sub,
                ValidAudiences = new string[] { clientId },
            };

            if (isDevelopment)
            {
                config.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; }
                };
            }

            config.Events = new JwtBearerEvents()
            {
                OnTokenValidated = async tokenValidatedContext =>
                {
                    HttpContext _httpContext = tokenValidatedContext.HttpContext;
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                    ClaimsPrincipal principal = tokenValidatedContext.Principal!;

                    // Get the role claim from user info endpoint.
                    string claimType = ClaimName.Role;
                    if (!principal.HasClaim(claim => claim.Type == claimType))
                    {
                        var client = new HttpClient();
                        var token = tokenValidatedContext.HttpContext.Request.Headers[HeaderNames.Authorization]
                            .ToString()
                            .Substring("Bearer ".Length);
                        var response = await client.GetUserInfoAsync(new UserInfoRequest
                        {
                            Address = $"{isServerUrl}/userinfo",
                            Token = token
                        });

                        if (!response.IsError)
                        {
                            List<Claim> claims = response.Claims.ToList();
                            foreach (var roleClaim in claims.Where(claim => claim.Type == claimType))
                            {
                                claimsIdentity.AddClaim(new Claim(claimType, roleClaim.Value));
                            }
                        }
                    }

                    principal.AddIdentity(claimsIdentity);
                }
            };
        });

        services.AddSingleton<IAuthorizationHandler, RoleHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        return services;
    }
}
