using Devon4Net.Infrastructure.Common.Handlers;
using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Devon4Net.Infrastructure.MongoDb.Constants;
using Devon4Net.Infrastructure.MongoDb.MongoDb;
using Devon4Net.Infrastructure.MongoDb.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devon4Net.Infrastructure.MongoDb
{
    public static class MongoDbConfiguration
    {
        public static void SetupMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoOptions = services.GetTypedOptions<MongoDbOptions>(configuration, MongoDbConstants.OptionsSection);
            if ( mongoOptions == null || !mongoOptions.UseMongo || string.IsNullOrEmpty(mongoOptions.ConnectionString) ) return;

            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

        }
    }
}
