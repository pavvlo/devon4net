using Devon4Net.Infrastructure.Common.Handlers;
using Devon4Net.Infrastructure.Kafka.Handlers;
using Devon4Net.Infrastructure.Kafka.Handlers.Consumer;
using Devon4Net.Infrastructure.Kafka.Handlers.Producer;
using Devon4Net.Infrastructure.Kafka.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Devon4Net.Infrastructure.Kafka
{
    public static class KafkaConfiguration
    {
        public static void SetupKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var kafkaOptions = services.GetTypedOptions<KafkaOptions>(configuration, "Kafka");

            if (kafkaOptions?.EnableKafka != true || kafkaOptions.Producers?.Any() != true) return;

            services.AddTransient(typeof(IKafkaHandler), typeof(KafkaHandler));
        }

        public static void AddKafkaConsumer<T>(this IServiceCollection services, IConfiguration configuration, string consumerId, bool commit = false, int commitPeriod = 5) where T : class
        {
            var memberInfo = typeof(T).BaseType;
            if (memberInfo?.Name.Contains("KafkaConsumerHandler") == false)
            {
                throw new ArgumentException($"The provided type {typeof(T).FullName} does not inherit from KafkaConsumerHandler");
            }

            var kafkaOptions = services.GetTypedOptions<KafkaOptions>(configuration, "Kafka");

            services.AddSingleton(_=> (T)Activator.CreateInstance(typeof(T), services, consumerId, commit, commitPeriod));
        }

        public static void AddKafkaProducer<T>(this IServiceCollection services, IConfiguration configuration, string producerId) where T : class
        {
            var memberInfo = typeof(T).BaseType;
            if (memberInfo?.Name.Contains("KafkaProducerHandler") == false)
            {
                throw new ArgumentException($"The provided type {typeof(T).FullName} does not inherit from KafkaProducerHandler");
            }

            var kafkaOptions = services.GetTypedOptions<KafkaOptions>(configuration, "Kafka");

            services.AddSingleton(_ => (T)Activator.CreateInstance(typeof(T), services, kafkaOptions, producerId));
        }

        public static void AddKafkaStreamService<T>(this IServiceCollection services, IConfiguration configuration, string applicationId) where T : BackgroundService
        {
            var memberInfo = typeof(T).BaseType;

            if (memberInfo?.Name.Contains("KafkaStreamService") == false)
            {
                throw new ArgumentException($"The provided type {typeof(T).FullName} does not inherit from KafkaStreamService");
            }

            var kafkaOptions = services.GetTypedOptions<KafkaOptions>(configuration, "Kafka");

            services.AddHostedService(_ => (T)Activator.CreateInstance(typeof(T), kafkaOptions, applicationId));
        }

    }
}