using AutoMapper;
using Devon4Net.Infrastructure.Test;


namespace Devon4Net.Application.XUnit.Test.UnitTest
{
    public class UnitTest : BaseManagementTest
    {
        public override void ConfigureMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile());
            });
            Mapper = mockMapper.CreateMapper();
        }
    }
}
