using Infrastructure.Mongo.Models;
using ApplicationCore.Repositories;
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
    public class ComputerMongoRepository : MongoBaseRepository<ComputerMongo>
    {
        public ComputerMongoRepository(IOptions<MongoDbSettings> databaseSettings) :
            base(databaseSettings, databaseSettings.Value.ComputersCollectionName)
        {

        }
    }
}
