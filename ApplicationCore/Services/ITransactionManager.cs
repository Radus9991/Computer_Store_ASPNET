using System.Data;

namespace ApplicationCore.Services
{
    public interface ITransactionManager
    {
        Task BeginTransaction(IsolationLevel? level = null);
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
