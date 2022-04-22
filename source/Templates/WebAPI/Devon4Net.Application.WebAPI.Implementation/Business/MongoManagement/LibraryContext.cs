using Devon4Net.Infrastructure.MongoDb.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    public class LibraryContext : MongoDbContext
    {
        public LibraryContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ConfigureDatabase("Library");
        }
    }
}
