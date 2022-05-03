using Devon4Net.Application.WebAPI.Implementation.Business.EmployeeManagement.Service;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using Devon4Net.Application.WebAPI.Implementation.Domain.RepositoryInterfaces;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// You can create your unit test using this class
/// </summary>
namespace Devon4Net.Application.XUnit.Test.UnitTests.Management
{
    public class EmployeeServiceTest : UnitTest
    {
        public IEmployeeService employeeService { get; set; }

        public EmployeeServiceTest()
        {
            var uow = GetMockedUow(GetMockedRepository());
            employeeService = new EmployeeService(uow);
        }

        [Theory]
        [InlineData("", "Smith", "mail@devon4net.com")]
        [InlineData("Jhon", "", "mail@devon4net.com")]
        [InlineData("Jhon", "Smith", "")]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        public async void CreateEmployee_ThrowsException(string name, string surname, string mail)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await employeeService.CreateEmployee(name, surname, mail));
        }

        [Theory]
        [InlineData("Jhon", "Smith", "mail@devon4net.com")]
        public async void CreateEmployee_Correctly(string name, string surname, string mail)
        {
            var expected = new Employee
            {
                Name = name,
                Surname = surname,
                Mail = mail
            };
            var obtained = await employeeService.CreateEmployee(name, surname, mail);

            Assert.True(expected.Equals(obtained));
            Assert.Equal(expected, obtained);
        }

        private IEmployeeRepository GetMockedRepository()
        {
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository
               .Setup(r => r.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns<string, string, string>((name, surname, mail) =>
               {
                   var employee = new Employee
                   {
                       Name = name,
                       Surname = surname,
                       Mail = mail
                   };
                   var task = new Task<Employee>(() => employee);
                   task.Start();
                   return task;
               });
            return mockRepository.Object;
        }

        private IUnitOfWork<EmployeeContext> GetMockedUow(IEmployeeRepository employeeRepository)
        {
            var mockUow = new Mock<IUnitOfWork<EmployeeContext>>();
            mockUow
                .Setup(uow => uow.Repository<IEmployeeRepository>())
                .Returns(employeeRepository);
            return mockUow.Object;
        }
    }

}
