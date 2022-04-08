using Devon4Net.Application.NUnit.Test.Integration;
using Devon4Net.Application.WebAPI.Implementation.Business.EmployeeManagement.Service;
using Devon4Net.Application.XUnit.Database;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;

namespace Devon4Net.Application.XUnit.Test.UnitTest.Management
{
    public class EmployeeManagementTest : IntegrationTest
    {
        public EmployeeService EmployeeService { get; set; }

        public UnitOfWork<ModelContext> UoW { get; set; }

        public EmployeeManagementTest()
        {
            UoW = new UnitOfWork<ModelContext>(Context, null);
            EmployeeService = new EmployeeService(UoW);
        }
    }
}
