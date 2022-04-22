using Devon4Net.Infrastructure.MongoDb.Common;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    public class Student : MongoEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public Subject Subject { get; set; }
    }
}
