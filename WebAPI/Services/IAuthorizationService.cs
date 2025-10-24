using ApplicationCore.Models;
using System.Security.Claims;

namespace WebAPI.Services
{
    public interface IAuthorizationService
    {
        Task<User> GetAuthorizedUser(ClaimsPrincipal claimsPrincipal);
        bool IsAuthorized(ClaimsPrincipal claimsPrincipal);
    }
}