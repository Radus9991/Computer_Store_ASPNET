using ApplicationCore.Models;
using Infrastructure.Mongo.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mongo.Repositories
{
    public class UserMongoRepository : MongoBaseRepository<UserMongo>
    {
        public UserMongoRepository(IOptions<MongoDbSettings> databaseSettings) :
            base(databaseSettings, databaseSettings.Value.UsersCollectionName)
        {

        }
    }
}
