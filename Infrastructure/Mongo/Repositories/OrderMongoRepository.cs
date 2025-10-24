using ApplicationCore.Repositories;
using Infrastructure.Mongo.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mongo.Repositories
{
    public class OrderMongoRepository : MongoBaseRepository<OrderMongo>
    {
        public OrderMongoRepository(IOptions<MongoDbSettings> databaseSettings) :
            base(databaseSettings, databaseSettings.Value.OrdersCollectionName)
        {

        }
    }
}
