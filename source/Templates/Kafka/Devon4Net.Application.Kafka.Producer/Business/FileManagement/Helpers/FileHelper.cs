using Devon4Net.Application.Kafka.Producer.Business.FileManagement.Dto;

namespace Devon4Net.Application.Kafka.Producer.Business.FileManagement.Helpers
{
    public static class FileHelper
    {
        public static List<DataPiece<byte[]>> GetDataPieces(IFormFile file, int byteChunks = 2048, string fileName = "output")
        {
            var totalParts = (int) Math.Ceiling((double) file.Length / byteChunks);
            var fileExtension = file.FileName.Split(".").Last();
            var dataPieces = new List<DataPiece<byte[]>>();
            var guid = Guid.NewGuid();

            using (Stream jsonReader = file.OpenReadStream())
            {
                byte[] buffer = new byte[byteChunks];
                int position = 1;
                int bytesRead;
                while ((bytesRead = jsonReader.Read(buffer, 0, byteChunks)) > 0)
                {
                    var piece = new DataPiece<byte[]>()
                    {
                        Guid = guid,
                        Data = bytesRead < byteChunks
                            ? buffer.Take(bytesRead).ToArray()
                            : buffer,
                        PieceOffset = byteChunks,
                        FileName = fileName,
                        FileExtension = fileExtension,
                        Position = position++,
                        TotalParts = totalParts
                    };
                    dataPieces.Add(piece);
                    buffer = new byte[byteChunks];
                }
            }

            return dataPieces;
        }
    }
}
