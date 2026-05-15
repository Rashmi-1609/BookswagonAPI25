using FluentAssertions;
using Moq;
using PublisherService.Application.DTO;
using PublisherService.Application.Services;
using PublisherService.Domain.Entities;
using PublisherService.Domain.Interfaces;
using AppPublisherService = global::PublisherService.Application.Services.PublisherService;
using Xunit;

namespace PublisherService.Test.PublisherService.Application;

public class PublisherServiceTests
{
    private readonly Mock<IPublisherRepository> _repositoryMock;
    private readonly AppPublisherService _sut; // System Under Test

    public PublisherServiceTests()
    {
        _repositoryMock = new Mock<IPublisherRepository>();
        _sut = new AppPublisherService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetPublisherByIdAsync_WithValidId_ReturnsPublisherDto()
    {
        // Arrange
        int id = 1;
        var publisherEntity = new Publisher { PublisherId = id, CompanyName = "Test Publisher" };
        _repositoryMock.Setup(repo => repo.GetPublisherByIdAsync(id)).ReturnsAsync(publisherEntity);

        // Act
        var result = await _sut.GetPublisherByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.PublisherId.Should().Be(id);
        result.CompanyName.Should().Be("Test Publisher");
    }

    [Fact]
    public async Task GetPublisherByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        int id = -1;

        // Act
        var result = await _sut.GetPublisherByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(repo => repo.GetPublisherByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetPublisherByIdAsync_WhenPublisherNotFound_ReturnsNull()
    {
        // Arrange
        int id = 999;
        _repositoryMock.Setup(repo => repo.GetPublisherByIdAsync(id)).ReturnsAsync((Publisher?)null);

        // Act
        var result = await _sut.GetPublisherByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPublishersByNameAsync_WithEmptyName_ReturnsEmptyList()
    {
        // Arrange
        string name = "";

        // Act
        var result = await _sut.GetPublishersByNameAsync(name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _repositoryMock.Verify(repo => repo.GetPublishersByNameAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetPublishersByNameAsync_WithValidName_ReturnsMappedDtoList()
    {
        // Arrange
        string name = "Tech";
        var publishers = new List<Publisher>
        {
            new Publisher { PublisherId = 1, CompanyName = "Tech Books" },
            new Publisher { PublisherId = 2, CompanyName = "Tech Press" }
        };
        _repositoryMock.Setup(repo => repo.GetPublishersByNameAsync(name)).ReturnsAsync(publishers);

        // Act
        var result = await _sut.GetPublishersByNameAsync(name);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].CompanyName.Should().Be("Tech Books");
        result[1].CompanyName.Should().Be("Tech Press");
    }
}
