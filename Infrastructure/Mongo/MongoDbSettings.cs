using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mongo
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ComputersCollectionName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;

        public string OrdersCollectionName { get; set; } = null!;
    }
}
