using ApplicationCore.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mongo.Services
{
    public class TransactionManagerMongo : ITransactionManager
    {
        private MongoClient mongoClient;
        private IClientSession session;

        public TransactionManagerMongo(IOptions<MongoDbSettings> databaseSettings)
        {
            mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            session = mongoClient.StartSession();
        }
        public async Task BeginTransaction(IsolationLevel? level = null)
        {
            session.StartTransaction();
        }

        public async Task CommitTransaction()
        {
            await session.CommitTransactionAsync();
        }

        public async Task RollbackTransaction()
        {
            await session.AbortTransactionAsync();
        }
    }
}
