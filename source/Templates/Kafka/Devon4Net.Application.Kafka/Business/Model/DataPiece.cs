namespace Devon4Net.Application.Kafka.Business.Model
{
    public class DataPiece<T> where T : class
    {
        public int TotalParts { get; set; }
        public int Position { get; set; }
        public T Data { get; set; }
    }
}
