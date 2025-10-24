using ApplicationCore.DTO;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using AutoMapper;
using Infrastructure.EF.Models;

namespace Infrastructure.EF.Services
{
    public class OrderServiceEF : IOrderService
    {
        private readonly IBaseRepository<OrderEntity, int> orderRepository;
        private readonly IBaseRepository<UserEntity, int> userRepository;
        private readonly IBaseRepository<ComputerEntity, int> computerRepository;
        private readonly IMapper mapper;
        private readonly ITransactionManager transactionManager;

        public OrderServiceEF(IBaseRepository<OrderEntity, int> orderRepository,
            IBaseRepository<UserEntity, int> userRepository,
            IBaseRepository<ComputerEntity, int> computerRepository,
            IMapper mapper,
            ITransactionManager transactionManager)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.computerRepository = computerRepository;
            this.mapper = mapper;
            this.transactionManager = transactionManager;
        }

        public async Task ChangeOrderPaymentStatus(string paypalId, PaymentStatus paymentStatus)
        {
            try
            {
                await transactionManager.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                var order = await orderRepository.FindFirst(o => o.PaypalId == paypalId);
                order.PaymentStatus = paymentStatus;
                await orderRepository.Update(order);

            }
            catch (Exception)
            {
                await transactionManager.RollbackTransaction();
                throw;
            }
        }

        public async Task<Order> CreateOrder(string userId, List<string> computerIds, string paypalId)
        {
            if (string.IsNullOrEmpty(paypalId))
            {
                throw new ArgumentException("Invalid paypalId");
            }

            try
            {
                await transactionManager.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                UserEntity user = await userRepository.Get(int.Parse(userId));

                if (userId == null || computerIds == null || user == null)
                {
                    throw new ArgumentException("Wrong user ID or computer IDs");
                }

                var computers = await computerRepository.FindAll(x => computerIds.Contains(x.Id.ToString()));

                if (computers.Count != computerIds.Count)
                {
                    throw new ArgumentException("Invalid user ID or computer IDs");
                }

                double totalPrice = computers.Sum(computer => computer.Price);

                var order = new OrderEntity
                {
                    Computers = computers,
                    TotalAmount = totalPrice,
                    PaymentStatus = PaymentStatus.PENDING,
                    User = user,
                    PaypalId = paypalId
                };

                await orderRepository.Add(order);

                await transactionManager.CommitTransaction();

                return mapper.Map<Order>(order);
            }
            catch (Exception)
            {
                await transactionManager.RollbackTransaction();
                throw;
            }
        }

        public async Task<Order> FinishOrder(string paypalId)
        {
            try
            {
                await transactionManager.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

                var order = await orderRepository.FindFirst(x => x.PaypalId == paypalId);

                order.PaymentStatus = PaymentStatus.SUCCESS;

                await orderRepository.Update(order);

                await transactionManager.CommitTransaction();

                return mapper.Map<Order>(order);
            }
            catch (Exception)
            {
                await transactionManager.RollbackTransaction();
                throw;
            }

        }

        public async Task<Page<Order>> GetAllUserOrders(string userId, int? pageIndex = null, int? pageSize = null)
        {
            var allUserOrders = await GetUserOrders(userId, pageIndex, pageSize);
            var mappedUserOrders = mapper.Map<List<Order>>(allUserOrders);

            return new Page<Order>
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                MaxCount = await orderRepository.Count(o => o.User.Id == int.Parse(userId)),
                Elements = mappedUserOrders
            };
        }

        public async Task<Order> GetOrderById(string userId, string orderId)
        {
            return mapper.Map<Order>(await orderRepository.FindFirst(o => o.Id == int.Parse(orderId) && o.User.Id == int.Parse(userId)));
        }

        public async Task<Page<Computer>> GetOrderComputers(string userId, string orderId, int? pageIndex = null, int? pageSize = null)
        {
            var order = await orderRepository.Get(int.Parse(orderId));
            if(order.User.Id != int.Parse(userId))
            {
                throw new InvalidUserException("Invalid user ID.");
            }

            var userComputers = order.Computers.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
            var mappedUserComputers = mapper.Map<List<Computer>>(userComputers);

            return new Page<Computer>
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                MaxCount = order.Computers.Count,
                Elements = mappedUserComputers,
            };
        }

        private async Task<List<OrderEntity>> GetUserOrders(string userId, int? pageIndex = null, int? pageSize = null)
        {
            return await orderRepository.FindAll(o => o.User.Id == int.Parse(userId), pageIndex, pageSize);
        }
    }
}
