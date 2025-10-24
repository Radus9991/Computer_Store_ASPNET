using ApplicationCore.DTO;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(string userId, List<string> computerIds, string paypalId);
        Task<Order> FinishOrder(string paypalId);
        Task ChangeOrderPaymentStatus(string paypalId, PaymentStatus paymentStatus);
        Task<Order> GetOrderById(string userId, string orderId);
        Task<Page<Order>> GetAllUserOrders(string userId, int? pageIndex = null, int? pageSize = null);
        Task<Page<Computer>> GetOrderComputers(string userId, string orderId, int? pageIndex = null, int? pageSize = null);
    }
}
