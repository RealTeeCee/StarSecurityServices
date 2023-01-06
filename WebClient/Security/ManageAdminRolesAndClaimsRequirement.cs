using Microsoft.AspNetCore.Authorization;

namespace WebClient.Security
{
    public class ManageAdminRolesAndClaimsRequirement : IAuthorizationRequirement //This class contain the requirement to be able to manage admin roles and claims
    {
    }
}
