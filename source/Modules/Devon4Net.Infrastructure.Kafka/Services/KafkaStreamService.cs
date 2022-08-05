using Confluent.Kafka;
using Devon4Net.Infrastructure.Kafka.Common.Const;
using Devon4Net.Infrastructure.Kafka.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.Metrics;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;

namespace Devon4Net.Infrastructure.Kafka.Streams.Services
{
    public abstract class KafkaStreamService : BackgroundService
    {
        public KafkaOptions KafkaOptions { get; set; }
        public string ApplicationId { get; set; }

        public abstract StreamBuilder CreateStreamBuilder();
        public abstract string GetApplicationId();

        public KafkaStreamService(IOptions<KafkaOptions> kafkaOptions)
        {
            KafkaOptions = kafkaOptions?.Value;
            ApplicationId = GetApplicationId();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StreamBuilder builder = CreateStreamBuilder();
            Topology topic = builder.Build();
            KafkaStream stream = new KafkaStream(topic, GetConfigFromOptions());

            await stream.StartAsync();
        }

        private IStreamConfig GetConfigFromOptions()
        {
            var streamOptions = KafkaOptions.Streams.Find(s => s.ApplicationId == ApplicationId);

            var config = new StreamConfig<StringSerDes, StringSerDes>();
            config.ApplicationId = streamOptions.ApplicationId;
            config.BootstrapServers = streamOptions.Servers;
            config.AutoOffsetReset = GetAutoOffsetReset(streamOptions.AutoOffsetReset);
            config.StateDir = streamOptions.StateDir;
            config.CommitIntervalMs = streamOptions.CommitIntervalMs ?? KafkaDefaultValues.StreamsCommitIntervalMs;
            config.Guarantee = GetProcessingGuarantee(streamOptions.Guarantee);
            config.MetricsRecording = GetMetricsRecordingLevel(streamOptions.MetricsRecording);
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