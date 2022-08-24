using Devon4Net.Application.Kafka.Business.Model;
using Devon4Net.Infrastructure.Kafka.Handlers;
using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Kafka.Streams.Services;
using Microsoft.Extensions.Options;
using Streamiz.Kafka.Net;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Services
{
    public class JSONMergeStreamService : KafkaStreamService
    {
        private IKafkaHandler KafkaHandler { get; set; }

        public override string GetApplicationId() => "json_merge_stream";

        public JSONMergeStreamService(IKafkaHandler kafkaHandler, IOptions<KafkaOptions> kafkaOptions) : base(kafkaOptions)
        {
            KafkaHandler = kafkaHandler;
            Setup();
        }

        public void Setup()
        {
            KafkaHandler.CreateTopic("Admin1", "entrance").Wait();
            KafkaHandler.CreateTopic("Admin1", "exit").Wait();
            KafkaHandler.CreateTopic("Admin1", "input").Wait();
            KafkaHandler.CreateTopic("Admin1", "output").Wait();
        }

        public override StreamBuilder CreateStreamBuilder()
        {
            StreamBuilder builder = new();
            builder
                .Stream<string, string>("entrance")
                .GroupByKey()
                .Aggregate(
                    () => new List<string>(),
                    (key, newValue, result) =>
                    {
                        result.Add(newValue);
                        //result.OrderBy(o => o.Position).ToList();
                        return result;
                    })
                .ToStream("exit")
                .Peek((k, v) => Console.WriteLine($"Stream says -> Key: [{k}] , Value: [{v}]"));

            return builder;
        }

        }
}
