using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devon4Net.Infrastructure.Kafka.Common.Converters
{
    public static class KafkaConverters
    {
        public static Acks? GetAck(string producerAck)
        {
            return producerAck.ToLower() switch
            {
                "gzip" => Acks.None,
                "all" => Acks.All,
                "leader" => Acks.Leader,
                "none" => Acks.None,
                _ => Acks.None
            };
        }

        public static CompressionType? GetCompressionType(string producerCompressionType)
        {
            return producerCompressionType.ToLower() switch
            {
                "gzip" => CompressionType.Gzip,
                "snappy" => CompressionType.Snappy,
                "lz4" => CompressionType.Lz4,
                "zstd" => CompressionType.Zstd,
                "none" => CompressionType.None,
                _ => CompressionType.None
            };
        }

        public static AutoOffsetReset? GetAutoOffsetReset(string consumerAutoOffsetReset)
        {
            return consumerAutoOffsetReset.ToLower() switch
            {
                "latest" => AutoOffsetReset.Latest,
                "earliest" => AutoOffsetReset.Earliest,
                "error" => AutoOffsetReset.Error,
                _ => AutoOffsetReset.Latest
            };
        }

        public static IsolationLevel GetIsolationLevel(string isolationLevel)
        {
            return isolationLevel.ToLower() switch
            {
                "readuncommitted" => IsolationLevel.ReadUncommitted,
                "readcommitted" => IsolationLevel.ReadCommitted,
                _ => IsolationLevel.ReadCommitted
            };
        }
    }
}
