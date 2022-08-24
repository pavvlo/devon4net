using Devon4Net.Application.Kafka.Business.Model;
using Devon4Net.Infrastructure.Kafka.Handlers;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Handlers
{
    public class MessageProducerHandler : KafkaProducerHandler<string, string>
    {
        public MessageProducerHandler(IServiceCollection services, IKafkaHandler kafkaHandler, string producerId) : base(services, kafkaHandler, producerId)
        {
        }
    }
}
