using Devon4Net.Application.Kafka.Business.Model;
using Devon4Net.Infrastructure.Kafka.Handlers.Producer;
using Devon4Net.Infrastructure.Kafka.Options;
using Microsoft.Extensions.Options;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Handlers
{
    public class MessageProducerHandler : KafkaProducerHandler<string, string>
    {
        public MessageProducerHandler(IServiceCollection services, KafkaOptions kafkaOptions, string producerId) : base(services, kafkaOptions, producerId)
        {
        }
    }
}
