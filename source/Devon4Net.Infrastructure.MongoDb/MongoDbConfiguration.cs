using Devon4Net.Infrastructure.Common.Handlers;
using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Devon4Net.Infrastructure.MongoDb.Common;
using Devon4Net.Infrastructure.MongoDb.Constants;
using Devon4Net.Infrastructure.MongoDb.MongoDb;
using Devon4Net.Infrastructure.MongoDb.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Devon4Net.Infrastructure.MongoDb
{
    public static class MongoDbConfiguration
    {

        public static MongoDbOptions MongoOptions { get; set; }

        public static void SetupMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            MongoOptions = services.GetTypedOptions<MongoDbOptions>(configuration, MongoDbConstants.OptionsSection);
            if (MongoOptions == null || !MongoOptions.UseMongo || MongoOptions.Databases == null || MongoOptions.Databases.Count() == 0) return;

            services.AddTransient(typeof(IMongoDbRepository<,>), typeof(MongoDbRepository<,>));
        }

        public static void AddMongoDatabase<T>(this IServiceCollection services, IConfiguration configuration, string databaseName) where T : MongoDbContext
        {
            if (MongoOptions == null || !MongoOptions.UseMongo || MongoOptions.Databases == null || MongoOptions.Databases.Count() == 0) return;
            var databaseOptions = MongoOptions.Databases.FirstOrDefault(d => d.DatabaseName == databaseName);
            if (databaseOptions == null)
                throw new ContextNotFoundException(MongoDbConstants.DatabaseNotFoundMessage);

            var context = (T) Activator.CreateInstance(typeof(T));
            context.Database = new MongoClient(databaseOptions.ConnectionString).GetDatabase(databaseName);
            
            services.AddSingleton(context);
        }
    }
}
