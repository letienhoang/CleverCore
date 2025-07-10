using AutoMapper;
using CleverCore.Application.AutoMapper;
using CleverCore.Application.Implementation;
using CleverCore.Application.ViewModels.Blog;
using CleverCore.Data.Entities;
using CleverCore.Data.Enums;
using CleverCore.Infrastructure.Interfaces;
using CleverCore.Utilities.Constants;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace CleverCore.Application.Test.Implementation
{
    public class ReportServiceTest
    {
        private readonly Mock<IRepository<Blog, int>> _mockBlogRepository;
        private readonly Mock<IRepository<Tag, string>> _mockTagRepository;
        private readonly Mock<IRepository<BlogTag, int>> _mockBlogTagRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public ReportServiceTest()
        {
            _mockBlogRepository = new Mock<IRepository<Blog, int>>();
            _mockTagRepository = new Mock<IRepository<Tag, string>>();
            _mockBlogTagRepository = new Mock<IRepository<BlogTag, int>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public void Add_ValidInput_SuccessResult()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DomainToViewModelMappingProfile()));
            var mapper = mockMapper.CreateMapper();

            var listTags = new List<Tag>
            {
                new Tag { Id = "test", Name = "Test", Type = CommonConstants.BlogTag },
                new Tag { Id = "blog", Name = "Blog", Type = CommonConstants.BlogTag },
                new Tag { Id = "test-blog", Name = "Test Blog", Type = CommonConstants.BlogTag }
            };

            _mockTagRepository.Setup(x => x.FindAll())
                .Returns(listTags.AsQueryable());
            _mockTagRepository.Setup(x => x.Add(It.IsAny<Tag>()));
            _mockBlogRepository.Setup(x => x.Add(It.IsAny<Blog>()));

            var blogService = new BlogService(_mockBlogRepository.Object, _mockBlogTagRepository.Object,
                _mockTagRepository.Object, _mockUnitOfWork.Object, mapper);

            var result = blogService.Add(new BlogViewModel
            {
                Id = 0,
                Name = "Test Blog",
                Status = Status.Active,
                Tags = "Test, Blog",
            });

            Assert.NotNull(result);
        }

        [Fact]
        public void GetAll_ValidQuery_ResultSuccess()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DomainToViewModelMappingProfile()));
            var mapper = mockMapper.CreateMapper();

            _mockBlogRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Blog, object>>>()))
                .Returns(new List<Blog>
                {
                    new Blog { Id = 1, Name = "Test Blog 1", Status = Status.Active, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 1, TagId = "test"}}},
                    new Blog { Id = 2, Name = "Test Blog 2", Status = Status.InActive, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 2, TagId = "blog"}}},
                    new Blog { Id = 3, Name = "Test Blog 3", Status = Status.Active, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 3, TagId = "test-blog"}}},
                    new Blog { Id = 4, Name = "Test Blog 4", Status = Status.InActive, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 4, TagId = "test"}}}
                }.AsQueryable());

            var blogService = new BlogService(_mockBlogRepository.Object, _mockBlogTagRepository.Object, _mockTagRepository.Object, _mockUnitOfWork.Object, mapper);
                
            var result = blogService.GetAll();
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void GetAllPaging_ValidQuery_ResultSuccess()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DomainToViewModelMappingProfile()));
            var mapper = mockMapper.CreateMapper();

            _mockBlogRepository.Setup(x => x.FindAll())
                .Returns(new List<Blog>
                {
                    new Blog { Id = 1, Name = "Test Blog 1", Status = Status.Active, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 1, TagId = "test"}}},
                    new Blog { Id = 2, Name = "Test Blog 2", Status = Status.InActive, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 2, TagId = "blog"}}},
                    new Blog { Id = 3, Name = "Test Blog 3", Status = Status.Active, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 3, TagId = "test-blog"}}},
                    new Blog { Id = 4, Name = "Test Blog 4", Status = Status.InActive, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 4, TagId = "test"}}},
                    new Blog { Id = 5, Name = "Test Blog 5", Status = Status.Active, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 5, TagId = "test"}}},
                }.AsQueryable());

            var blogService = new BlogService(_mockBlogRepository.Object, _mockBlogTagRepository.Object, _mockTagRepository.Object, _mockUnitOfWork.Object, mapper);

            var result = blogService.GetAllPaging(string.Empty, 2, 1);
            Assert.Equal(3, result.PageCount);
        }

        [Fact]
        public void GetById_ValidQuery_ResultSuccess()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DomainToViewModelMappingProfile()));
            var mapper = mockMapper.CreateMapper();

            _mockBlogRepository.Setup(x => x.FindById(It.IsAny<int>()))
                .Returns(new Blog { Id = 1, Name = "Test Blog 1", Status = Status.Active, BlogTags = new List<BlogTag>() { new BlogTag() { Id = 1, TagId = "test" } } });

            var blogService = new BlogService(_mockBlogRepository.Object, _mockBlogTagRepository.Object, _mockTagRepository.Object, _mockUnitOfWork.Object, mapper);

            var result = blogService.GetById(1);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetLastest_ValidQuery_ResultSuccess()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DomainToViewModelMappingProfile()));
            var mapper = mockMapper.CreateMapper();

            _mockBlogRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Blog, bool>>>()))
                .Returns(new List<Blog>
                {
                    new Blog { Id = 1, Name = "Test Blog 1", Status = Status.Active, DateCreated = DateTime.Now.AddDays(-1), BlogTags = new List<BlogTag>() {new BlogTag(){Id = 1, TagId = "test"}}},
                    new Blog { Id = 2, Name = "Test Blog 2", Status = Status.Active, DateCreated = DateTime.Now, BlogTags = new List<BlogTag>() {new BlogTag(){Id = 2, TagId = "blog"}}},
                        new Blog { Id = 3, Name = "Test Blog 3", Status = Status.Active, DateCreated = DateTime.Now.AddDays(-2), BlogTags = new List<BlogTag>() {new BlogTag(){Id = 3, TagId = "test-blog"}}},
                    new Blog { Id = 4, Name = "Test Blog 4", Status = Status.Active, DateCreated = DateTime.Now.AddDays(1), BlogTags = new List<BlogTag>() {new BlogTag(){Id = 4, TagId = "test"}}}
                }.AsQueryable());

            var blogService = new BlogService(_mockBlogRepository.Object, _mockBlogTagRepository.Object, _mockTagRepository.Object, _mockUnitOfWork.Object, mapper);

            var result = blogService.GetLastest(2);
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Blog 4", result[0].Name);
        }
    }
}