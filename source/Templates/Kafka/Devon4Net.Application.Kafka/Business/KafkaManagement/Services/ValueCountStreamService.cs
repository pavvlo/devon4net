
using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Kafka.Streams.Services;
using Streamiz.Kafka.Net.Stream;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Services
{
    public class ValueCountStreamService : KafkaStreamService<string, string>
    {

        public ValueCountStreamService(KafkaOptions kafkaOptions, string applicationId) : base(kafkaOptions, applicationId)
        { 
        }

        public override void CreateStreamBuilder(ref IKStream<string, string> stream)
        {

            stream
                .FlatMapValues((v) => v.Split(" ").AsEnumerable())
                .GroupBy((k, v) => v)
                .Count()
                .ToStream()
                .MapValues((v) => v.ToString())
                .Peek((k, v) => Console.WriteLine($"Stream says -> Key: [{k}] , Value: [{v}]"))
                .To("output");
        }

    }
}
