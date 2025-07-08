using Xunit;
using CleverCore.Utilities.Dtos;

namespace CleverCore.Utilities.Test.Dtos
{
    public class GenericResultTest
    {
        [Fact]
        public void Contructor_CreateObjectNoParam_NotNull()
        {
            var genericResult = new GenericResult();
            Assert.NotNull(genericResult);
        }

        [Fact]
        public void Contructor_CreateObjectOneParam_SuccessIsTrue()
        {
            var genericResult = new GenericResult(true);
            Assert.True(genericResult.Success);
        }

        [Fact]
        public void Contructor_CreateObjectTwoParam_SuccessAndDataHasValue()
        {
            var genericResult = new GenericResult(true, new object());
            Assert.NotNull(genericResult.Data);
        }

        [Fact]
        public void Contructor_CreateObjectTwoParam_SuccessAndMessageHasValue()
        {
            var genericResult = new GenericResult(true, "test");
            Assert.Equal("test", genericResult.Message);
            Assert.True(genericResult.Success);
        }
    }
}
