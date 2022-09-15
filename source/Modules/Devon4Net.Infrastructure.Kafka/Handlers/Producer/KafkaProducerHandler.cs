using Confluent.Kafka;
using Devon4Net.Infrastructure.Kafka.Common.Const;
using Devon4Net.Infrastructure.Kafka.Common.Converters;
using Devon4Net.Infrastructure.Kafka.Exceptions;
using Devon4Net.Infrastructure.Kafka.Options;
using Devon4Net.Infrastructure.Logger.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Devon4Net.Infrastructure.Kafka.Handlers.Producer
{
    public class KafkaProducerHandler<T, TV> : IKafkaProducerHandler<T, TV> where T : class where TV : class
    {
        private KafkaOptions KafkaOptions { get; }
        protected IServiceCollection Services { get; set; }
        private string ProducerId { get; }

        public KafkaProducerHandler(IServiceCollection services, KafkaOptions kafkaOptions, string producerId)
        {
            Services = services;
            KafkaOptions = kafkaOptions;
            ProducerId = producerId;
        }

        public IProducer<T, TV> GetProducerBuilder<T, TV>(string producerId) where T : class where TV : class
        {
            var producerOptions = GetProducerOptions(producerId);

            var configuration = GetDefaultKafkaProducerConfiguration(producerOptions);

            var producer = GetProducerBuilderInstance<T, TV>(configuration);

            var result = producer.Build();

            return result;
        }

        #region ProcucerConfiguration

        private static ProducerBuilder<T, TV> GetProducerBuilderInstance<T, TV>(ProducerConfig configuration) where T : class where TV : class
        {
            var producer = new ProducerBuilder<T, TV>(configuration);

            producer = producer.SetErrorHandler((_, e) => Devon4NetLogger.Error(new ConsumerException($"Error code {e.Code} : {e.Reason}")));
            producer = producer.SetStatisticsHandler((_, json) => Devon4NetLogger.Information($"Statistics: {json}"));
            producer = producer.SetLogHandler((_, partitions) => Devon4NetLogger.Information($"Kafka log handler: [{string.Join(", ", partitions)}]"));
            return producer;
        }

        private ProducerOptions GetProducerOptions(string producerId)
        {
            if (string.IsNullOrEmpty(producerId))
            {
                throw new ProducerNotFoundException("The producerId param can not be null or empty");
            }

            var producerOptions = KafkaOptions.Producers.Find(p => p.ProducerId == producerId);

            if (producerOptions == null)
            {
                throw new ConsumerNotFoundException($"Could not find producer configuration with ConsumerId {producerId}");
            }

            return producerOptions;
        }

        private static ProducerConfig GetDefaultKafkaProducerConfiguration(ProducerOptions producer)
        {
            var result = new ProducerConfig
            {
                BootstrapServers = producer.Servers,
                ClientId = producer.ClientId,
                CompressionLevel = producer.CompressionLevel ?? KafkaDefaultValues.CompressionLevel,
                CompressionType = KafkaConverters.GetCompressionType(producer.CompressionType),
                EnableSslCertificateVerification = producer.EnableSslCertificateVerification,
                CancellationDelayMaxMs = producer.CancellationDelayMaxMs ?? KafkaDefaultValues.CancellationDelayMaxMs,
                Acks = KafkaConverters.GetAck(producer.Ack),
                BrokerAddressTtl = producer.BrokerAddressTtl ?? KafkaDefaultValues.BrokerAddressTtl,
                BatchNumMessages = producer.BatchNumMessages ?? KafkaDefaultValues.BatchNumMessages,
                EnableIdempotence = producer.EnableIdempotence,
                MaxInFlight = producer.MaxInFlight ?? KafkaDefaultValues.MaxInFlight,
                MessageSendMaxRetries = producer.MessageSendMaxRetries ?? KafkaDefaultValues.MessageSendMaxRetries,
                BatchSize = producer.BatchSize ?? KafkaDefaultValues.BatchSize,
                MessageMaxBytes = producer.MessageMaxBytes ?? KafkaDefaultValues.MessageMaxBytes,
                ReceiveMessageMaxBytes = producer.ReceiveMessageMaxBytes ?? KafkaDefaultValues.ReceiveMessageMaxBytes
            };

            if (!string.IsNullOrEmpty(producer.Debug))
            {
                result.Debug = producer.Debug;
            }

            return result;
        }

        #endregion

        public Task<DeliveryResult<T, TV>> SendMessage(T key, TV value)
        {
            var result = DeliverMessage(key, value, ProducerId);
            var date = result.Result.Timestamp.UtcDateTime;
            Devon4NetLogger.Information($"Message delivered. Key: {result.Result.Key} | Value : {result.Result.Value} | Topic: {result.Result.Topic} | UTC TimeStamp : {date.ToShortDateString()}-{date.ToLongTimeString()} | Status: {result.Result.Status}");
            return result;
        }

        public async Task<DeliveryResult<T, TV>> DeliverMessage<T, TV>(T key, TV value, string producerId) where T : class where TV : class
        {
            DeliveryResult<T, TV> result;
            var producerOptions = GetProducerOptions(producerId);
            using var producer = GetProducerBuilder<T, TV>(producerId);
            try
            {
                result = await producer.ProduceAsync(producerOptions.Topic, new Message<T, TV> { Key = key, Value = value }).ConfigureAwait(false);
            }
            catch (ProduceException<string, string> e)
            {
                Devon4NetLogger.Error(e);
                throw;
            }
            finally
            {
                producer?.Flush();
                producer?.Dispose();
            }

            return result;
        }

        public TS GetInstance<TS>()
        {
            var sp = Services.BuildServiceProvider();
            return sp.GetService<TS>();
        }
    }
}