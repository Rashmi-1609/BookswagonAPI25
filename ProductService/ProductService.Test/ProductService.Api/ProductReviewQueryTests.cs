using FluentAssertions;
using HotChocolate;
using Moq;
using ProductService.Api.GraphQL.Mutations;
using ProductService.Api.GraphQL.Queries;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using Xunit;

namespace ProductService.Test.ProductService.Api;

public class ProductReviewQueryTests
{
    private readonly Mock<IProductReviewService> _serviceMock;
    private readonly ProductReviewQuery _query;
    private readonly ProductReviewMutation _mutation;

    public ProductReviewQueryTests()
    {
        _serviceMock = new Mock<IProductReviewService>();
        _query = new ProductReviewQuery();
        _mutation = new ProductReviewMutation();
    }

    [Fact]
    public async Task GetProductReviewById_CallsServiceAndReturnsList()
    {
        // Arrange
        int productId = 101;
        var mockDtos = new List<ProductReviewDto>
        {
            new() { ProductReviewId = 1, ProductId = productId, ReviewTitle = "Superb" }
        };

        _serviceMock.Setup(svc => svc.GetProductReviewByIdAsync(productId))
            .ReturnsAsync(mockDtos);

        // Act
        var result = await _query.GetProductReviewById(_serviceMock.Object, productId);

        // Assert
        result.Should().BeSameAs(mockDtos);
        _serviceMock.Verify(svc => svc.GetProductReviewByIdAsync(productId), Times.Once);
    }

    [Fact]
    public async Task GetProductRatingCount_CallsServiceAndReturnsList()
    {
        // Arrange
        int productId = 101;
        var mockDtos = new List<ProductReviewDto>
        {
            new() { Rating = 5, RatingCount = 42 }
        };

        _serviceMock.Setup(svc => svc.GetProductRatingCountAsync(productId))
            .ReturnsAsync(mockDtos);

        // Act
        var result = await _query.GetProductRatingCount(_serviceMock.Object, productId);

        // Assert
        result.Should().BeSameAs(mockDtos);
        _serviceMock.Verify(svc => svc.GetProductRatingCountAsync(productId), Times.Once);
    }

    [Fact]
    public async Task PostReview_ThrowsGraphQLException_WhenTitleIsEmpty()
    {
        // Arrange
        var input = new ProductReviewInputDto
        {
            ReviewTitle = string.Empty, // Empty title
            ReviewBy = "John",
            ProductId = 101
        };

        // Act
        Func<Task> act = async () => await _mutation.PostReview(_serviceMock.Object, input);

        // Assert
        await act.Should().ThrowAsync<GraphQLException>()
            .WithMessage("ReviewTitle is required.");

        _serviceMock.Verify(svc => svc.AddProductReviewAsync(It.IsAny<ProductReviewInputDto>()), Times.Never);
    }

    [Fact]
    public async Task PostReview_CallsServiceAndReturnsId_WhenInputIsValid()
    {
        // Arrange
        var input = new ProductReviewInputDto
        {
            ReviewTitle = "Super book!",
            ReviewBy = "John",
            ProductId = 101
        };

        _serviceMock.Setup(svc => svc.AddProductReviewAsync(input))
            .ReturnsAsync(123);

        // Act
        var result = await _mutation.PostReview(_serviceMock.Object, input);

        // Assert
        result.Should().Be(123);
        _serviceMock.Verify(svc => svc.AddProductReviewAsync(input), Times.Once);
    }

    [Fact]
    public async Task TakeUserVoting_CallsServiceAndReturnsResult()
    {
        // Arrange
        var mockVoteResult = new ReviewHelpFulDto { ReviewHelpId = 1, Helpful = 5 };
        _serviceMock.Setup(svc => svc.TakeUserVotingAsync(1, 101, 2, "cookie", 1))
            .ReturnsAsync(mockVoteResult);

        // Act
        var result = await _query.TakeUserVoting(_serviceMock.Object, 1, 101, 2, "cookie", 1);

        // Assert
        result.Should().BeSameAs(mockVoteResult);
        _serviceMock.Verify(svc => svc.TakeUserVotingAsync(1, 101, 2, "cookie", 1), Times.Once);
    }

