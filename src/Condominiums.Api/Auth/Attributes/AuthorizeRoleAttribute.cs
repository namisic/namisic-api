using Condominiums.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Condominiums.Api.Auth.Attributes;

/// <summary>
/// Authorization attribute that allows to validate role claim.
/// </summary>
public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    private readonly string _prefix = PolicyPrefix.RoleRequired;

    public AuthorizeRoleAttribute(params string[] roles) => RolesArray = roles;

    public string[] RolesArray
    {
        get
        {
            string[] roles = Policy!.Substring(_prefix.Length).Split(',');
            return roles;
        }
        set
        {
            Policy = $"{_prefix}{string.Join(',', value)}";
        }
    }
}
