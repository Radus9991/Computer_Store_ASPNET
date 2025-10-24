using ApplicationCore.Services;
using ApplicationCore.Models;
using System.Security.Claims;

namespace WebAPI.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService userService;

        public AuthorizationService(IUserService userService)
        {
            this.userService = userService;
        }

        public bool IsAuthorized(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Identity.IsAuthenticated;
        }

        public async Task<User> GetAuthorizedUser(ClaimsPrincipal claimsPrincipal)
        {
            var userEmail = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

            return await userService.GetUser(userEmail);
        }
    }
}
