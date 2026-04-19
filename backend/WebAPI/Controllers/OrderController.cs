using ApplicationCore.DTO;
using ApplicationCore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.DTO.Computer;
using WebAPI.DTO.Order;
using WebAPI.Hubs;
using WebAPI.Models.Redis;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IPaypalService paypalService;
        private readonly IAuthorizationService authService;
        private readonly IComputerService computerService;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;
        private const int PageSize = 5;
        private string emailPattern;
        private readonly IHubContext<OrderHub> hubContext;

        public OrderController(IOrderService orderService,
            IPaypalService paypalService,
            IAuthorizationService authService,
            IComputerService computerService,
            IEmailService emailService,
            IMapper mapper,
            IHubContext<OrderHub> hubContext)
        {
            this.orderService = orderService;
            this.paypalService = paypalService;
            this.authService = authService;
            this.computerService = computerService;
            this.emailService = emailService;
            this.mapper = mapper;
            this.emailPattern = System.IO.File.ReadAllText("email.html");
            this.hubContext = hubContext;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(List<string> computerIds)
        {
            var user = await authService.GetAuthorizedUser(User);
            var computers = computerService.GetAvailableComputers().Result.Elements.Where(c => computerIds.Contains(c.Id)).ToList();
            var paypalId = await paypalService.CreateOrder(computers);

            if(paypalId == null)
            {
                return BadRequest("PaypalId is null");
            }

            await orderService.CreateOrder(user.Id.ToString(), computerIds, paypalId);

            return Ok(new { PaypalId = paypalId });
        }

        [HttpPost("approve")]
        public async Task<IActionResult> AcceptTransaction(ApproveOrderDTO approveOrder)
        {
            var finished = await paypalService.IsPaymentFinished(approveOrder.OrderID);
            if (finished)
            {
                var order = await orderService.FinishOrder(approveOrder.OrderID);

                await hubContext.Clients.All.SendAsync("NewOrderReceived", order.Id, order.PaymentStatus);

                EmailMessage emailMessage = new EmailMessage
                {
                    Content = emailPattern.Replace("{username}", order.User.Name).Replace("{orderId}", order.Id)
                    .Replace("{paymentStatus}", order.PaymentStatus.ToString())
                    .Replace("{totalAmount}", order.TotalAmount.ToString())
                    .Replace("{date}", order.Date.ToString()),
                    Subject = "Payment confirmation.}",
                    ToEmail = order.User.Email,
                    ToName = order.User.Name
                };

                await emailService.SendEmail(emailMessage);
                return Ok();
            }

            return BadRequest();
        }


        [HttpGet("{pageIndex}")]
        public async Task<IActionResult> GetUserOrders(int pageIndex)
        {
            var user = await authService.GetAuthorizedUser(User);
            var orders = await orderService.GetAllUserOrders(user.Id, pageIndex, PageSize);
            return Ok(mapper.Map<Page<OrderGetDTO>>(orders));
        }

        [HttpGet("{orderId}/computers/{pageIndex}")]
        public async Task<IActionResult> GetOrderComputers(string orderId, int pageIndex)
        {
            var user = await authService.GetAuthorizedUser(User);
            if (user == null || string.IsNullOrEmpty(orderId))
            {
                return BadRequest("Invalid user or order ID");
            }
            return Ok(mapper.Map<Page<ComputerGetDTO>>(await orderService.GetOrderComputers(user.Id, orderId, pageIndex, PageSize)));
        }

        [HttpGet("GetOrderList")]
        public async Task<IActionResult> GetUserOrdersAllIds()
        {
            var user = await authService.GetAuthorizedUser(User);
            return Ok(mapper.Map<Page<OrderShortGetDTO>>(await orderService.GetAllUserOrders(user.Id)));
        }

        [HttpGet("GetOrder/{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            var user = await authService.GetAuthorizedUser(User);
            return Ok(mapper.Map<OrderGetDTO>(await orderService.GetOrderById(user.Id, orderId)));
        }
    }
}
