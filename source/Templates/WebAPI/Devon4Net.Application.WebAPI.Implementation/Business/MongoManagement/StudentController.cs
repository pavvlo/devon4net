using Devon4Net.Infrastructure.Logger.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Devon4Net.Infrastructure.MongoDb.Repository;
using MongoDB.Driver;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    /// <summary>
    /// TODOs controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IMongoDbRepository<Student, SchoolContext> _studentRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="studentRepository"></param>
        /// 

        public StudentController(IMongoDbRepository<Student, SchoolContext> studentRepository)
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
        public async Task<ActionResult> GetStudents(string name)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            var result = await _studentRepository.Get(s => s.Name == name && s.Subject == null);
            return Ok(result);
        }

        [HttpGet]
        [Route("[controller]/byId")]
        [ProducesResponseType(typeof(List<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetStudentById(string id)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            var result = await _studentRepository.Get(id);
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
        public async Task<ActionResult> Create(IEnumerable<Student> students)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            await _studentRepository.Create(students);
            return Ok("Se ha creado " + students.Count() + " estudiantes");
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
        public async Task <ActionResult> Delete(IEnumerable<Student> students)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController"); 
            return Ok(await _studentRepository.Delete(students));
        }

        [HttpDelete]
        [Route("[controller]/byName")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteByName(string name)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok(await _studentRepository.Delete(s => s.Name == name));
        }

        [HttpDelete]
        [Route("[controller]/byId")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteById(string id)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok(await _studentRepository.Delete(id));
        }

        /// <summary>
        /// Modifies the done status of the object provided the id
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("[controller]/Replace")]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpOptions]
        public async Task <ActionResult> ReplaceStudent(Student student)
        {
            Devon4NetLogger.Debug("Executing ModifyTodo from controller TodoController");
            await _studentRepository.Replace(s => s.Name == student.Name, student);
            return Ok("Se ha modificado el estudiante" + student.Name);
        }

        

        /// <summary>
        /// Modifies the done status of the object provided the id
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("[controller]/Update")]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpOptions]
        public async Task<ActionResult> UpdateStudent(Student student)
        {
            Devon4NetLogger.Debug("Executing ModifyTodo from controller TodoController");
            var builder = Builders<Student>.Filter;
            var builderUpdate = Builders<Student>.Update;
            var filter = builder.Eq(student => student.Name, "string");
            var update = builderUpdate.Set(student => student.Name, "Melania");
            await _studentRepository.Update(filter, update);
            return Ok("Se ha modificado el estudiante " + student.Name);
        }

        [HttpGet]
        [Route("[controller]/Count")]
        [ProducesResponseType(typeof(List<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCount(string subject)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            var result = await _studentRepository.Count();
            //var result2 = await _studentRepository.Count(s => s.Subject.Name == "Plastica"); 
            return Ok("Total de estudiantes: " + result.ToString() + " de los cuales en plastica: " + "result2.ToString()");
        }

        [HttpGet]
        [Route("[controller]/EstimatedCount")]
        [ProducesResponseType(typeof(List<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetEstimatedCount()
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            var result = await _studentRepository.EstimateCount();
            return Ok(result);
        }
    }
}
