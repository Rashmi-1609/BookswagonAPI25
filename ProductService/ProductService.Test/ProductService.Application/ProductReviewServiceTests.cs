using FluentAssertions;
using Moq;
using ProductService.Application.DTOs;
using ProductService.Application.Services;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using Xunit;

namespace ProductService.Test.ProductService.Application;

public class ProductReviewServiceTests
{
    private readonly Mock<IProductReviewRepository> _repositoryMock;
    private readonly ProductReviewService _service;

    public ProductReviewServiceTests()
    {
        _repositoryMock = new Mock<IProductReviewRepository>();
        _service = new ProductReviewService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetProductReviewByIdAsync_ReturnsMappedDtos_WhenReviewsExist()
    {
        // Arrange
        int productId = 101;
        var mockReviews = new List<ProductReview>
        {
            new()
            {
                ProductReviewId = 1,
                ProductId = productId,
                ReviewTitle = "Great book!",
                ReviewBy = "John Doe",
                Description = "Loved it",
                Rating = 5,
                DateCreated = DateTime.Now.AddDays(-5),
                PostDate = "5 Days Ago",
                ProductReviewImages = new List<ProductReviewImage>
                {
                    new() { ProductReviewId = 1, ImageLocation = "img.jpg", ImageCaption = "Caption" }
                },
                ReviewHelpFul = new ReviewHelpFul { ReviewHelpId = 10, Helpful = 3, NotHelpFul = 0 }
            }
        };

        _repositoryMock.Setup(repo => repo.GetProductReviewByIdAsync(productId))
            .ReturnsAsync(mockReviews);

        // Act
        var result = await _service.GetProductReviewByIdAsync(productId);

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        var dto = result.First();
        dto.ProductReviewId.Should().Be(1);
        dto.ProductId.Should().Be(productId);
        dto.ReviewTitle.Should().Be("Great book!");
        dto.ReviewBy.Should().Be("John Doe");
        dto.Description.Should().Be("Loved it");
        dto.Rating.Should().Be(5);
        dto.PostDate.Should().Be("5 Days Ago");
        dto.ProductReviewImages.Should().HaveCount(1);
        dto.ProductReviewImages.First().ImageLocation.Should().Be("img.jpg");
        dto.ReviewHelpFul.Helpful.Should().Be(3);
    }

    [Fact]
    public async Task GetProductReviewByIdAsync_ReturnsEmpty_WhenNoReviewsFound()
    {
        // Arrange
        int productId = 101;
        _repositoryMock.Setup(repo => repo.GetProductReviewByIdAsync(productId))
            .ReturnsAsync(Enumerable.Empty<ProductReview>());

        // Act
        var result = await _service.GetProductReviewByIdAsync(productId);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task AddProductReviewAsync_ReturnsZero_WhenInputIsNull()
    {
        // Act
        var result = await _service.AddProductReviewAsync(null!);

        // Assert
        result.Should().Be(0);
        _repositoryMock.Verify(repo => repo.AddProductReviewAsync(It.IsAny<ProductReview>()), Times.Never);
    }

    [Fact]
    public async Task AddProductReviewAsync_MapsInputAndReturnsReviewId_WhenInputIsValid()
    {
        // Arrange
        var inputDto = new ProductReviewInputDto
        {
            ProductId = 202,
            ReviewTitle = "Interesting Read",
            ReviewBy = "Alice Smith",
            Description = "Good content.",
            Rating = 4,
            RecommendThis = "Yes",
            ReaderType = "General",
            ReaderSpoiler = "No",
            UserEmail = "alice@example.com",
            ReviewTagNames = new List<ReviewTagNameDto> { new() { Id = 1, TagName = "Tag1" } },
            ProductReviewImages = new List<ProductReviewImageDto> { new() { ImageLocation = "loc.png", ImageCaption = "Cap" } }
        };

        _repositoryMock.Setup(repo => repo.AddProductReviewAsync(It.IsAny<ProductReview>()))
            .Callback<ProductReview>(r => r.ProductReviewId = 99)
            .ReturnsAsync(99);

        // Act
        var result = await _service.AddProductReviewAsync(inputDto);

        // Assert
        result.Should().Be(99);
        _repositoryMock.Verify(repo => repo.AddProductReviewAsync(It.Is<ProductReview>(r =>
            r.ProductId == 202 &&
            r.ReviewTitle == "Interesting Read" &&
            r.ReviewBy == "Alice Smith" &&
            r.Description == "Good content." &&
            r.Rating == 4 &&
            r.RecommendThis == "Yes" &&
            r.UserEmail == "alice@example.com" &&
            r.ReviewTagNames.Count == 1 &&
            r.ProductReviewImages.Count == 1
        )), Times.Once);
    }

    [Fact]
    public async Task TakeUserVotingAsync_ReturnsMappedDto_WhenVotingSucceeds()
    {
        // Arrange
        int custProfileId = 1;
        int productId = 101;
        int reviewId = 5;
        string cookiesId = "cookies-abc";
        int votingType = 1;

        var mockHelpful = new ReviewHelpFul
        {
            ReviewHelpId = 55,
            Helpful = 10,
            NotHelpFul = 2,
            Reported = 0
        };

        _repositoryMock.Setup(repo => repo.TakeUserVotingAsync(custProfileId, productId, reviewId, cookiesId, votingType))
            .ReturnsAsync(mockHelpful);

        // Act
        var result = await _service.TakeUserVotingAsync(custProfileId, productId, reviewId, cookiesId, votingType);

        // Assert
        result.Should().NotBeNull();
        result!.ReviewHelpId.Should().Be(55);
        result.Helpful.Should().Be(10);
        result.NotHelpFul.Should().Be(2);
        result.Reported.Should().Be(0);
    }

    [Fact]
    public async Task TakeUserVotingAsync_ReturnsNull_WhenVotingReturnsNull()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.TakeUserVotingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((ReviewHelpFul?)null);

        // Act
        var result = await _service.TakeUserVotingAsync(1, 2, 3, "cookies", 1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProductReviewDetailAsync_ReturnsMappedDtos_WhenReviewsExist()
    {
        // Arrange
        var mockReviews = new List<ProductReview>
        {
            new() { ProductReviewId = 1, ProductId = 101, ReviewTitle = "Detail", Rating = 4 }
        };

        _repositoryMock.Setup(repo => repo.GetProductReviewDetailAsync(101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English"))
            .ReturnsAsync(mockReviews);

        // Act
        var result = await _service.GetProductReviewDetailAsync(101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English");

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        result.First().ReviewTitle.Should().Be("Detail");
    }

    [Fact]
    public async Task GetUserProfileReviewsAsync_ReturnsMappedDtos_WhenReviewsExist()
    {
        // Arrange
        var mockReviews = new List<ProductReview>
        {
            new() { ProductReviewId = 1, ProductId = 101, ReviewTitle = "User review", Rating = 3 }
        };

        _repositoryMock.Setup(repo => repo.GetUserProfileReviewsAsync(5, 1, 10))
            .ReturnsAsync(mockReviews);

        // Act
        var result = await _service.GetUserProfileReviewsAsync(5, 1, 10);

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        result.First().ReviewTitle.Should().Be("User review");
    }

    [Fact]
    public async Task GetReviewReaderTypeAsync_ReturnsMappedDtos()
    {
        // Arrange
        var mockReaderTypes = new List<ReviewReaderType>
        {
            new() { ReaderTypeId = 2, ReaderType = "Bibliophile" }
        };

        _repositoryMock.Setup(repo => repo.GetReviewReaderTypeAsync(101))
            .ReturnsAsync(mockReaderTypes);

        // Act
        var result = await _service.GetReviewReaderTypeAsync(101);

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        result.First().ReaderTypeId.Should().Be(2);
        result.First().ReaderType.Should().Be("Bibliophile");
    }

    [Fact]
    public async Task GetAllReviewReaderTypeAsync_ReturnsMappedDtos()
    {
        // Arrange
        var mockReaderTypes = new List<ReviewReaderType>
        {
            new() { ReaderTypeId = 1, ReaderType = "General" }
        };

        _repositoryMock.Setup(repo => repo.GetAllReviewReaderTypeAsync())
            .ReturnsAsync(mockReaderTypes);

        // Act
        var result = await _service.GetAllReviewReaderTypeAsync();

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        result.First().ReaderTypeId.Should().Be(1);
        result.First().ReaderType.Should().Be("General");
    }

    [Fact]
    public async Task GetReviewTagsNameAsync_ReturnsMappedDtos()
    {
        // Arrange
        var mockTags = new List<ReviewTagName>
        {
            new() { Id = 3, TagName = "Fascinating" }
        };

        _repositoryMock.Setup(repo => repo.GetReviewTagsNameAsync())
            .ReturnsAsync(mockTags);

        // Act
        var result = await _service.GetReviewTagsNameAsync();

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Id.Should().Be(3);
        result.First().TagName.Should().Be("Fascinating");
    }

    [Fact]
    public async Task GetProductRatingCountAsync_ReturnsMappedDtos()
    {
        // Arrange
        var mockRatingCounts = new List<ProductReview>
        {
            new() { Rating = 5, RatingCount = 10 }
        };

        _repositoryMock.Setup(repo => repo.GetProductRatingCountAsync(101))
            .ReturnsAsync(mockRatingCounts);

        // Act
        var result = await _service.GetProductRatingCountAsync(101);

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Rating.Should().Be(5);
        result.First().RatingCount.Should().Be(10);
    }

    [Fact]
    public async Task GetProductReviewByIdAsync_FiltersOutNullEntities_WhenNullReturnedFromRepository()
    {
        // Arrange
        var mockReviews = new List<ProductReview>
        {
            null!, // Null element
            new() { ProductReviewId = 2, ProductId = 101, ReviewTitle = "Valid" }
        };

        _repositoryMock.Setup(repo => repo.GetProductReviewByIdAsync(101))
            .ReturnsAsync(mockReviews);

        // Act
        var result = await _service.GetProductReviewByIdAsync(101);

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        result.First().ProductReviewId.Should().Be(2);
    }

    [Fact]
    public async Task GetProductReviewByIdAsync_HandlesNullNavigationProperties_Correctly()
    {
        // Arrange
        var mockReviews = new List<ProductReview>
        {
            new()
            {
                ProductReviewId = 3,
                ProductId = 101,
                ProductReviewImages = null!, // Null images list
                ReviewHelpFul = null!        // Null helpful votes
            }
        };

        _repositoryMock.Setup(repo => repo.GetProductReviewByIdAsync(101))
            .ReturnsAsync(mockReviews);

        // Act
        var result = await _service.GetProductReviewByIdAsync(101);

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);
        var dto = result.First();
        dto.ProductReviewImages.Should().NotBeNull().And.BeEmpty();
        dto.ReviewHelpFul.Should().NotBeNull();
        dto.ReviewHelpFul.ReviewHelpId.Should().Be(0);
        dto.ReviewHelpFul.Helpful.Should().Be(0);
        dto.ReviewHelpFul.NotHelpFul.Should().Be(0);
        dto.ReviewHelpFul.Reported.Should().Be(0);
    }

    [Fact]
    public async Task GetProductReviewDetailAsync_ReturnsEmpty_WhenNoReviewsFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetProductReviewDetailAsync(101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English"))
            .ReturnsAsync(Enumerable.Empty<ProductReview>());

        // Act
        var result = await _service.GetProductReviewDetailAsync(101, 1, 0, 0, 0, 0, 0, 1, 1, 1, 10, "General", "English");

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetUserProfileReviewsAsync_ReturnsEmpty_WhenNoReviewsFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetUserProfileReviewsAsync(5, 1, 10))
            .ReturnsAsync(Enumerable.Empty<ProductReview>());

        // Act
        var result = await _service.GetUserProfileReviewsAsync(5, 1, 10);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetReviewReaderTypeAsync_ReturnsEmpty_WhenNoReaderTypesFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetReviewReaderTypeAsync(101))
            .ReturnsAsync(Enumerable.Empty<ReviewReaderType>());

        // Act
        var result = await _service.GetReviewReaderTypeAsync(101);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetAllReviewReaderTypeAsync_ReturnsEmpty_WhenNoReaderTypesFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetAllReviewReaderTypeAsync())
            .ReturnsAsync(Enumerable.Empty<ReviewReaderType>());

        // Act
        var result = await _service.GetAllReviewReaderTypeAsync();

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetReviewTagsNameAsync_ReturnsEmpty_WhenNoTagsFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetReviewTagsNameAsync())
            .ReturnsAsync(Enumerable.Empty<ReviewTagName>());

        // Act
        var result = await _service.GetReviewTagsNameAsync();

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetProductRatingCountAsync_ReturnsEmpty_WhenNoRatingCountsFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetProductRatingCountAsync(101))
            .ReturnsAsync(Enumerable.Empty<ProductReview>());

        // Act
        var result = await _service.GetProductRatingCountAsync(101);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }
}
