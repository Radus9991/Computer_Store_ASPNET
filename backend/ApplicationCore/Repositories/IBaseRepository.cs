using ApplicationCore.Entity;
using System.Linq.Expressions;

namespace ApplicationCore.Repositories
{
    public interface IBaseRepository <T, TKey> where T: IEntity<TKey> 
    {
        Task<T> Add(T entity);
        Task<T> Remove(TKey id);
        Task<T> Update(T entityNew);
        Task<T> Get(TKey id);
        Task<List<T>> GetAll(int? pageIndex = null, int? pageSize = null);
        Task<List<T>> FindAll(Expression<Func<T, bool>> expression, int? pageIndex = null, int? pageSize = null);
        Task<T> FindFirst(Expression<Func<T, bool>> expression);
        Task<int> Count(Expression<Func<T, bool>> expression);
    }
}
