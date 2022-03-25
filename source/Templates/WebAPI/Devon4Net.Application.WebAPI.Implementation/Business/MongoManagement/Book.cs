using Devon4Net.Infrastructure.MongoDb.Common;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    [MongoDatabase("Library")]
    public class Book : MongoEntity
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public double Price { get; set; }
    }
}
