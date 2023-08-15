using Condominiums.Api.Auth.Requirements;
using Condominiums.Api.Constants;
using Condominiums.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Condominiums.Api.Auth;

public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider _backupPolicyProvider { get; }
    private RoleNameOptions _roleNameOptions { get; }

    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IOptions<RoleNameOptions> roleNameOptions)
    {
        // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
        // doesn't handle all policies it should fall back to an alternate provider.
        _backupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        _roleNameOptions = roleNameOptions.Value;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        AuthorizationPolicy defaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        return Task.FromResult(defaultPolicy);
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => Task.FromResult<AuthorizationPolicy>(null);
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PolicyPrefix.RoleRequired, StringComparison.OrdinalIgnoreCase))
        {
            string[] roles = policyName.Substring(PolicyPrefix.RoleRequired.Length).Split(',');
            roles = _roleNameOptions.GetType()
                .GetProperties()
                .Where(p => roles.Contains(p.Name) && p.GetValue(_roleNameOptions) != null)
                .Select(p => (string)p.GetValue(_roleNameOptions)!)
                .ToArray();
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
            policy.AddRequirements(new RoleRequirement(roles));
            return Task.FromResult(policy.Build());
        }

        return _backupPolicyProvider.GetPolicyAsync(policyName);
    }
}
