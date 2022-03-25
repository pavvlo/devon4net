
using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Devon4Net.Infrastructure.MongoDb.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        public IDictionary<string, IMongoDatabase> Databases { get; set; }

        public MongoDbContext(IOptions<MongoDbOptions> options)
        {
            foreach(var database in options.Value.Databases)
                AddDatabase(database.DatabaseName, database.ConnectionString);
        }

        public bool AddDatabase(string name, string connection)
        {
            if(Databases == null) Databases = new Dictionary<string, IMongoDatabase>();
            var database = new MongoClient(connection).GetDatabase(name);
            return Databases.TryAdd(name, database);
        }
    }
}
