using ApplicationCore.Services;
using Infrastructure.EF.Context;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.EF.Services
{
    public class TransactionManagerEF : ITransactionManager
    {
        private readonly DataContext context;
        private IDbContextTransaction transaction;

        public TransactionManagerEF(DataContext context)
        {
            this.context = context;
        }

        public async Task BeginTransaction(IsolationLevel? level = null)
        {
            if(level == null)
            {
                throw new ArgumentException("Null isolation level");
            }

            transaction = await context.Database.BeginTransactionAsync(level.Value);
        }

        public async Task CommitTransaction()
        {
            if (transaction != null)
            {
                await transaction.CommitAsync();
            }
        }

        public async Task RollbackTransaction()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
