using FluentAssertions;
using Moq;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;
using ReviewService.Api.Application.Services;
using ReviewService.Api.Domain.Entities;

namespace ReviewService.Tests.Application.Services;

public class ProductReviewServiceTests
{
    private readonly Mock<IProductReviewRepository> _mockRepository;
    private readonly ProductReviewService _service;

    public ProductReviewServiceTests()
    {
        _mockRepository = new Mock<IProductReviewRepository>();
        _service = new ProductReviewService(_mockRepository.Object);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task WhenProductIdIsInvalid_ShouldReturnFailure(int productId)
    {
        // Act
        var result = await _service.GetReviewSummaryAsync(productId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid Product ID provided.");
        result.Data.Should().BeNull();
        
        // Verify the repository was never called with invalid product ID
        _mockRepository.Verify(
            x => x.GetReviewSummaryByProductIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task WhenRepositoryReturnsNull_ShouldReturnFailure()
    {
        // Arrange
        int productId = 1;
        _mockRepository
            .Setup(x => x.GetReviewSummaryByProductIdAsync(productId))
            .ReturnsAsync((ProductReviewSummary?)null);

        // Act
        var result = await _service.GetReviewSummaryAsync(productId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Failed to retrieve product reviews.");
        result.Data.Should().BeNull();
        
        // Verify the repository was called exactly once
        _mockRepository.Verify(
            x => x.GetReviewSummaryByProductIdAsync(productId),
            Times.Once);
    }

    [Fact]
    public async Task WhenRepositoryReturnsData_ShouldReturnSuccess()
    {
        // Arrange
        int productId = 42;
        var mockReviews = new List<ProductReview>
        {
            new() { ProductId = productId, Rating = 5, ReviewStatus = 1, IsActive = true, IsDeleted = false },
            new() { ProductId = productId, Rating = 4, ReviewStatus = 1, IsActive = true, IsDeleted = false }
        };
        
        var expectedSummary = new ProductReviewSummary
        {
            TotalRating = 5,
            AvgRating = 4.5,
            TotalCount = 2,
            Reviews = mockReviews
        };

        _mockRepository
            .Setup(x => x.GetReviewSummaryByProductIdAsync(productId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _service.GetReviewSummaryAsync(productId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
        result.Data.Should().NotBeNull();
        result.Data!.TotalRating.Should().Be(5);
        result.Data.AvgRating.Should().Be(4.5);
        result.Data.TotalCount.Should().Be(2);
        result.Data.Reviews.Should().HaveCount(2);
        result.Data.Reviews.Should().Equal(mockReviews);

        // Verify the repository was called exactly once
        _mockRepository.Verify(
            x => x.GetReviewSummaryByProductIdAsync(productId),
            Times.Once);
    }
}
