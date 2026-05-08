using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AuthorService.Api.GraphQL;
using AuthorService.Application.DTO;
using AuthorService.Application.Interfaces;

namespace AuthorService.Tests.AuthorService.GraphQL
{
    public class AuthorQueryTests
    {
        private readonly Mock<IAuthorService> _authorMockService;
        private readonly AuthorQuery _query; // System Under Test

        public AuthorQueryTests()
        {
            // 1. Arrange the Mock
            _authorMockService = new Mock<IAuthorService>();

            // 2. Initialize the Query class (No constructor dependencies!)
            _query = new AuthorQuery();
        }

        // ----- Tests for GetAuthorById -----
        [Fact]
        public async Task GetAuthorById_Success_ReturnsAuthorDto()
        {
            // Arrange
            int testId = 1;
            var expectedDto = new AuthorDto(testId, "Kavita Bhatnagar", "Kavita", "Bhatnagar", "Bhatnagar, Kavita", null, null, null, null, null);

            _authorMockService.Setup(svc => svc.GetAuthorByIdAsync(testId))
                              .ReturnsAsync(expectedDto);

            // Act -> pass the Mock object directly into method parameter
            var result = await _query.GetAuthorById(_authorMockService.Object, testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testId, result.AuthorId);
            Assert.Equal("Kavita Bhatnagar", result.AuthorName);
        }

        [Fact]
        public async Task GetAuthorById_Failure_ReturnsNullWhenNotFound()
        {
            // Arrange
            int testId = 99;
            _authorMockService.Setup(svc => svc.GetAuthorByIdAsync(testId))
                              .ReturnsAsync((AuthorDto?)null);

            // Act
            var result = await _query.GetAuthorById(_authorMockService.Object, testId);

            // Assert
            Assert.Null(result); // GraphQL translates this to a null payload
        }

        [Fact]
        public async Task GetAuthorById_Error_AllowsExceptionToPropagate()
        {
            // Arrange
            int testId = 1;
            _authorMockService.Setup(svc => svc.GetAuthorByIdAsync(testId))
                              .ThrowsAsync(new Exception("Service timeout"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _query.GetAuthorById(_authorMockService.Object, testId));
            Assert.Equal("Service timeout", exception.Message);
        }


        // ------ Tests for GetAuthorsByName -----

        [Fact]
        public async Task GetAuthorsByName_Success_ReturnsAuthorDto()
        {
            // Arrange
            string testName = "JK Rowling";
            var expectedDto = new AuthorDto(5, testName, "JK", "Rowling", "Rowling, JK", null, null, null, null, null);

            _authorMockService.Setup(svc => svc.GetAuthorByNameAsync(testName))
                              .ReturnsAsync(expectedDto);

            // Act
            var result = await _query.GetAuthorsByName(_authorMockService.Object, testName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testName, result.AuthorName);
        }

        [Fact]
        public async Task GetAuthorsByName_Failure_ReturnsNullWhenNotFound()
        {
            // Arrange
            string testName = "Unknown Author";
            _authorMockService.Setup(svc => svc.GetAuthorByNameAsync(testName))
                              .ReturnsAsync((AuthorDto?)null);

            // Act
            var result = await _query.GetAuthorsByName(_authorMockService.Object, testName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAuthorsByName_Error_AllowsExceptionToPropagate()
        {
            // Arrange
            string testName = "JK Rowling";
            _authorMockService.Setup(svc => svc.GetAuthorByNameAsync(testName))
                              .ThrowsAsync(new Exception("Database locked"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _query.GetAuthorsByName(_authorMockService.Object, testName));
            Assert.Equal("Database locked", exception.Message);
        }


        // ----- Tests for GetFeaturedAuthors -----

        [Fact]
        public async Task GetFeaturedAuthors_Success_ReturnsListOfAuthorDtos()
        {
            // Arrange
            int topCount = 3;
            var expectedList = new List<AuthorDto>
            {
                new AuthorDto(1, "Author One", null, null, null, null, null, null, null, null),
                new AuthorDto(2, "Author Two", null, null, null, null, null, null, null, null)
            };

            _authorMockService.Setup(svc => svc.GetFeaturedAuthorsAsync(topCount))
                              .ReturnsAsync(expectedList);

            // Act
            var result = await _query.GetFeaturedAuthors(_authorMockService.Object, topCount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<AuthorDto>>(result);
        }

        [Fact]
        public async Task GetFeaturedAuthors_Failure_ReturnsEmptyList()
        {
            // Arrange
            int topCount = 5;
            _authorMockService.Setup(svc => svc.GetFeaturedAuthorsAsync(topCount))
                              .ReturnsAsync(new List<AuthorDto>());

            // Act
            var result = await _query.GetFeaturedAuthors(_authorMockService.Object, topCount);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFeaturedAuthors_Error_AllowsExceptionToPropagate()
        {
            // Arrange
            int topCount = 5;
            _authorMockService.Setup(svc => svc.GetFeaturedAuthorsAsync(topCount))
                              .ThrowsAsync(new Exception("Memory overflow"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _query.GetFeaturedAuthors(_authorMockService.Object, topCount));
            Assert.Equal("Memory overflow", exception.Message);
        }

    }
}