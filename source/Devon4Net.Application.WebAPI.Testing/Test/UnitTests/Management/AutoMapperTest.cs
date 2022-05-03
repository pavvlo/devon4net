
using Xunit;

namespace Devon4Net.Application.XUnit.Test.UnitTests.Management
{

    public class AutoMapperTest : UnitTest
    {
        /// <summary>
        /// This will test your AutoMapperProfile configuration
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
