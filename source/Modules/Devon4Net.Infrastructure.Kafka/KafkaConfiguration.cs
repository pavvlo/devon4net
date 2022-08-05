﻿using Devon4Net.Infrastructure.Common.Handlers;
using Devon4Net.Infrastructure.Kafka.Handlers;
using Devon4Net.Infrastructure.Kafka.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        public static void AddKafkaConsumer<T>(this IServiceCollection services, string consumerId, bool commit = false, int commitPeriod = 5) where T : class
        {
            var memberInfo = typeof(T).BaseType;
            if (memberInfo?.Name.Contains("KafkaConsumerHandler") == false)
            {
                throw new ArgumentException($"The provided type {typeof(T).FullName} does not inherit from KafkaConsumerHandler");
            }

            using var sp = services.BuildServiceProvider();
            var kafHandler = sp.GetService<IKafkaHandler>();

            var obj = (T)Activator.CreateInstance(typeof(T), services, kafHandler, consumerId, commit, commitPeriod);

            services.AddSingleton(obj);
        }

        public static void AddKafkaProducer<T>(this IServiceCollection services, string producerId) where T : class
        {
            var memberInfo = typeof(T).BaseType;
            if (memberInfo?.Name.Contains("KafkaProducerHandler") == false)
            {
                throw new ArgumentException($"The provided type {typeof(T).FullName} does not inherit from KafkaProducerHandler");
            }

            using var sp = services.BuildServiceProvider();
            var kafHandler = sp.GetService<IKafkaHandler>();

            var obj = (T)Activator.CreateInstance(typeof(T), services, kafHandler, producerId);

            services.AddSingleton(obj);
        }

    }
}