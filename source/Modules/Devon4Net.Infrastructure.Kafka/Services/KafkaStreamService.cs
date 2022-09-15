using Confluent.Kafka;
using Devon4Net.Infrastructure.Kafka.Common.Const;
using Devon4Net.Infrastructure.Kafka.Common.Extensions;
using Devon4Net.Infrastructure.Kafka.Options;
using Microsoft.Extensions.Hosting;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.Metrics;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;

namespace Devon4Net.Infrastructure.Kafka.Streams.Services
{
    public abstract class KafkaStreamService<TInput, TOutput> : BackgroundService where TOutput : class where TInput : class
    {
        protected string ApplicationId { get; set; }
        protected IStreamConfig Configuration { get; set; }
        protected StreamBuilder StreamBuilder { get; set; }
        protected StreamOptions StreamOptions { get; set; }
        protected KafkaStream Stream { get; set; }
        public abstract void CreateStreamBuilder(ref IKStream<TInput, TOutput> stream);

        public KafkaStreamService(KafkaOptions kafkaOptions, string applicationId)
        {
            ApplicationId = applicationId;
            Configuration = GetConfigFromOptions(kafkaOptions);
            GenerateStreamBuilder();
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Stream.StartAsync();
        }

        public override void Dispose()
        {
            Stream.Dispose();
            base.Dispose();
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        private void GenerateStreamBuilder()
        {
            StreamBuilder = new StreamBuilder();
            var topics = new List<string>(StreamOptions.GetTopics());

            var stream = StreamBuilder.Stream<TInput, TOutput>(topics.PopOrDefault());

            if (StreamOptions.AllowMultipleTopics) MergeMultipleStreams(ref stream, topics);

            CreateStreamBuilder(ref stream);

            Stream = new KafkaStream(StreamBuilder.Build(), Configuration);
        }

        private void MergeMultipleStreams(ref IKStream<TInput, TOutput> stream, List<string> topics)
        {
            while (topics.Count > 0)
            {
                string topic = topics.PopOrDefault();
                stream.Merge(StreamBuilder.Stream<TInput, TOutput>(topic));
            }
        }


        private IStreamConfig GetConfigFromOptions(KafkaOptions kafkaOptions)
        {
            StreamOptions = kafkaOptions.Streams.Find(s => s.ApplicationId == ApplicationId);
            var config = new StreamConfig<StringSerDes, StringSerDes>();
            
            config.ApplicationId = StreamOptions.ApplicationId;
            config.BootstrapServers = StreamOptions.Servers;
            config.AutoOffsetReset = GetAutoOffsetReset(StreamOptions.AutoOffsetReset);
            config.StateDir = StreamOptions.StateDir;
            config.CommitIntervalMs = StreamOptions.CommitIntervalMs ?? KafkaDefaultValues.StreamsCommitIntervalMs;
            config.Guarantee = GetProcessingGuarantee(StreamOptions.Guarantee);
            config.MetricsRecording = GetMetricsRecordingLevel(StreamOptions.MetricsRecording);

            return config;
        }

        private static AutoOffsetReset GetAutoOffsetReset(string autoOffsetReset)
        {
            return autoOffsetReset.ToLower() switch
            {
                "latest" => AutoOffsetReset.Latest,
                "earliest" => AutoOffsetReset.Earliest,
                "error" => AutoOffsetReset.Error,
                _ => AutoOffsetReset.Latest
            };
        }
        private static ProcessingGuarantee GetProcessingGuarantee(string processingGuarantee)
        {
            return processingGuarantee.ToLower() switch
            {
                "at_least_one" => ProcessingGuarantee.AT_LEAST_ONCE,
                "exactly_one" => ProcessingGuarantee.EXACTLY_ONCE,
                _ => ProcessingGuarantee.AT_LEAST_ONCE
            };
        }
        private static MetricsRecordingLevel GetMetricsRecordingLevel(string metricsRecordingLevel)
        {
            return metricsRecordingLevel.ToLower() switch
            {
                "info" => MetricsRecordingLevel.INFO,
                "debug" => MetricsRecordingLevel.DEBUG,
                _ => MetricsRecordingLevel.DEBUG
            };
        }

    }
}