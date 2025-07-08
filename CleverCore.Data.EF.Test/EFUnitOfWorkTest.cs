using CleverCore.Data.Entities;
using CleverCore.Data.Enums;
using System;
using Xunit;

namespace CleverCore.Data.EF.Test
{
    public class EFUnitOfWorkTest
    {
        private readonly AppDbContext _context;

        public EFUnitOfWorkTest()
        {
            _context = ContextFactory.CreateInMemoryDbContext();
        }

        [Fact]
        public void Commit_ShouldSaveChanges()
        {
            Guid functionId = Guid.NewGuid();
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            var unitOfWork = new EFUnitOfWork(_context);
            repository.Add(new Function()
            {
                Id = functionId.ToString(),
                Name = "Test Function",
                Status = Status.Active,
                URL = "http://testfunction.com",
                IconCss = "icon-test",
                SortOrder = 1,
                ParentId = null
            });
            unitOfWork.Commit();

            var addedFunction = repository.FindById(functionId.ToString());
            Assert.NotNull(addedFunction);
        }
    }
}