using Xunit;

namespace CleverCore.Data.EF.Test
{
    public class AppDbContextTest
    {
        [Fact]
        public void Constructor_CreateInMemoryDb_Success()
        {
            var context = ContextFactory.CreateInMemoryDbContext();

            Assert.True(context.Database.EnsureCreated());
        }
    }
}