using Confluent.Kafka;

namespace Devon4Net.Infrastructure.Kafka.Handlers
{
    public interface IKafkaHandler
    {
        Task<bool> CreateTopic(string adminId, string topicName, short replicationFactor = 1, int numPartitions = 1);
        Task<bool> DeleteTopic(string adminId, List<string> topicsName);
    }
}