    [Fact]
    public async Task GetProductReviewDetail_DelegatesToService()
    {
        // Arrange
        var mockDetails = new List<ProductReviewDto> { new() { ProductReviewId = 1 } };
        _serviceMock.Setup(svc => svc.GetProductReviewDetailAsync(101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English"))
            .ReturnsAsync(mockDetails);

        // Act
        var result = await _query.GetProductReviewDetail(_serviceMock.Object, 101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English");

        // Assert
        result.Should().BeSameAs(mockDetails);
    }

    [Fact]
    public async Task GetUserProfileReviews_DelegatesToService()
    {
        // Arrange
        var mockReviews = new List<ProductReviewDto> { new() { ProductReviewId = 1 } };
        _serviceMock.Setup(svc => svc.GetUserProfileReviewsAsync(5, 1, 10))
            .ReturnsAsync(mockReviews);

        // Act
        var result = await _query.GetUserProfileReviews(_serviceMock.Object, 5, 1, 10);

        // Assert
        result.Should().BeSameAs(mockReviews);
    }

    [Fact]
    public async Task GetReviewReaderType_DelegatesToService()
    {
        // Arrange
        var mockTypes = new List<ReviewReaderTypeDto> { new() { ReaderTypeId = 1 } };
        _serviceMock.Setup(svc => svc.GetReviewReaderTypeAsync(101))
            .ReturnsAsync(mockTypes);

        // Act
        var result = await _query.GetReviewReaderType(_serviceMock.Object, 101);

        // Assert
        result.Should().BeSameAs(mockTypes);
    }

    [Fact]
    public async Task GetAllReviewReaderType_DelegatesToService()
    {
        // Arrange
        var mockTypes = new List<ReviewReaderTypeDto> { new() { ReaderTypeId = 1 } };
        _serviceMock.Setup(svc => svc.GetAllReviewReaderTypeAsync())
            .ReturnsAsync(mockTypes);

        // Act
        var result = await _query.GetAllReviewReaderType(_serviceMock.Object);

        // Assert
        result.Should().BeSameAs(mockTypes);
    }

    [Fact]
    public async Task GetReviewTagsName_DelegatesToService()
    {
        // Arrange
        var mockTags = new List<ReviewTagNameDto> { new() { Id = 1 } };
        _serviceMock.Setup(svc => svc.GetReviewTagsNameAsync())
            .ReturnsAsync(mockTags);

        // Act
        var result = await _query.GetReviewTagsName(_serviceMock.Object);

        // Assert
        result.Should().BeSameAs(mockTags);
    }

    [Fact]
    public async Task GetProductReviewById_ReturnsEmptyList_WhenServiceReturnsEmpty()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.GetProductReviewByIdAsync(101))
            .ReturnsAsync(Enumerable.Empty<ProductReviewDto>());

        // Act
        var result = await _query.GetProductReviewById(_serviceMock.Object, 101);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProductRatingCount_ReturnsEmptyList_WhenServiceReturnsEmpty()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.GetProductRatingCountAsync(101))
            .ReturnsAsync(new List<ProductReviewDto>());

        // Act
        var result = await _query.GetProductRatingCount(_serviceMock.Object, 101);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProductReviewDetail_ReturnsEmptyList_WhenServiceReturnsEmpty()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.GetProductReviewDetailAsync(101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English"))
            .ReturnsAsync(new List<ProductReviewDto>());

        // Act
        var result = await _query.GetProductReviewDetail(_serviceMock.Object, 101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserProfileReviews_ReturnsEmptyList_WhenServiceReturnsEmpty()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.GetUserProfileReviewsAsync(5, 1, 10))
            .ReturnsAsync(new List<ProductReviewDto>());

        // Act
        var result = await _query.GetUserProfileReviews(_serviceMock.Object, 5, 1, 10);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetReviewReaderType_ReturnsEmptyList_WhenServiceReturnsEmpty()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.GetReviewReaderTypeAsync(101))
            .ReturnsAsync(new List<ReviewReaderTypeDto>());

        // Act
        var result = await _query.GetReviewReaderType(_serviceMock.Object, 101);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllReviewReaderType_ReturnsEmptyList_WhenServiceReturnsEmpty()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.GetAllReviewReaderTypeAsync())
            .ReturnsAsync(new List<ReviewReaderTypeDto>());

        // Act
        var result = await _query.GetAllReviewReaderType(_serviceMock.Object);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetReviewTagsName_ReturnsEmptyList_WhenServiceReturnsEmpty()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.GetReviewTagsNameAsync())
            .ReturnsAsync(new List<ReviewTagNameDto>());

        // Act
        var result = await _query.GetReviewTagsName(_serviceMock.Object);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task TakeUserVoting_ReturnsNull_WhenServiceReturnsNull()
    {
        // Arrange
        _serviceMock.Setup(svc => svc.TakeUserVotingAsync(1, 101, 2, "cookie", 1))
            .ReturnsAsync((ReviewHelpFulDto?)null);

        // Act
        var result = await _query.TakeUserVoting(_serviceMock.Object, 1, 101, 2, "cookie", 1);

        // Assert
        result.Should().BeNull();
    }
}
