using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AuthorService.Application.DTO;
using AuthorService.Application.Services;
using AuthorService.Domain.Entities;
using AuthorService.Domain.Interfaces;
using AppAuthorService = AuthorService.Application.Services.AuthorService;

namespace AuthorService.Tests.AuthorService.Application
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _authorMockRepo;
        private readonly AppAuthorService _service; // System Under Test (SUT)

        public AuthorServiceTests()
        {
            // Create the Mock (fake)
            _authorMockRepo = new Mock<IAuthorRepository>();

            // Pass this Mock into the Service (service is under test here)
            _service = new AppAuthorService(_authorMockRepo.Object);
        }

        // ----- Tests for GetAuthorByIdAsync -----

        [Fact]
        public async Task GetAuthorByIdAsync_Success_ReturnsMappedAuthorDto()
        {
            // Arrange -> set fake data and mock behavior
            int testId = 1;

            var testAuthor = new Author
            {
                AuthorId = testId,
                Name = "Chetan Bhagat",
                FirstName = "Chetan",
                LastName = "Bhagat",
                AuthorDetail = "Indian Author and Columnist"
            };

            _authorMockRepo.Setup(repo => repo.GetAuthorByIdAsync(testId))
                                 .ReturnsAsync(testAuthor);

            // Act -> call the method under test
            var result = await _service.GetAuthorByIdAsync(testId);

            // Assert -> verify the results
            Assert.NotNull(result);
            Assert.IsType<AuthorDto>(result);
            Assert.Equal(testAuthor.AuthorId, result.AuthorId);
            Assert.Equal(testAuthor.Name, result.AuthorName); // Testing the mapping logic
            Assert.Equal(testAuthor.FirstName, result.FirstName);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_Failure_ReturnsNullWhenNotFound()
        {
            // Arrange
            int testId = 99;
            _authorMockRepo.Setup(repo => repo.GetAuthorByIdAsync(testId))
                                 .ReturnsAsync((Author?)null);

            // Act
            var result = await _service.GetAuthorByIdAsync(testId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_Error_ThrowsExceptionOnDatabaseFailure()
        {
            // Arrange
            int testId = 1;
            _authorMockRepo.Setup(repo => repo.GetAuthorByIdAsync(testId))
                                 .ThrowsAsync(new Exception("Database connection lost"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAuthorByIdAsync(testId));
            Assert.Equal("Database connection lost", exception.Message);
        }



        //  ----- Tests for GetFeaturedAuthorsAsync -----

        [Fact]
        public async Task GetFeaturedAuthorsAsync_Success_ReturnsListOfAuthorDtos()
        {
            // Arrange
            int topCount = 2;
            var domainAuthors = new List<Author>
            {
                new Author { AuthorId = 1, Name = "Author One" },
                new Author { AuthorId = 2, Name = "Author Two" }
            };

            _authorMockRepo.Setup(repo => repo.GetFeaturedAuthorsAsync(topCount))
                                 .ReturnsAsync(domainAuthors);

            // Act
            var result = await _service.GetFeaturedAuthorsAsync(topCount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<AuthorDto>>(result);
            Assert.Equal("Author One", result[0].AuthorName);
        }

        [Fact]
        public async Task GetFeaturedAuthorsAsync_Failure_ReturnsEmptyListWhenNoAuthorsFound()
        {
            // Arrange
            int topCount = 5;
            _authorMockRepo.Setup(repo => repo.GetFeaturedAuthorsAsync(topCount))
                                 .ReturnsAsync(new List<Author>());

            // Act
            var result = await _service.GetFeaturedAuthorsAsync(topCount);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFeaturedAuthorsAsync_Error_ThrowsExceptionOnDatabaseFailure()
        {
            // Arrange
            int topCount = 5;
            _authorMockRepo.Setup(repo => repo.GetFeaturedAuthorsAsync(topCount))
                                 .ThrowsAsync(new Exception("Timeout expired"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetFeaturedAuthorsAsync(topCount));
            Assert.Equal("Timeout expired", exception.Message);
        }


        // ----- Tests for GetAuthorByNameAsync -----

        [Fact]
        public async Task GetAuthorByNameAsync_Success_ReturnsMappedAuthorDto()
        {
            // Arrange
            string testName = "Jane Austen";
            var domainAuthor = new Author { AuthorId = 5, Name = testName };

            _authorMockRepo.Setup(repo => repo.GetAuthorByNameAsync(testName))
                                 .ReturnsAsync(domainAuthor);

            // Act
            var result = await _service.GetAuthorByNameAsync(testName);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testName, result.AuthorName);
        }

        [Fact]
        public async Task GetAuthorByNameAsync_Failure_ReturnsNullWhenNotFound()
        {
            // Arrange
            string testName = "Unknown Author";
            _authorMockRepo.Setup(repo => repo.GetAuthorByNameAsync(testName))
                                 .ReturnsAsync((Author?)null);

            // Act
            var result = await _service.GetAuthorByNameAsync(testName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAuthorByNameAsync_Error_ThrowsExceptionOnDatabaseFailure()
        {
            // Arrange
            string testName = "Jane Austen";
            _authorMockRepo.Setup(repo => repo.GetAuthorByNameAsync(testName))
                                 .ThrowsAsync(new Exception("Data reader error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAuthorByNameAsync(testName));
            Assert.Equal("Data reader error", exception.Message);
        }

    }
}