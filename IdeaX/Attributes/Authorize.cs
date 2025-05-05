using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdeaX.Attributes
{
    
         public class Authorize : Attribute, IAuthorizationFilter
    {
        public new List<string> _roles;
        public Authorize(params string[] roles)
        {
            _roles = roles.ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated) 
            {
                context.Result = new ForbidResult();
                return;
            }
            else
            {
                var role = user.Claims.FirstOrDefault(x => x.Type == "RoleName")?.Value;
                if (!_roles.Contains(role)) 
                {
                    context.Result = new ForbidResult();
                    return;
                }
                }
            }

        }
    
}
