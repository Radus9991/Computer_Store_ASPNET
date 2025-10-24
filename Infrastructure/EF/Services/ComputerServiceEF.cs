using ApplicationCore.Exceptions;
using ApplicationCore.Services;
using AutoMapper;
using Infrastructure.EF.Models;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System.ComponentModel.DataAnnotations;
using ApplicationCore.DTO;

namespace Infrastructure.EF.Sevices
{
    public class ComputerServiceEF : IComputerService
    {
        private readonly IBaseRepository<ComputerEntity, int> computerRepository;
        private readonly IBaseRepository<UserEntity, int> userRepository;
        private readonly IMapper mapper;
        private readonly ITransactionManager transactionManager;

        public ComputerServiceEF(IBaseRepository<ComputerEntity, int> computerRepository,
            IBaseRepository<UserEntity, int> userRepository,
            IMapper mapper,
            ITransactionManager transactionManager)
        {
            this.computerRepository = computerRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.transactionManager = transactionManager;
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

        private async Task<UserEntity> GetUserEF(int userId)
        {
            var user = await userRepository.Get(userId);
            if (user == null)
            {
                throw new InvalidUserException("Invalid user id");
            }
            return user;
        }

        private async Task<ComputerEntity> GetComputerEF(int computerId)
        {
            return await computerRepository.Get(computerId) ?? throw new InvalidComputerException("Invalid computer id");
        }


        public async Task<Computer> GetComputer(string computerId)
        {
            var computer = await GetComputerEF(int.Parse(computerId));
            return mapper.Map<Computer>(computer);
        }

        public async Task<Computer> AddComputer(string userId, Computer computer)
        {
            try
            {
                await transactionManager.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                var user = await GetUserEF(int.Parse(userId));

                if (user.Type != UserType.Admin)
                {
                    throw new PermissionException("User has no permission!");
                }

                ValidateComputer(computer);

                ComputerEntity computerEntity = mapper.Map<ComputerEntity>(computer);
                var result = mapper.Map<Computer>(await computerRepository.Add(computerEntity));

                await transactionManager.CommitTransaction();

                return result;

            }
            catch (Exception)
            {
                await transactionManager.RollbackTransaction();
                throw;
            }

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
    }
}
