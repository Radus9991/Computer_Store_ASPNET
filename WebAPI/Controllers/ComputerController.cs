using ApplicationCore.Exceptions;
using ApplicationCore.Services;
using AutoMapper;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO.Computer;
using ApplicationCore.DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly IComputerService computerService;
        private readonly IMapper mapper;
        private readonly Services.IAuthorizationService authService;
        private readonly IOrderService orderService;
        private const int PageSize = 5;

        public ComputerController(IComputerService computerService, IMapper mapper, Services.IAuthorizationService authService, IOrderService orderService)
        {
            this.computerService = computerService;
            this.mapper = mapper;
            this.authService = authService;
            this.orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComputer(ComputerCreateDTO computer)
        {
            if (ModelState.IsValid)
            {
                var user = await authService.GetAuthorizedUser(User);
                try
                {
                    await computerService.AddComputer(user.Id, mapper.Map<Computer>(computer));
                }
                catch (PermissionException ex)
                {
                    return Unauthorized(new { Error = ex.Message });
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }


        [HttpGet("available/{pageIndex}")]
        public async Task<IActionResult> GetAvailableComputers(int pageIndex)
        {
            var computers = await computerService.GetAvailableComputers(pageIndex, PageSize);
            var x = (mapper.Map<Page<ComputerGetDTO>>(computers));
            return Ok(x);
        }

        [HttpPost("basket")]
        [Authorize]
        public async Task<IActionResult> GetBasketComputers(string[] computerIds)
        {
            List<Computer> computers = new List<Computer>();

            foreach (var id in computerIds)
            {
                var pc = await computerService.GetComputer(id);
                if (pc != null)
                {
                    computers.Add(pc);
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok(mapper.Map<List<ComputerGetDTO>>(computers));
        }
    }
}
