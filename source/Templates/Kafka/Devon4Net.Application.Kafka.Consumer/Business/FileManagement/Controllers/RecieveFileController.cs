using Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Dto;
using Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Helpers;
using Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Application.Kafka.Consumer.Business.FileManagement.Controllers
{
    public class RecieveFileController : Controller
    {
        private readonly IFileService _fileService;

        public RecieveFileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("/v1/kafka/downloadfile")]
        public async Task<IActionResult> DownloadFile()
        {
            var guids = _fileService.GetDistinctFileGuids();
            foreach (var guid in guids)
            {
                await ReaderHelper.ReadPiecesAndWriteToFile(_fileService.GetPiecesByFileGuid(guid), @"C:\Proyectos\pavlo-fork-devon4net\POC");
            }
            return Ok();
        }
    }
}
