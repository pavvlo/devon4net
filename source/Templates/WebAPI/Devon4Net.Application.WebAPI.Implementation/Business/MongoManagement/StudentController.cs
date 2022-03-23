using Devon4Net.Infrastructure.Logger.Logging;
using Devon4Net.Application.WebAPI.Implementation.Business.TodoManagement.Dto;
using Devon4Net.Application.WebAPI.Implementation.Business.TodoManagement.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement;

namespace Devon4Net.Application.WebAPI.Implementation.Business.TodoManagement.Controllers
{
    /// <summary>
    /// TODOs controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name=""></param>
        public StudentController()
        {
        }

        /// <summary>
        /// Gets the entire list of Students
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetStudents()
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok();
        }

        /// <summary>
        /// Gets the entire list of Student
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetStudent(string studentId)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok();
        }

        /// <summary>
        /// Creates the object
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Student), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Create(Student student)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok();
        }

        /// <summary>
        /// Deletes the object provided the id
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string studentId)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok();
        }

        /// <summary>
        /// Modifies the done status of the object provided the id
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpOptions]
        public ActionResult ModifyStudent(Student student)
        {
            Devon4NetLogger.Debug("Executing ModifyTodo from controller TodoController");
            return Ok();
        }
    }
}
