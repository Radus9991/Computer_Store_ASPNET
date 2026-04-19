using Infrastructure.EF.Models;
using ApplicationCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.EF.Repositories
{
    public class UserEntityRepository : IBaseRepository<UserEntity, int>
    {
        private readonly Context.DataContext context;

        public UserEntityRepository(Context.DataContext context)
        {
            this.context = context;
        }

        public async Task<UserEntity> Add(UserEntity entity)
        {
            if (entity == null || await context.Users.AnyAsync(x => x.Email == entity.Email))
            {
                throw new ArgumentException();
            }

            await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> Count(Expression<Func<UserEntity, bool>> expression)
        {
            return context.Users.Count(x => expression.Compile().Invoke(x));
        }

        public async Task<List<UserEntity>> FindAll(Expression<Func<UserEntity, bool>> expression, int? pageIndex = null, int? pageSize = null)
        {
            var result = new List<UserEntity>();

            if (pageIndex != null && pageSize != null)
            {
                result = context.Users.AsEnumerable()
                    .Where(x => expression.Compile().Invoke(x))
                    .Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
            }
            else
            {
                result = context.Users.AsEnumerable().Where(x => expression.Compile().Invoke(x)).ToList();
            }

            return result;
        }

        public async Task<UserEntity> FindFirst(Expression<Func<UserEntity, bool>> expression)
        {
            return context.Users.AsEnumerable().FirstOrDefault(u => expression.Compile().Invoke(u));
        }

        public async Task<UserEntity> Get(int id)
        {
            return await FindFirst(x => x.Id == id);
        }

        public async Task<List<UserEntity>> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            return await FindAll(_ => true, pageIndex, pageSize);
        }

        public async Task<UserEntity> Remove(int id)
        {
            var user = await Get(id);

            if (user == null)
            {
                throw new ArgumentNullException();
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity> Update(UserEntity entityNew)
        {
            var user = await Get(entityNew.Id);

            if (user == null)
            {
                throw new ArgumentNullException();
            }

            context.Users.Update(entityNew);
            await context.SaveChangesAsync();
            return entityNew;
        }
    }
}
