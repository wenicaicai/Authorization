using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuthentication.AuthorizationRequirements
{
    public class CustomRequireClaims : IAuthorizationRequirement
    {
        public CustomRequireClaims(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }

    public class CustomRequireClaimsHandler : AuthorizationHandler<CustomRequireClaims>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, CustomRequireClaims requirement)
        {
            var hasClaim = context.User.Claims.Any(e => e.Type == requirement.ClaimType);
            if (hasClaim)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(
            this AuthorizationPolicyBuilder builder,
            string claimType)
        {
            builder.AddRequirements(new CustomRequireClaims(claimType));
            return builder;
        }
    }
}
