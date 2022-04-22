using Devon4Net.Infrastructure.Common.Options.MongoDb;
using Devon4Net.Infrastructure.MongoDb.MongoDb;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devon4Net.Application.WebAPI.Implementation.Business.MongoManagement
{
    public class SchoolContext : MongoDbContext
    {
        public SchoolContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ConfigureDatabase("School");
        }
    }
}
