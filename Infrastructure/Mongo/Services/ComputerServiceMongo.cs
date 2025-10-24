using ApplicationCore.Exceptions;
using AutoMapper;
using ApplicationCore.Models;
using Infrastructure.Mongo.Models;
using ApplicationCore.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Services;
using ApplicationCore.DTO;

namespace Infrastructure.Mongo.Sevices
{
    public class ComputerServiceMongo : IComputerService
    {
        private readonly IBaseRepository<ComputerMongo, ObjectId?> computerRepository;
        private readonly IBaseRepository<UserMongo, ObjectId?> userRepository;
        private readonly IBaseRepository<OrderMongo, ObjectId?> orderRepository;
        private readonly IMapper mapper;

        public ComputerServiceMongo(IBaseRepository<ComputerMongo, ObjectId?> computerRepository,
            IBaseRepository<UserMongo, ObjectId?> userRepository,
            IBaseRepository<OrderMongo, ObjectId?> orderRepository,
            IMapper mapper)
        {
            this.computerRepository = computerRepository;
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
            this.mapper = mapper;
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

        private async Task<UserMongo> GetUserMongo(ObjectId? userId)
        {
            var user = await userRepository.Get(userId);
            if (user == null)
            {
                throw new InvalidUserException("Invalid user id");
            }
            return user;
        }

        private async Task<ComputerMongo> GetComputerMongo(ObjectId? computerId)
        {
            return await computerRepository.Get(computerId) ?? throw new InvalidComputerException("Invalid computer id");
        }

        public async Task<Computer> GetComputer(string computerId)
        {
            var computer = await GetComputerMongo(ObjectId.Parse(computerId));
            return mapper.Map<Computer>(computer);
        }

        public async Task<Computer> AddComputer(string userId, Computer computer)
        {
            UserMongo user = await GetUserMongo(ObjectId.Parse(userId));

            if (user.Type != UserType.Admin)
            {
                throw new PermissionException("User has no permission!");
            }

            ValidateComputer(computer);

            ComputerMongo computerEntity = mapper.Map<ComputerMongo>(computer);

            return mapper.Map<Computer>(await computerRepository.Add(computerEntity));
        }

        public async Task<Page<Computer>> GetAvailableComputers(int? pageIndex = null, int? pageSize = null)
        {
            var computers = mapper.Map<List<Computer>>(await computerRepository.GetAll(pageIndex, pageSize));
            return new Page<Computer>
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                MaxCount = await computerRepository.Count(x => true),
                Elements = computers
            };
        }


        //public async Task<List<Computer>> GetUserComputers(string userId)
        //{
        //    var user = await GetUserMongo(ObjectId.Parse(userId));
        //    return mapper.Map<List<Computer>>(await orderRepository.FindAll(o => o.User != null && o.User.Id == userId));
        //}

        //private async Task BuyComputer(UserMongo user, ComputerMongo computer)
        //{
        //    computer.User = user;
        //    //user.Computers.Add(computer);

        //    var transaction = new TransactionMongo { Amount = -computer.Price, Description = $"You bought computer with ID: {computer.Id}", User = user, Date = DateTime.Now };

        //    await transactionRepository.Add(transaction);

        //    await computerRepository.Update(computer);
        //    await userRepository.Update(user);
        //}

        //public async Task BuyComputers(string userId, List<string> computerIds)
        //{
        //    var user = await GetUserMongo(ObjectId.Parse(userId));

        //    List<ComputerMongo> computers = new List<ComputerMongo>();
        //    foreach (var computerId in computerIds)
        //    {
        //        var computer = await GetComputerMongo(ObjectId.Parse(computerId));
        //        if (computer.User != null)
        //        {
        //            throw new InvalidComputerException("Computer already bought!");
        //        }
        //        computers.Add(computer);
        //    }

        //    var saldo = await GetUserSaldo(user.Id);
        //    var totalAmount = computers.Sum(x => x.Price);

        //    if (saldo < totalAmount)
        //    {
        //        throw new NoSufficientFundsException("You have no money! Charge your saldo!");
        //    }

        //    foreach (var computer in computers)
        //    {
        //        await BuyComputer(user, computer);
        //    }
        //}

        //private async Task<double> GetUserSaldo(ObjectId? userId)
        //{
        //    return transactionRepository.FindAll(u => u.User.Id == userId).Result.Sum(x => x.Amount);
        //}
    }
}
