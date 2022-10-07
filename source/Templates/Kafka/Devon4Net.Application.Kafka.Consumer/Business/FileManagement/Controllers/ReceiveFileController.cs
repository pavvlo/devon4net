using Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Helpers;
using Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Controllers
{
    public class ReceiveFileController : Controller
    {
        private readonly IFileService _fileService;

        public ReceiveFileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/getFilesGuids")]
        public IList<string> GetFilesGuids()
        {
            return _fileService.GetDistinctFileGuids();
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/downloadfile")]
        public async Task<IActionResult> DownloadFile(string guid)
        {
            if (_fileService.IsFileComplete(guid))
            {
                var files = _fileService.GetPiecesByFileGuid(guid).OrderBy(o => o.Position);
                await ReaderHelper.ReadPiecesAndWriteToFile(files, @"C:\Projects\devon4net\POC\Json");
                _fileService.DeleteFileByGuid(guid);
                return Ok();
            }
            return Ok("Not downloaded, file is not completed.");
        }
    }
}
