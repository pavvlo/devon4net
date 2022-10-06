using Confluent.Kafka;

namespace Devon4Net.Infrastructure.Kafka.Handlers.Producer
{
    public interface IKafkaProducerHandler<T, TV>
    {
        IProducer<T, TV> GetProducerBuilder<T, TV>(string producerId) where T : class where TV : class;
        Task<DeliveryResult<T, TV>> DeliverMessage<T, TV>(T key, TV value, string producerId) where T : class where TV : class;
        Task<DeliveryResult<T, TV>> SendMessage(T key, TV value);
    }
}