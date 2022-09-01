using Devon4Net.Application.Keycloak.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Application.Keycloak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        [HttpGet]
        [Route("GetStudentSubjects")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<string> GetStudentSubjects()
        {
            return new string[] { "Math", "History", "Java" };
        }

        // Method only for Role "Administrator"
        [HttpGet]
        [Route("GetTeacherStudents")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "SchoolPolicy")]
        public IEnumerable<Student> GetTeacherStudents(int teacherId)
        {
            var result = new List<Student>() {
                new Student
                {
                    Id = 1,
                    Age = 17,
                    Name = "Paul",
                    Surname = "Mccartney",
                    Subjects = new List<string>() {"Math", "History", "Java"}
                },
                new Student
                {
                    Id = 2,
                    Age = 16,
                    Name = "Melany",
                    Surname = "Friedrich",
                    Subjects =  new List<string>() {"Math", "Biology", "Python"}
                },
                new Student
                {
                    Id = 3,
                    Age = 17,
                    Name = "George",
                    Surname = "Washington",
                    Subjects = new List<string>() {"Biology", "Chemistry", "C#", "Java"}
                }
            };
            return result;
        }
    }
}
