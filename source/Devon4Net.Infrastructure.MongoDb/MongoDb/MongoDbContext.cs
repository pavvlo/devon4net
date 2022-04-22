
using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Devon4Net.Infrastructure.MongoDb.MongoDb
{
    public class MongoDbContext: IMongoDbContext
    {
        private readonly MongoDbOptions Options;

        public IMongoDatabase Database { get; set; }

        public MongoDbContext(IServiceProvider services)
        {
            Options = services.GetService<IOptions<MongoDbOptions>>().Value;
        }

        public void ConfigureDatabase(string name)
        {
            var database = Options.Databases.FirstOrDefault(d => d.DatabaseName == name);
            Database = new MongoClient(database.ConnectionString).GetDatabase(name);
        }
    }
}
