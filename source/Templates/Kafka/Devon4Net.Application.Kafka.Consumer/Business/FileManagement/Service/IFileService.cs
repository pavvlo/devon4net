
using Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Dto;
using System.Linq.Expressions;


namespace Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Service
{
    public interface IFileService
    {
        public IList<string> GetDistinctFileGuids();
        public IList<DataPiece<byte[]>> GetPiecesByFileGuid(string guid);
        public bool IsFileComplete(string guid);
    }
}