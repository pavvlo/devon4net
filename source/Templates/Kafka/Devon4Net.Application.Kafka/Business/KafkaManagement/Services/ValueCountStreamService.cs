using Devon4Net.Infrastructure.Kafka.Handlers;
using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Kafka.Streams.Services;
using Microsoft.Extensions.Options;
using Streamiz.Kafka.Net;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Services
{
    public class ValueCountStreamService : KafkaStreamService
    {
        public IKafkaHandler KafkaHandler;
        public override string GetApplicationId() => "value_count_stream";

        public ValueCountStreamService(IKafkaHandler kafkaHandler, IOptions<KafkaOptions> kafkaOptions) : base(kafkaOptions)
        {
            KafkaHandler = kafkaHandler;
            Setup();
        }

        public void Setup()
        {
            //KafkaHandler.CreateTopic("Admin1", "hola").Wait();
            //KafkaHandler.CreateTopic("Admin1", "adios").Wait();
        }

        public override StreamBuilder CreateStreamBuilder()
        {
            StreamBuilder builder = new();

            builder.Stream<string, string>("input")
                .FlatMapValues((v) => v.Split(" ").AsEnumerable())
                .GroupBy((k, v) => v)
                .Count()
                .ToStream()
                .MapValues((v) => v.ToString())
                .Peek((k, v) => Console.WriteLine($"Stream says -> Key: [{k}] , Value: [{v}]"))
                .To("output");

            return builder;
        }

    }
}
