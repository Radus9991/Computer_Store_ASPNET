using ApplicationCore.Repositories;
using Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.EF.Repositories
{
    public class OrderEntityRepository : IBaseRepository<OrderEntity, int>
    {
        private readonly Context.DataContext context;

        public OrderEntityRepository(Context.DataContext context)
        {
            this.context = context;
        }

        public async Task<OrderEntity> Add(OrderEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("There is no order.");
            }

            await context.Orders.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> Count(Expression<Func<OrderEntity, bool>> expression)
        {
            return context.Orders.Count(expression);
        }

        public async Task<List<OrderEntity>> FindAll(Expression<Func<OrderEntity, bool>> expression, int? pageIndex = null, int? pageSize = null)
        {
            var result = GetData().AsEnumerable().Where(x => expression.Compile().Invoke(x));

            if (pageIndex != null && pageSize != null)
            {
                result = result
                    .Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return result.ToList();
        }

        public async Task<OrderEntity> FindFirst(Expression<Func<OrderEntity, bool>> expression)
        {
            return GetData().AsEnumerable().FirstOrDefault(o => expression.Compile().Invoke(o));
        }

        public async Task<OrderEntity> Get(int id)
        {
            return await FindFirst(x => x.Id == id);
        }

        public async Task<List<OrderEntity>> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            return await FindAll(_ => true, pageIndex, pageSize);
        }

        public async Task<OrderEntity> Remove(int id)
        {
            var order = await Get(id);

            if (order == null)
            {
                throw new ArgumentNullException();
            }

            context.Orders.Remove(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderEntity> Update(OrderEntity entityNew)
        {
            var order = await Get(entityNew.Id);

            if (order == null)
            {
                throw new ArgumentNullException();
            }

            context.Orders.Update(entityNew);
            await context.SaveChangesAsync();
            return entityNew;
        }

        private IQueryable<OrderEntity> GetData()
        {
            return context.Orders
                .Include(x => x.Computers)
                .Include(x => x.User);
        }
    }
}
