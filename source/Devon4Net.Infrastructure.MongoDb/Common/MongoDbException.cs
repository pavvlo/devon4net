
namespace Devon4Net.Infrastructure.MongoDb.Common
{
    public class MongoDbException : Exception
    {
        public MongoDbException() : base()
        {
        }

        public MongoDbException(string message) : base(message)
        {
        }

        public MongoDbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MongoDbException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
