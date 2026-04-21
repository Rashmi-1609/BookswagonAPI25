using FluentAssertions;
using Moq;
using PublisherService.Api.Application.Common;
using PublisherService.Api.Application.Interfaces;
using PublisherService.Api.Domain.Entities;
using PublisherService.Api.GraphQL.Queries;
using HotChocolate;

namespace PublisherService.Tests.GraphQL.Queries;

public class PublisherQueryTests
{
    private readonly Mock<IPublisherService> _mockService;
    private readonly PublisherQuery _query;

    public PublisherQueryTests()
    {
        _mockService = new Mock<IPublisherService>();
        _query = new PublisherQuery();
    }

    [Fact]
    public async Task PublisherById_WhenServiceReturnsSuccess_ShouldReturnPublisher()
    {
        // Arrange
        int id = 10;
        var publisher = new Publisher { PublisherId = id, CompanyName = "Test Publisher" };
        _mockService.Setup(s => s.GetPublisherByIdAsync(id))
            .ReturnsAsync(ServiceResult<Publisher>.Success(publisher));

        // Act
        var result = await _query.GetPublisherByIdAsync(id, _mockService.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(publisher);
    }

    [Fact]
    public async Task PublisherById_WhenServiceReturnsFailure_ShouldThrowGraphQLException()
    {
        // Arrange
        int id = 20;
        string errorMessage = "Not found!";
        _mockService.Setup(s => s.GetPublisherByIdAsync(id))
            .ReturnsAsync(ServiceResult<Publisher>.Failure(errorMessage));

        // Act
        var act = async () => await _query.GetPublisherByIdAsync(id, _mockService.Object);

        // Assert
        var ex = await Assert.ThrowsAsync<GraphQLException>(act);
        ex.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void SearchPublishers_ShouldReturnQueryable()
    {
        // Arrange
        string name = "Test";
        var publishers = new[]
        {
            new Publisher { PublisherId = 1, CompanyName = "A" },
            new Publisher { PublisherId = 2, CompanyName = "B" }
        }.AsQueryable();
        _mockService.Setup(s => s.GetPublishersByName(name))
            .Returns(ServiceResult<IQueryable<Publisher>>.Success(publishers));

        // Act
        var result = _query.GetPublishersByName(name, _mockService.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(publishers);
    }
}
