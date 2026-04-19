using ApplicationCore.DTO;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using AutoMapper;
using Infrastructure.Mongo.Models;
using MongoDB.Bson;

namespace Infrastructure.Mongo.Services
{
    public class OrderServiceMongo : IOrderService
    {
        private readonly IBaseRepository<OrderMongo, ObjectId?> orderRepository;
        private readonly IBaseRepository<UserMongo, ObjectId?> userRepository;
        private readonly IBaseRepository<ComputerMongo, ObjectId?> computerRepository;
        private readonly IMapper mapper;
        private readonly ITransactionManager transactionManager;

        public OrderServiceMongo(IBaseRepository<OrderMongo, ObjectId?> orderRepository,
            IBaseRepository<UserMongo, ObjectId?> userRepository,
            IBaseRepository<ComputerMongo, ObjectId?> computerRepository,
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
            var order = await orderRepository.FindFirst(o => o.PaypalId == paypalId);
            order.PaymentStatus = paymentStatus;
            await orderRepository.Update(order);
        }

        public async Task<Order> CreateOrder(string userId, List<string> computerIds, string paypalId)
        {
            if (string.IsNullOrEmpty(paypalId))
            {
                throw new ArgumentException("Invalid Paypal Id");
            }        

            await transactionManager.BeginTransaction();
            try
            {
                UserMongo user = await userRepository.FindFirst(u => u.Id.ToString() == userId);

                if (userId == null || computerIds == null || user == null)
                {
                    throw new ArgumentException("Invalid user ID or computer IDs");
                }

                var computers = await computerRepository.FindAll(c => computerIds.Contains(c.Id.ToString()));

                if (computers.Count != computerIds.Count)
                {
                    throw new ArgumentException("Invalid user ID or computer IDs");
                }

                double totalPrice = computers.Sum(computer => computer.Price);

                var order = new OrderMongo
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
            var order = await orderRepository.FindFirst(x => x.PaypalId == paypalId);

            order.PaymentStatus = PaymentStatus.SUCCESS;

            await orderRepository.Update(order);

            return mapper.Map<Order>(order);
        }

        public async Task<List<Order>> GetAllUserOrders(string userId, int? pageIndex = null, int? pageSize = null)
        {
            var userOrders = await orderRepository.FindAll(o => o.User.Id.Equals(userId), pageIndex, pageSize);
            userOrders.ForEach(o => o.Date = o.Date.ToLocalTime());
            return mapper.Map<List<Order>>(userOrders);
        }

        public async Task<Order> GetOrderById(string userId, string orderId)
        {
            var user = await userRepository.Get(ObjectId.Parse(userId));

            if (user == null)
            {
                throw new InvalidUserException("Invalid user id");
            }

            var order = await orderRepository.Get(ObjectId.Parse(orderId));

            if (order.User.Id != user.Id)
            {
                throw new InvalidOrderException("Order does not belong to user");
            }

            var orderFound = await orderRepository.Get(ObjectId.Parse(orderId));
            orderFound.Date = orderFound.Date.ToLocalTime();
            return mapper.Map<Order>(orderFound);
        }

        public async Task<List<Computer>> GetUserComputers(string userId, int? pageIndex = null, int? pageSize = null)
        {
            var orders = await GetAllUserOrders(userId, pageIndex, pageSize);
            return mapper.Map<List<Computer>>(orders.SelectMany(x => x.Computers));
        }

        Task<Page<Order>> IOrderService.GetAllUserOrders(string userId, int? pageIndex, int? pageSize)
        {
            throw new NotImplementedException();
        }

        Task<Page<Computer>> IOrderService.GetOrderComputers(string userId, string orderId, int? pageIndex, int? pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
