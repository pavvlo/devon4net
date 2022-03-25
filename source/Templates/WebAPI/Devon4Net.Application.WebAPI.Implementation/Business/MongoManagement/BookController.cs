using Devon4Net.Infrastructure.Logger.Logging;
using Devon4Net.Infrastructure.MongoDb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bookRepository"></param>
        /// 

        public BookController(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Gets the entire list of Students
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetStudents()
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            var result = await _bookRepository.Get();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create(Book book)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            await _bookRepository.Create(book);
            return Ok("Se ha creado el libro");
        }
    }
}
