using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.DTO.User;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            this.configuration = configuration;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserAuthDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await userService.AuthUser(dto.Email, dto.Password);
                    return Ok(TokenCreateDTO.of("Correct credentials", CreateToken(user)));
                } catch(InvalidUserException ex)
                {
                    return Unauthorized(new { Error = ex.Message});
                }
            }

            return BadRequest("Incorrect credentials");
        }

        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                {ClaimTypes.Email, user.Email },
                {ClaimTypes.NameIdentifier, user.Id },
                {ClaimTypes.Role, user.Type.ToString() },
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = credentials,
                Claims = claims
            };

            var handler = new JwtSecurityTokenHandler();
            var tokenObj = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(tokenObj);
        }
    }
}
