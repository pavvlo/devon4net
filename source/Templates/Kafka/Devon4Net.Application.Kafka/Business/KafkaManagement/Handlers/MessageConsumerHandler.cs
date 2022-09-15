using Devon4Net.Application.Kafka.Business.Model;
using Devon4Net.Infrastructure.Kafka.Handlers.Consumer;
using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Logger.Logging;
using Microsoft.Extensions.Options;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Handlers
{
    public class MessageConsumerHandler : KafkaConsumerHandler<string, string>
    {
        public MessageConsumerHandler(IServiceCollection services,KafkaOptions kafkaOptions, string consumerId, bool commit = false, int commitPeriod = 5) : base(services, kafkaOptions, consumerId, commit, commitPeriod)
        {
        }

        public override void HandleCommand(string key, string value)
        {
            Devon4NetLogger.Information($"Received message key: {key} | value: {value}");
        }
    }
}
