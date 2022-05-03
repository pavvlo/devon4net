using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using System.Linq;
using Xunit;

namespace Devon4Net.Application.XUnit.Test.IntegrationTests.Management
{
    public class EmployeeManagementTest : IntegrationTest
    {
        public EmployeeManagementTest()
        {
        }

        [Fact]
        public async void DatabaseInsertTest()
        {
            var initialQuantity = Context.Employee.ToList().Count();

            Context.Employee.Add(new Employee
            {
                Id = 1,
                Name = "Jhon",
                Surname = "Smith",
                Mail = "mail@devon4net.com"
            });

            await Context.SaveChangesAsync();

            var finalQuantity = Context.Employee.ToList().Count();

            Assert.True(initialQuantity < finalQuantity);
        }
    }
}
