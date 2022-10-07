using Devon4Net.Application.Kafka.Producer.Business.FileManagement.Dto;
using Devon4Net.Application.Kafka.Producer.Business.FileManagement.Helpers;
using Devon4Net.Application.Kafka.Producer.Business.KafkaManagement.Handlers;
using Devon4Net.Infrastructure.Logger.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Application.Kafka.Producer.Business.FileManagement.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        public MessageProducerHandler Producer { get; set; }

        public FileController(MessageProducerHandler messageProducer)
        {
            Producer = messageProducer;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var dataPieces = FileHelper.GetDataPieces(file);
                foreach(var piece in dataPieces)
                {
                    await Producer.SendMessage("jsonKey", piece);
                }
                //await ReadPiecesAndWriteToFile(dataPieces, @"C:\Projects\devon4net\POC\Json");
                
            }
            catch (Exception e)
            {
                Devon4NetLogger.Error(e.Message);
                return BadRequest();
            }
            
            return Ok();
        }

        private async Task ReadPiecesAndWriteToFile(IEnumerable<DataPiece<byte[]>> pieces, string directoryPath, string fileName = "output", int byteChunks = 2048)
        {
            using (var fileStream = new FileStream(@$"{directoryPath}\{pieces.First().FileName}.{pieces.First().FileExtension}", FileMode.Create))
            {
                foreach (var piece in pieces)
                {
                    await fileStream.WriteAsync(piece.Data, 0, piece.Data.Length);
                }
            }
        }

        private async void ReadPiecesAndWriteToFileAsync(IEnumerable<DataPiece<byte[]>> pieces, string directoryPath, string fileName = "output", int byteChunks = 2048)
        {
            var taskList = new List<Task>();
            foreach (var piece in pieces)
            {
                //taskList.Append(ReadPieceAndWriteToFile(piece, directoryPath));
            }
            await Task.WhenAll(taskList);
        }

    }
}
