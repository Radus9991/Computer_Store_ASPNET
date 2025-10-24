using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Memory.Repositories
{
    public class ComputerRepository : IBaseRepository<Computer, string>
    {
        List<Computer> computers = new List<Computer>();

        public async Task<Computer> Add(Computer entity)
        {
            if (entity == null)
            {
                throw new ArgumentException();
            }

            entity.Id = Guid.NewGuid().ToString();
            computers.Add(entity);
            return entity;
        }

        public Task<int> Count(Expression<Func<Computer, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Computer>> FindAll(Expression<Func<Computer, bool>> expression, int? pageIndex = null, int? pageSize = null)
        {
            var result = new List<Computer>();

            if (pageIndex != null && pageSize != null)
            {
                result = computers.AsEnumerable()
                    .Where(x => expression.Compile().Invoke(x))
                    .Skip((pageIndex.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
            }
            else
            {
                result = computers.AsEnumerable().Where(x => expression.Compile().Invoke(x)).ToList();
            }

            return result;
        }

        public async Task<Computer> FindFirst(Expression<Func<Computer, bool>> expression)
        {
            return computers.FirstOrDefault(c => expression.Compile().Invoke(c));
        }

        public async Task<Computer> Get(string id)
        {
            return await FindFirst(x => x.Id == id);
        }

        public async Task<List<Computer>> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            return await FindAll(_ => true, pageIndex, pageSize);
        }

        public async Task<Computer> Remove(string id)
        {
            var computer = await Get(id);
            computers.Remove(computer);
            return computer;
        }

        public async Task<Computer> Update(Computer entityNew)
        {
            var computer = await Get(entityNew.Id);
            if (computer == null)
            {
                throw new ArgumentException();
            }
            await Remove(computer.Id);
            await Add(entityNew);
            return entityNew;
        }
    }
}
