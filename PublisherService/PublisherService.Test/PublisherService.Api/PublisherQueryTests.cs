using FluentAssertions;
using HotChocolate;
using Moq;
using PublisherService.Api.GraphQL.Queries;
using PublisherService.Application.DTO;
using PublisherService.Application.Interfaces;
using Xunit;

namespace PublisherService.Test.PublisherService.Api;

public class PublisherQueryTests
{
    private readonly Mock<IPublisherService> _publisherServiceMock;
    private readonly PublisherQuery _sut;

    public PublisherQueryTests()
    {
        _publisherServiceMock = new Mock<IPublisherService>();
        _sut = new PublisherQuery();
    }

    [Fact]
    public async Task GetPublisherById_WhenPublisherExists_ReturnsPublisher()
    {
        // Arrange
        int id = 1;
        var publisherDto = new PublisherDto { PublisherId = id, CompanyName = "Test Publisher" };
        _publisherServiceMock.Setup(service => service.GetPublisherByIdAsync(id)).ReturnsAsync(publisherDto);

        // Act
        var result = await _sut.GetPublisherById(_publisherServiceMock.Object, id);

        // Assert
        result.Should().NotBeNull();
        result!.PublisherId.Should().Be(id);
        result.CompanyName.Should().Be("Test Publisher");
    }

    [Fact]
    public async Task GetPublisherById_WhenPublisherDoesNotExist_ReturnsNull()
    {
        // Arrange
        int id = 999;
        _publisherServiceMock.Setup(service => service.GetPublisherByIdAsync(id)).ReturnsAsync((PublisherDto?)null);

        // Act
        var result = await _sut.GetPublisherById(_publisherServiceMock.Object, id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPublishersByName_CallsServiceAndReturnsList()
    {
        // Arrange
        string name = "Tech";
        var expectedPublishers = new List<PublisherDto>
        {
            new PublisherDto { PublisherId = 1, CompanyName = "Tech Books" },
            new PublisherDto { PublisherId = 2, CompanyName = "Tech Press" }
        };
        _publisherServiceMock.Setup(service => service.GetPublishersByNameAsync(name, 1, 10)).ReturnsAsync(expectedPublishers);

        // Act
        var result = await _sut.GetPublishersByName(_publisherServiceMock.Object, name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPublishers);
        _publisherServiceMock.Verify(service => service.GetPublishersByNameAsync(name, 1, 10), Times.Once);
    }
}
