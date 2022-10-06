using Devon4Net.Application.Kafka.Consumer.Domain.Entities;
using Devon4Net.Infrastructure.Kafka.Handlers.Consumer;
using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Logger.Logging;

namespace Devon4Net.Application.Kafka.Consumer.Business.KafkaManagement.Handlers
{
    public class MessageConsumerHandler : KafkaConsumerHandler<string, DataPiece<byte[]>>
    {
        public MessageConsumerHandler(IServiceCollection services, KafkaOptions kafkaOptions, string consumerId, bool commit = false, int commitPeriod = 5, bool enableConsumerFlag = true) : base(services, kafkaOptions, consumerId, commit, commitPeriod, enableConsumerFlag)
        {
        }

        public override void HandleCommand(string key, DataPiece<byte[]> value)
        {
            Devon4NetLogger.Information($"Consumer receives -> File: {value.FileName} {value.Position}/{value.TotalParts}");
        }
    }
}
