namespace Devon4Net.Infrastructure.Common.Options.MongoDb
{
    public class MongoDbOptions
    {
        public bool UseMongo { get; set; }

        public bool AllowMultipleDatabases { get; set; }

        public IEnumerable<MongoDatabase> Databases { get; set; }
    }
}
