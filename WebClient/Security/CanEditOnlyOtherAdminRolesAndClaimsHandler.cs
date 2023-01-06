using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebClient.Security
{
    //Handlers: AuthorizationHandler<T> where T is the Requirement and in this case T is our created Requirement - ManageAdminRolesAndClaimsRequirement
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        //implement handle requirement async method of base AuthorizationHandler
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            //Resource property is return the IActionResult that we are protecting as AuthorizationFilterContext  
            var authFilterContext = context.Resource as AuthorizationFilterContext; //context.Resource return type of Object so cast to type AuthorizationFilterContext
            if (authFilterContext == null)
            {
                return Task.CompletedTask;
            }

            //to retrieve logged in Id use context.User.Claims to get list of User Claims -> get list of NameIdentitfer because it's contain ID of the logged User
            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            //retrieve the Id of the admin Being Edited passed in the URL as a query string parameter by using authFilterContext -> get the request query string "userId"
            string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"];

            //Check if the logged in Id is in role "Admin" AND have ClaimType = "Edit Role"  and ClaimValue = "true" AND if logged in Id != UserId being Edited
            if (context.User.IsInRole("Admin") &&
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
                adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(requirement); //Successfully evaluated the requirement 
            }
            //else 
            //{
            //    context.Fail(); //Failed evaluated the requirement 
            //}

            return Task.CompletedTask;
        }
    }
}
