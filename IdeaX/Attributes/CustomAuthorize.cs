using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdeaX.Attributes
{
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly List<string> _roles;
        public CustomAuthorize(params string[] roles) => _roles = roles.ToList();

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 1. Skip nếu có [AllowAnonymous]
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
                return;

            // 2. Kiểm tra authentication
            var user = context.HttpContext.User;
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }

            // 3. Kiểm tra role
            var role = user.Claims.FirstOrDefault(x => x.Type == "RoleName")?.Value;
            if (!_roles.Contains(role))
                context.Result = new ForbidResult();
        }
    }
    
}