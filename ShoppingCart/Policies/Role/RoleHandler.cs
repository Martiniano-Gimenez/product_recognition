using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Service.ServiceContracts;

namespace ShoppingCart.Policies.Role
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IRoleLevelService _roleLevelService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public RoleHandler(IRoleLevelService roleLevelService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _roleLevelService = roleLevelService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RoleRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var roleClaim = context.User.FindFirst(c => c.Type == "Role");

            if (httpContext.User is null || !httpContext.User.Identity.IsAuthenticated || roleClaim is null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var controllerActionDescriptor = httpContext.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

            string currentController = controllerActionDescriptor.ControllerName;
            string currentAction = controllerActionDescriptor.ActionName;

            var isAuthorized = _roleLevelService.IsAuthorizedToUseAction(currentController, currentAction);

            if (isAuthorized)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
