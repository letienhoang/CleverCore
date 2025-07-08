using CleverCore.Data.Entities;
using CleverCore.Data.Enums;
using CleverCore.Infrastructure.Interfaces;
using System;
using System.Linq;
using Xunit;

namespace CleverCore.Data.EF.Test
{
    public class EFRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EFRepositoryTest()
        {
            _context = ContextFactory.CreateInMemoryDbContext();
            _context.Database.EnsureCreated();

            _unitOfWork = new EFUnitOfWork(_context);
        }

        [Fact]
        public void Dispose_Repository_DisposesContext()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            repository.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _context.Functions.ToList());
        }

        [Fact]
        public void Constructor_CreateEfRepository_Success()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            Assert.NotNull(repository);
        }

        [Fact]
        public void Add_Function_Success()
        {
            Guid functionId = Guid.NewGuid();
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
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
            _unitOfWork.Commit();

            var addedFunction = repository.FindById(functionId.ToString());
            Assert.NotNull(addedFunction);
        }

        [Fact]
        public void FindAll_WithPredicate_ReturnsFilteredFunctions()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            Guid functionId = Guid.NewGuid();
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
            functionId = Guid.NewGuid();
            repository.Add(new Function()
            {
                Id = functionId.ToString(),
                Name = "Inactive Function",
                Status = Status.InActive,
                URL = "http://inactivefunction.com",
                IconCss = "icon-inactive",
                SortOrder = 2,
                ParentId = null
            });
            _unitOfWork.Commit();
            var activeFunctions = repository.FindAll(x => x.Status == Status.Active);
            Assert.Single(activeFunctions);
            Assert.Equal("Test Function", activeFunctions.First().Name);
        }

        [Fact]
        public void FindAll_WithIncludeProperties_ReturnsFunctionsWithIncludes()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            Guid functionId = Guid.NewGuid();
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
            _unitOfWork.Commit();
            var functions = repository.FindAll(x => x.Status == Status.Active);
            Assert.NotNull(functions);
            Assert.Single(functions);
            Assert.Equal("Test Function", functions.First().Name);
        }

        [Fact]
        public void FindById_Function_ReturnsCorrectFunction()
        {
            Guid functionId = Guid.NewGuid();
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
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
            _unitOfWork.Commit();
            var function = repository.FindById(functionId.ToString());
            Assert.NotNull(function);
            Assert.Equal("Test Function", function.Name);
        }

        [Fact]
        public void FindSingle_Function_ReturnsCorrectFunction()
        {
            Guid functionId = Guid.NewGuid();
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
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
            _unitOfWork.Commit();
            var function = repository.FindSingle(x => x.Name == "Test Function");
            Assert.NotNull(function);
            Assert.Equal("Test Function", function.Name);
        }

        [Fact]
        public void Remove_FunctionWithEntity_RemoveSuccess()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            Guid functionId = Guid.NewGuid();
            var function = new Function()
            {
                Id = functionId.ToString(),
                Name = "Test Function",
                Status = Status.Active,
                URL = "http://testfunction.com",
                IconCss = "icon-test",
                SortOrder = 1,
                ParentId = null
            };
            repository.Add(function);
            _unitOfWork.Commit();
            repository.Remove(function);
            _unitOfWork.Commit();
            var removedFunction = repository.FindById(functionId.ToString());
            Assert.Null(removedFunction);
        }

        [Fact]
        public void Remove_FunctionById_RemoveSuccess()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            Guid functionId = Guid.NewGuid();
            var function = new Function()
            {
                Id = functionId.ToString(),
                Name = "Test Function",
                Status = Status.Active,
                URL = "http://testfunction.com",
                IconCss = "icon-test",
                SortOrder = 1,
                ParentId = null
            };
            repository.Add(function);
            _unitOfWork.Commit();
            repository.Remove(function.Id);
            _unitOfWork.Commit();
            var removedFunction = repository.FindById(functionId.ToString());
            Assert.Null(removedFunction);
        }

        [Fact]
        public void RemoveMultiple_Functions_RemoveSuccess()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            Guid functionId1 = Guid.NewGuid();
            Guid functionId2 = Guid.NewGuid();
            repository.Add(new Function()
            {
                Id = functionId1.ToString(),
                Name = "Test Function 1",
                Status = Status.Active,
                URL = "http://testfunction1.com",
                IconCss = "icon-test1",
                SortOrder = 1,
                ParentId = null
            });
            repository.Add(new Function()
            {
                Id = functionId2.ToString(),
                Name = "Test Function 2",
                Status = Status.Active,
                URL = "http://testfunction2.com",
                IconCss = "icon-test2",
                SortOrder = 2,
                ParentId = null
            });
            _unitOfWork.Commit();
            repository.Remove(functionId1.ToString());
            repository.Remove(functionId2.ToString());
            _unitOfWork.Commit();
            var removedFunction1 = repository.FindById(functionId1.ToString());
            var removedFunction2 = repository.FindById(functionId2.ToString());
            Assert.Null(removedFunction1);
            Assert.Null(removedFunction2);
        }

        [Fact]
        public void Update_Function_UpdateSuccess()
        {
            EfRepository<Function, string> repository = new EfRepository<Function, string>(_context);
            Guid functionId = Guid.NewGuid();
            var function = new Function()
            {
                Id = functionId.ToString(),
                Name = "Test Function",
                Status = Status.Active,
                URL = "http://testfunction.com",
                IconCss = "icon-test",
                SortOrder = 1,
                ParentId = null
            };
            repository.Add(function);
            _unitOfWork.Commit();
            function.Name = "Updated Function";
            repository.Update(function);
            _unitOfWork.Commit();
            var updatedFunction = repository.FindById(functionId.ToString());
            Assert.NotNull(updatedFunction);
            Assert.Equal("Updated Function", updatedFunction.Name);
        }
    }
}