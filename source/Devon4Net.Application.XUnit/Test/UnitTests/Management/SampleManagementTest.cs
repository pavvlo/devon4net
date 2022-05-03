using Xunit;

/// <summary>
/// You can create your unit test using this class
/// </summary>
namespace Devon4Net.Application.XUnit.Test.UnitTests.Management
{
    public class SampleManagementTest : UnitTest
    {
        public SampleManagementTest()
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
            Assert.NotEqual(5, Add(2, 2));
        }


        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }

        [Theory]
        [InlineData(6)]
        public void MySecondTheory(int value)
        {
            Assert.False(IsOdd(value));
        }

        private int Add(int x, int y)
        {
            return x + y;
        }

        private bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }

}
