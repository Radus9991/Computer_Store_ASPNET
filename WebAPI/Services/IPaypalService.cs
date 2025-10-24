using ApplicationCore.Models;

namespace WebAPI.Services
{
    public interface IPaypalService
    {
        Task<string> CreateOrder(List<Computer> computers);
        Task<bool> IsPaymentFinished(string paypalId);
    }
}