using FluentAssertions;
using Moq;
using PublisherService.Api.Application.Interfaces;
using PublisherSvc = PublisherService.Api.Application.Services.PublisherService;
using PublisherService.Api.Domain.Entities;

namespace PublisherService.Tests.Application.Services;

public class PublisherServiceTests
{
    private readonly Mock<IPublisherRepository> _mockRepo;
    private readonly PublisherSvc _service;

    public PublisherServiceTests()
    {
        _mockRepo = new Mock<IPublisherRepository>();
        _service = new PublisherSvc(_mockRepo.Object);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task WhenIdIsInvalid_ShouldReturnFailure(int id)
    {
        // Act
        var result = await _service.GetPublisherByIdAsync(id);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid Publisher ID. ID must be greater than 0.");
        result.Data.Should().BeNull();
        _mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task WhenPublisherNotFound_ShouldReturnFailure()
    {
        // Arrange
        int id = 99;
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Publisher?)null);

        // Act
        var result = await _service.GetPublisherByIdAsync(id);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Publisher with ID {id} not found.");
        result.Data.Should().BeNull();
        _mockRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task WhenPublisherExists_ShouldReturnSuccess()
    {
        // Arrange
        int id = 5;
        var publisher = new Publisher { PublisherId = id, CompanyName = "Test Publisher" };
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(publisher);

        // Act
        var result = await _service.GetPublisherByIdAsync(id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEquivalentTo(publisher);
        _mockRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public void WhenCalled_ShouldReturnSuccessWithQueryable()
    {
        // Arrange
        var publishers = new List<Publisher>
        {
            new Publisher { PublisherId = 1, CompanyName = "A" },
            new Publisher { PublisherId = 2, CompanyName = "B" }
        }.AsQueryable();
        string name = "Test";
        _mockRepo.Setup(r => r.GetByName(name)).Returns(publishers);

        // Act
        var result = _service.GetPublishersByName(name);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEquivalentTo(publishers);
        _mockRepo.Verify(r => r.GetByName(name), Times.Once);
    }
}
