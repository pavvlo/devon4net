
namespace Devon4Net.Infrastructure.MongoDb.Common
{
    public class ContextNotFoundException : Exception
    {
        public ContextNotFoundException() : base()
        {
        }

        public ContextNotFoundException(string message) : base(message)
        {
        }

        public ContextNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContextNotFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
