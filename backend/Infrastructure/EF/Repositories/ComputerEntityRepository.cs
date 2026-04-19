using Infrastructure.EF.Models;
using ApplicationCore.Repositories;
using System.Linq.Expressions;

namespace Infrastructure.EF.Repositories
{
    public class ComputerEntityRepository : IBaseRepository<ComputerEntity, int>
    {
        private readonly Context.DataContext context;

        public ComputerEntityRepository(Context.DataContext context)
        {
            this.context = context;
        }

        public async Task<ComputerEntity> Add(ComputerEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException();
            }

            await context.Computers.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> Count(Expression<Func<ComputerEntity, bool>> expression)
        {
            return context.Computers.Count(expression);
        }

        public async Task<List<ComputerEntity>> FindAll(Expression<Func<ComputerEntity, bool>> expression, int? pageIndex = null, int? pageSize = null)
        {
            var result = context.Computers.AsEnumerable()
                    .Where(x => expression.Compile().Invoke(x));

            if (pageIndex != null && pageSize != null)
            {
                result = result.Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return result.ToList();  
        }

        public async Task<ComputerEntity> FindFirst(Expression<Func<ComputerEntity, bool>> expression)
        {
            return context.Computers.AsEnumerable().FirstOrDefault(x => expression.Compile().Invoke(x));
        }

        public async Task<ComputerEntity> Get(int id)
        {
            return await FindFirst(x => x.Id == id);
        }

        public async Task<List<ComputerEntity>> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            return await FindAll(_ => true, pageIndex, pageSize);
        }

        public async Task<ComputerEntity> Remove(int id)
        {
            var computer = await Get(id);
            context.Computers.Remove(computer);
            await context.SaveChangesAsync();
            return computer;
        }

        public async Task<ComputerEntity> Update(ComputerEntity entityNew)
        {
            var computer = await Get(entityNew.Id);
            if (computer == null)
            {
                throw new ArgumentException();
            }
            context.Computers.Update(entityNew);
            await context.SaveChangesAsync();
            return entityNew;
        }
    }
}
