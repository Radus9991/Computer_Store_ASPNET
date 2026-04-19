using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Memory.Repositories
{
    public class OrderRepository : IBaseRepository<Order, string>
    {
        List<Order> orders = new List<Order>();

        public async Task<Order> Add(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentException();
            }

            entity.Id = Guid.NewGuid().ToString();
            orders.Add(entity);
            return entity;
        }

        public Task<int> Count(Expression<Func<Order, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> FindAll(Expression<Func<Order, bool>> expression, int? pageIndex = null, int? pageSize = null)
        {
            var result = new List<Order>();

            if (pageIndex != null && pageSize != null)
            {
                result = orders.AsEnumerable()
                    .Where(x => expression.Compile().Invoke(x))
                    .Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
            }
            else
            {
                result = orders.AsEnumerable().Where(x => expression.Compile().Invoke(x)).ToList();
            }

            return result;
        }

        public async Task<Order> FindFirst(Expression<Func<Order, bool>> expression)
        {
            return orders.FirstOrDefault(u => expression.Compile().Invoke(u));
        }

        public async Task<Order> Get(string id)
        {
            return await FindFirst(x => x.Id == id);
        }

        public async Task<List<Order>> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            return await FindAll(_ => true, pageIndex, pageSize);
        }

        public async Task<Order> Remove(string id)
        {
            var order = await Get(id);
            orders.Remove(order);
            return order;
        }

        public async Task<Order> Update(Order entityNew)
        {
            var order = await Get(entityNew.Id);
            if (order == null)
            {
                throw new ArgumentException();
            }
            await Remove(order.Id);
            await Add(entityNew);
            return entityNew;
        }
    }
}
