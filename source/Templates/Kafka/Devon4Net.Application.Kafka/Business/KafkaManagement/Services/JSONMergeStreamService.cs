using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Kafka.Streams.Services;
using Microsoft.Extensions.Options;
using Streamiz.Kafka.Net;

namespace Devon4Net.Application.Kafka.Business.KafkaManagement.Services
{
    public class JSONMergeStreamService : KafkaStreamService
    {
        public override string GetApplicationId() => "json_merge_stream";

        public JSONMergeStreamService(IOptions<KafkaOptions> kafkaOptions) : base(kafkaOptions)
        {
        }

        public override StreamBuilder CreateStreamBuilder()
        {
            StreamBuilder streamBuilder = new();



            return streamBuilder;
        }

    }
}
