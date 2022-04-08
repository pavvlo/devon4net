using Xunit;
/// <summary>
/// You can create your unit test using this class
/// </summary>
namespace Devon4Net.Application.XUnit.Test.UnitTest.Management
{
    public class ManagementTest:  UnitTest
    {
        public ManagementTest()
        {
        }

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }

        bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }

}
