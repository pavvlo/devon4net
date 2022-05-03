
using Xunit;

namespace Devon4Net.Application.XUnit.Test.UnitTests.Management
{

    public class AutoMapperTest : UnitTest
    {
        /// <summary>
        /// This class will test your if your AutoMapperProfile configuration is valid
        /// </summary>
        public AutoMapperTest()
        {
        }

        [Fact]
        public void MapperConfigurationIsValid()
        {
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    } 
}
