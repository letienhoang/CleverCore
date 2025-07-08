using System;
using Xunit;
using CleverCore.Utilities.Dtos;

namespace CleverCore.Utilities.Test.Dtos
{
    public class PagedResultTest
    {
        [Fact]
        public void Constructor_CreateObjectNotNull_NoParam()
        {
            var pagedResult = new PagedResult<Array>();
            Assert.NotNull(pagedResult);
        }

        [Fact]
        public void Constructor_CreateObjectWithResultsIsEmptyList_NoParam()
        {
            var pagedResult = new PagedResult<Array>();
            Assert.NotNull(pagedResult.Results);
            Assert.Empty(pagedResult.Results);
        }
    }
}
