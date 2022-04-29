namespace Devon4Net.Infrastructure.MongoDb.Constants
{
    public static class MongoDbConstants
    {
        public const string OptionsSection = "MongoDb";
        public const string DatabaseNotFoundMessage = "The specified database does not exist. Database name and connection string must be declared in 'appsettings.{environment}.json'.";
        public const string ContextNotFoundMessage = "Context {0} was not found injected as a service hence cannot be used. Register your services during startup.";
    }
}
