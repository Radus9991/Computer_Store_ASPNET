using Infrastructure.Mongo.Models;
using ApplicationCore.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entity;

namespace Infrastructure.Mongo.Repositories
{
    public class MongoBaseRepository<T> : IBaseRepository<T, ObjectId?> where T : IEntity<ObjectId?>
    {
        private readonly IMongoCollection<T> collection;

        public MongoBaseRepository(IOptions<MongoDbSettings> databaseSettings, string collectionName)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            collection = mongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task<T> Add(T entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return (int)await collection.CountDocumentsAsync(expression);
        }

        public async Task<List<T>> FindAll(Expression<Func<T, bool>> expression, int? pageIndex = null, int? pageSize = null)
        {
            var result = collection.Find(expression)
               .ToEnumerable<T>();

            if (pageIndex != null && pageSize != null)
            {
                result = result.Skip((pageIndex.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
            }

            return result.ToList();
        }

        public async Task<T> FindFirst(Expression<Func<T, bool>> expression)
        {
            return await collection.Find(expression).FirstOrDefaultAsync();
        }

        public async Task<T> Get(ObjectId? id)
        {
            return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(int? pageIndex = null, int? pageSize = null)
        {
            return await FindAll(_ => true, pageIndex, pageSize);
        }

        public async Task<T> Remove(ObjectId? id)
        {
            var entity = await Get(id);

            await collection.DeleteOneAsync(x => x.Id == id);

            return entity;
        }

        public async Task<T> Update(T entityNew)
        {
            await collection.ReplaceOneAsync(x => x.Id == entityNew.Id, entityNew);

            return entityNew;
        }
    }
}
