
using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Devon4Net.Infrastructure.MongoDb.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly MongoDbOptions Options;

        public MongoDbContext(IOptions<MongoDbOptions> options)
        {
            Options = options.Value;
        }

        public IMongoDatabase GetDatabase(string name)
        {
            var database = Options.Databases.FirstOrDefault(d => d.DatabaseName == name);
            return new MongoClient(database.ConnectionString).GetDatabase(name);
        }
    }
}
