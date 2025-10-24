using ApplicationCore.DTO;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public interface IComputerService
    {
        Task<Computer> AddComputer(string userId, Computer computer);
        Task<Page<Computer>> GetAvailableComputers(int? pageIndex = null, int? pageSize = null);
        Task<Computer> GetComputer(string computerId);
    }
}