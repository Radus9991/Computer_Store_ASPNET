using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Redis;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IEmailService emailService;

        public RedisController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        //[HttpPost("send")]
        //public async Task<IActionResult> test()
        //{
        //    await emailService.SendEmail(new EmailMessage
        //    {
        //        Subject = "test",
        //        Content = "Hello world",
        //        ToEmail = "radekpekul@gmail.com",
        //        ToName =  "Radek"
        //    });
        //    return Ok();
        //}
    }
}
