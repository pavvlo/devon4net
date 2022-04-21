using Devon4Net.Infrastructure.MongoDb.Common;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    [MongoDatabase("School")]
    public class Subject
    {
        public string Name { get; set; }
        public string Teacher { get; set; }
    }
}
