

namespace Devon4Net.Infrastructure.MongoDb.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDatabaseAttribute : Attribute
    {
        private string _name;

        public MongoDatabaseAttribute(string name)
        {
            _name = name;
        }

        public string Name { get { return _name; } }
    }
}
