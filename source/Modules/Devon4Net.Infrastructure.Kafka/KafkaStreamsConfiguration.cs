using Devon4Net.Infrastructure.Common.Handlers;
using Devon4Net.Infrastructure.Kafka.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Devon4Net.Infrastructure.Kafka
{
    public static class KafkaStreamsConfiguration
    {
        public static void AddKafkaStreamService<T>(this IServiceCollection services, IConfiguration configuration, string applicationId) where T : BackgroundService
        {
            var memberInfo = typeof(T).BaseType;

            if (memberInfo?.Name.Contains("KafkaStreamService") == false)
            {
                throw new ArgumentException($"The provided type {typeof(T).FullName} does not inherit from KafkaStreamService");
            }

            var kafkaOptions = services.GetTypedOptions<KafkaOptions>(configuration, "Kafka");

            services.AddHostedService(_ => (T) Activator.CreateInstance(typeof(T), kafkaOptions, applicationId));
        }

    }
}
