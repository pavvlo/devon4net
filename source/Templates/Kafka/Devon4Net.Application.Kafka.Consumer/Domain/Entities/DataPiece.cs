using Confluent.Kafka;
using System.Text.Json;

namespace Devon4Net.Application.Kafka.Consumer.Domain.Entities
{
    public class DataPiece<T> : IDeserializer<T>, ISerializer<T> where T : class
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string FileName { get; set; }
        public int TotalParts { get; set; }
        public string FileExtension { get; set; }
        public int PieceOffset { get; set; }
        public int Position { get; set; }
        public T Data { get; set; }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return (T) JsonSerializer.Deserialize(new MemoryStream(data.ToArray()), typeof(DataPiece<T>));
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data, typeof(DataPiece<T>));
        }
    }
}
