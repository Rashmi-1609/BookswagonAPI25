using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ReviewService.Api.Application.Common;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;
using ReviewService.Api.Domain.Entities;
using ReviewService.Api.GraphQL.Queries;
using HotChocolate;
using Xunit;

namespace ReviewService.Tests.GraphQL.Queries;

public class ProductReviewQueryTests
{
    private readonly Mock<IProductReviewService> _mockService;
    private readonly ProductReviewQuery _query;

    public ProductReviewQueryTests()
    {
        _mockService = new Mock<IProductReviewService>();
        _query = new ProductReviewQuery();
    }

    [Fact]
    public async Task WhenServiceReturnsSuccess_ShouldReturnData()
    {
        // Arrange
        int productId = 123;
        var summary = new ProductReviewSummary
        {
            TotalRating = 5,
            AvgRating = 4.2,
            TotalCount = 2,
            Reviews = new()
            {
                new ProductReview { Id = 1, ProductId = productId, Rating = 5, ReviewStatus = 1, IsActive = true, IsDeleted = false },
                new ProductReview { Id = 2, ProductId = productId, Rating = 3, ReviewStatus = 1, IsActive = true, IsDeleted = false }
            }
        };
        _mockService.Setup(s => s.GetReviewSummaryAsync(productId))
            .ReturnsAsync(ServiceResult<ProductReviewSummary>.Success(summary));

        // Act
        var result = await _query.GetProductReviewDetailAsync(productId, _mockService.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(summary);
    }

    [Fact]
    public async Task WhenServiceReturnsFailure_ShouldThrowGraphQLException()
    {
        // Arrange
        int productId = 456;
        string errorMessage = "Some error occurred.";
        _mockService.Setup(s => s.GetReviewSummaryAsync(productId))
            .ReturnsAsync(ServiceResult<ProductReviewSummary>.Failure(errorMessage));

        // Act
        var act = async () => await _query.GetProductReviewDetailAsync(productId, _mockService.Object);

        // Assert
        var ex = await Assert.ThrowsAsync<GraphQLException>(act);
        ex.Message.Should().Be(errorMessage);
    }
}
