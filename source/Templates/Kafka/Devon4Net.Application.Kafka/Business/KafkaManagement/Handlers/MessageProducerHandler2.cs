using Devon4Net.Infrastructure.Kafka.Handlers;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Handlers
{
    public class MessageProducerHandler2 : KafkaProducerHandler<string, string>
    {
        public MessageProducerHandler2(IServiceCollection services, IKafkaHandler kafkaHandler, string producerId) : base(services, kafkaHandler, producerId)
        {
        }
    }
}
