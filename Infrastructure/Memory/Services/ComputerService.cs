using ApplicationCore.DTO;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Memory.Services
{
    public class ComputerService : IComputerService
    {
        private readonly IBaseRepository<Computer, string> computerRepository;
        private readonly IBaseRepository<User, string> userRepository;

        public ComputerService(IBaseRepository<Computer, string> computerRepository,
            IBaseRepository<User, string> userRepository)
        {
            this.computerRepository = computerRepository;
            this.userRepository = userRepository;
        }

        private void ValidateComputer(Computer computer)
        {
            ValidationContext validationContext = new ValidationContext(computer);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(computer, validationContext, validationResults);

            if (!isValid)
            {
                throw new InvalidComputerException(string.Join(", ", validationResults));
            }
        }

        private async Task<User> GetUser(int userId)
        {
            var user = await userRepository.Get(userId.ToString()) ?? throw new InvalidUserException("Invalid user id");
            return user;
        }


        public async Task<Computer> GetComputer(string computerId)
        {
            var computer = await computerRepository.Get(computerId) ?? throw new InvalidComputerException("Invalid computer id");
            return computer;
        }

        public async Task<Computer> AddComputer(string userId, Computer computer)
        {
            var user = await GetUser(int.Parse(userId));

            if (user.Type != UserType.Admin)
            {
                throw new PermissionException("User has no permission!");
            }

            ValidateComputer(computer);
            return await computerRepository.Add(computer);
        }

        public async Task<List<Computer>> GetUserComputers(string userId)
        {
            var user = await GetUser(int.Parse(userId));
            return user.Orders.SelectMany(x => x.Computers).ToList();
        }

        public async Task<Page<Computer>> GetAvailableComputers(int? pageIndex = null, int? pageSize = null)
        {
            var computers = await computerRepository.GetAll(pageIndex, pageSize);
            return new Page<Computer>
            {
                MaxCount = await computerRepository.Count(x => true), 
                PageNumber = pageIndex,
                PageSize = pageSize,
                Elements = computers
            };
        }

        //public async Task BuyComputers(string userId, List<string> computerIds)
        //{
        //    var user = await GetUser(int.Parse(userId));

        //    List<Computer> computers = new List<Computer>();
        //    foreach (var computerId in computerIds)
        //    {
        //        var computer = await GetComputer(computerId);
        //        if (computer.User != null)
        //        {
        //            throw new InvalidComputerException("Computer already bought!");
        //        }
        //        computers.Add(computer);
        //    }

        //    var saldo = await GetUserSaldo(user.Id.ToString());
        //    var totalAmount = computers.Sum(x => x.Price);

        //    if (saldo < totalAmount)
        //    {
        //        throw new NoSufficientFundsException("You have no money! Charge your saldo!");
        //    }

        //    computers.ForEach(async c => await BuyComputer(user, c));
        //}

        //private async Task BuyComputer(User user, Computer computer)
        //{
        //    computer.User = user;
        //    user.Computers.Add(computer);

        //    var transaction = new Transaction { Amount = -computer.Price, Description = $"You bought computer with ID: {computer.Id}", User = user, Date = DateTime.Now };

        //    await transactionRepository.Add(transaction);

        //    await computerRepository.Update(computer);
        //    await userRepository.Update(user);
        //}

        //private async Task<double> GetUserSaldo(string userId)
        //{
        //    return transactionRepository.FindAll(u => u.Id == userId).Result.Sum(x => x.Amount);
        //}
    }
}
