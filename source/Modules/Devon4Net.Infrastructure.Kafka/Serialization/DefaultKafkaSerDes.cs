using Confluent.Kafka;
using Streamiz.Kafka.Net.SerDes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Devon4Net.Infrastructure.Kafka.Serialization
{
    public class DefaultKafkaSerDes<T> : ISerDes<T>
    {
        public T Deserialize(byte[] data, SerializationContext context)
        {
            return (T)JsonSerializer.Deserialize(new MemoryStream(data.ToArray()), typeof(T));
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data, typeof(T));
        }

        object ISerDes.DeserializeObject(byte[] data, SerializationContext context)
        {
            return (T)JsonSerializer.Deserialize(new MemoryStream(data.ToArray()), typeof(T));
        }

        public byte[] SerializeObject(object data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data, typeof(T));
        }

        public void Initialize(SerDesContext context)
        {
        }
    }
}
