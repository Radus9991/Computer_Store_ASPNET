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
    public class UserRepository : IBaseRepository<User, string>
    {
        List<User> users = new List<User>();

        public async Task<User> Add(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentException();
            }

            entity.Id = Guid.NewGuid().ToString();
            users.Add(entity);
            return entity;
        }

        public Task<int> Count(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> FindAll(Expression<Func<User, bool>> expression, int? pageIndex = null, int? pageSize = null)
        {
            var result = new List<User>();

            if (pageIndex != null && pageSize != null)
            {
                result = users.AsEnumerable()
                    .Where(x => expression.Compile().Invoke(x))
                    .Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
            }
            else
            {
                result = users.AsEnumerable().Where(x => expression.Compile().Invoke(x)).ToList();
            }

            return result;
        }

        public async Task<User> FindFirst(Expression<Func<User, bool>> expression)
        {
            return users.FirstOrDefault(u => expression.Compile().Invoke(u));
        }

        public async Task<User> Get(string id)
        {
            return await FindFirst(x => x.Id == id);
        }


        public async Task<List<User>> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            return await FindAll(_ => true, pageIndex, pageSize);
        }

        public async Task<User> Remove(string id)
        {
            var user = await Get(id);
            users.Remove(user);
            return user;
        }

        public async Task<User> Update(User entityNew)
        {
            var user = await Get(entityNew.Id);
            if (user == null)
            {
                throw new ArgumentException();
            }
            await Remove(user.Id);
            await Add(entityNew);
            return entityNew;
        }
    }
}