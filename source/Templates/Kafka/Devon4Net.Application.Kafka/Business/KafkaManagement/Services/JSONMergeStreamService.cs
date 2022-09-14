using Devon4Net.Application.Kafka.Business.Model;
using Devon4Net.Infrastructure.Kafka.Handlers;
using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Kafka.Streams.Services;
using Microsoft.Extensions.Options;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.Stream;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Services
{
    public class JSONMergeStreamService : KafkaStreamService<string, string>
    {
        public JSONMergeStreamService(KafkaOptions kafkaOptions, string applicationId) : base(kafkaOptions, applicationId)
        {
        }

        public override void CreateStreamBuilder(ref IKStream<string, string> stream)
        {
            stream
               .GroupByKey()
               .Aggregate(
                   () => new List<string>(),
                   (key, newValue, result) =>
                   {
                       result.Add(newValue);
                       //result.OrderBy(o => o.Position).ToList();
                       return result;
                   })
               .ToStream()
               .Peek((k, v) => Console.WriteLine($"Stream says -> Key: [{k}] , Value: [{v}]"))
               .To("exit");
        }
    }
}
