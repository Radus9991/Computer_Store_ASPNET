using ApplicationCore.Models;

namespace WebAPI.Services
{
    public interface IBasketService
    {
        Task AddComputerToBasket(HttpContext context, string id);

        Task DeleteComputer(HttpContext context, string id);

        Task<List<Computer>> GetBasketComputers(HttpContext context);

        void ClearBasket(HttpContext context);
    }
}
