using Devon4Net.Infrastructure.Logger.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Devon4Net.Infrastructure.MongoDb.Repository;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    /// <summary>
    /// TODOs controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IRepository<Student> _studentRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="studentRepository"></param>
        /// 

        public StudentController(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
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
        public async Task<ActionResult> GetStudents()
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            var result = await _studentRepository.Get();
            return Ok(result);
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
        public async Task<ActionResult> Create(Student student)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            await _studentRepository.Create(student);
            return Ok("Se ha creado el estudiante" + student.Name);
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
        public async Task <ActionResult> Delete(Student student)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController"); 
            return Ok(await _studentRepository.Delete(student));
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
        public async Task <ActionResult> ReplaceStudent(Student student)
        {
            Devon4NetLogger.Debug("Executing ModifyTodo from controller TodoController");
            await _studentRepository.Replace(student);
            return Ok("Se ha modificado el estudiante" + student.Name);
        }

        /// <summary>
        /// Modifies the done status of the object provided the id
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("/v2")]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpOptions]
        public async Task<ActionResult> UpdateStudent(Student student)
        {
            Devon4NetLogger.Debug("Executing ModifyTodo from controller TodoController");
            await _studentRepository.Update(student);
            return Ok("Se ha modificado el estudiante " + student.Name);
        }
    }
}
