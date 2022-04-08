using AutoMapper;

namespace Devon4Net.Infrastructure.Test
{
    public abstract class BaseManagementTest 
    {
        public IMapper Mapper { get; set; }

        protected BaseManagementTest()
        {
            ConfigureMapper();
        }

        public abstract void ConfigureMapper();
    }
}
