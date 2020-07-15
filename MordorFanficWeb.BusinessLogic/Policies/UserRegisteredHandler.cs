using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Policies
{
    public class UserRegisteredHandler : AuthorizationHandler<RegisteredUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RegisteredUserRequirement requirement)
        {
            if (!context.User.HasClaim(claim => claim.Type == "admin" || claim.Type == "user"))
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
