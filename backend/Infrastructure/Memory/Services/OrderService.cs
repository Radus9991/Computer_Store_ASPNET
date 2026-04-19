using ApplicationCore.DTO;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Memory.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order, string> orderRepository;
        private readonly IBaseRepository<Computer, string> computerRepository;
        private readonly IBaseRepository<User, string> userRepository;

        public OrderService(IBaseRepository<Order, string> orderRepository,
            IBaseRepository<Computer, string> computerRepository,
            IBaseRepository<User, string> userRepository)
        {
            this.orderRepository = orderRepository;
            this.computerRepository = computerRepository;
            this.userRepository = userRepository;
        }

        public async Task ChangeOrderPaymentStatus(string paypalId, PaymentStatus paymentStatus)
        {
            var order = await orderRepository.FindFirst(o => o.PaypalId == paypalId);
            order.PaymentStatus = paymentStatus;
            await orderRepository.Update(order);
        }

        public async Task<Order> CreateOrder(string userId, List<string> computerIds, string paypalId)
        {
            if (userId == null || computerIds == null)
            {
                throw new ArgumentException("Invalid user ID or computer IDs");
            }

            var computers = await computerRepository.FindAll(c => computerIds.Contains(c.Id));
            double totalPrice = computers.Sum(computer => computer.Price);

            var order = new Order
            {
                Computers = computers,
                TotalAmount = totalPrice,
                PaymentStatus = PaymentStatus.PENDING,
                User = await userRepository.Get(userId),
                PaypalId = paypalId
            };

            await orderRepository.Add(order);

            return order;
        }

        public async Task<Order> FinishOrder(string paypalId)
        {
            var order = await orderRepository.FindFirst(order => order.PaypalId == paypalId);

            order.PaymentStatus = PaymentStatus.SUCCESS;

            await orderRepository.Update(order);

            return order;
        }

        public async Task<List<Order>> GetAllUserOrders(string userId, int? pageIndex = null, int? pageSize = null)
        {
            var allOrders = await orderRepository.GetAll(pageIndex, pageSize);
            var userOrders = allOrders.Where(o => o.User.Id.Equals(userId)).Take(5).ToList();
            userOrders.ForEach(o => o.Date = o.Date.ToLocalTime());
            return await orderRepository.FindAll(order => order.User.Id == userId);
        }

        public async Task<Order> GetOrderById(string userId, string orderId)
        {
            return await orderRepository.Get(orderId);
        }

        public async Task<List<Computer>> GetUserComputers(string userId, int? pageIndex = null, int? pageSize = null)
        {
            var orders = await GetAllUserOrders(userId, pageIndex, pageSize);
            return orders.SelectMany(order => order.Computers).ToList();
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
