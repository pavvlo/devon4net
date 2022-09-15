using Confluent.Kafka;

namespace Devon4Net.Infrastructure.Kafka.Handlers.Consumer
{
    public interface IKafkaConsumerHandler
    {
        IConsumer<T, TV> GetConsumerBuilder<T, TV>(string consumerId) where T : class where TV : class;
        T GetInstance<T>();
        void EnableConsumer(bool startConsumer = true);
        void DisableConsumer();
    }
}