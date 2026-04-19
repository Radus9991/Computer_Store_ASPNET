using ApplicationCore.Exceptions;
using ApplicationCore.Services;
using AutoMapper;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO.User;
using WebAPI.Services;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly WebAPI.Services.IAuthorizationService authorizationService;

        public UserController(IUserService userService, IMapper mapper, WebAPI.Services.IAuthorizationService authorizationService)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCreateDTO user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await userService.AddUser(mapper.Map<User>(user), user.Password);
                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (InvalidUserException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserRole()
        {
            var user = await authorizationService.GetAuthorizedUser(User);

            return Ok(new { Role = user.Type.ToString() });
        }
    }
}
