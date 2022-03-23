
using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Devon4Net.Infrastructure.MongoDb.MongoDb
{
    public class MongoDbContext: IMongoDbContext
    {
        public IMongoDatabase Database { get; set; }

        public MongoDbContext(IOptions<MongoDbOptions> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            Database = mongoClient.GetDatabase(options.Value.DatabaseName);
        }
    }
}
