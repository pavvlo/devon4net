using System.Linq.Expressions;
using Devon4Net.Domain.UnitOfWork.Service;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Devon4Net.Infrastructure.Logger.Logging;
using Devon4Net.Application.Kafka.Consumer.Domain.Database;
using Devon4Net.Application.Kafka.Consumer.Domain.RepositoryInterfaces;
using Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Dto;

namespace Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Service
{
    /// <summary>
    /// Employee service implementation
    /// </summary>
    public class FileService: Service<FileContext>, IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IUnitOfWork<FileContext> uoW) : base(uoW)
        {
            _fileRepository = uoW.Repository<IFileRepository>();
        }

        public IList<string> GetDistinctFileGuids()
        {
            var result = _fileRepository.GetDistinctFileGuids();
            return result;
        }

        public IList<DataPiece<byte[]>> GetPiecesByFileGuid(string guid)
        {
            var result = _fileRepository.GetPiecesByFileGuid(guid);
            return (IList<DataPiece<byte[]>>)result;
        }

        public bool IsFileComplete(string guid)
        {
            if (IsFileComplete(guid)) return true;
            return false;
        }
    }
}
