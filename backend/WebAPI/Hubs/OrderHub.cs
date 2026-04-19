using ApplicationCore.Models;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class OrderHub : Hub
    {
        public async Task NotifyNewOrder(string orderId, PaymentStatus status)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("NewOrderReceived", orderId, status);
            }
        }
    }
}
