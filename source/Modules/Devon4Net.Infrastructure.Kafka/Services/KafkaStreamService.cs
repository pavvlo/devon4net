using Confluent.Kafka;
using Devon4Net.Infrastructure.Kafka.Common.Const;
using Devon4Net.Infrastructure.Kafka.Options;
using Microsoft.Extensions.Hosting;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.Metrics;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;

namespace Devon4Net.Infrastructure.Kafka.Streams.Services
{
    public abstract class KafkaStreamService<TInput, TOutput> : BackgroundService, IKafkaStreamService<TInput, TOutput> where TOutput : class where TInput : class
    {
        public string ApplicationId { get; set; }
        public KafkaOptions KafkaOptions { get; set; }
        public KafkaStream Stream { get; set; }
        public abstract void CreateStreamBuilder(ref IKStream<TInput, TOutput> stream);

        public KafkaStreamService(KafkaOptions kafkaOptions, string applicationId)
        {
            ApplicationId = applicationId;
            KafkaOptions = kafkaOptions;
            GenerateStreamBuilder();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Stream.StartAsync();
        }

        private void GenerateStreamBuilder()
        {
            var streamBuilder = new StreamBuilder();
            var stream = streamBuilder.Stream<TInput, TOutput>("input");

            CreateStreamBuilder(ref stream);
            
            Stream = new KafkaStream(streamBuilder.Build(), GetConfigFromOptions());
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