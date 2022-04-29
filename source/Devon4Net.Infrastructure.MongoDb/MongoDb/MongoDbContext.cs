
using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Devon4Net.Infrastructure.MongoDb.MongoDb
{
    public class MongoDbContext: IMongoDbContext
    {
        public IMongoDatabase Database { get; set; }

        public MongoDbContext()
        {
        }
    }
}
