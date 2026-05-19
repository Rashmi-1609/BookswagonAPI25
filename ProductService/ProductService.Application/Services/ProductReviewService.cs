using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Services;

/// <summary>
/// Service layer for handling Product Review business logic.
/// </summary>
public class ProductReviewService(IProductReviewRepository repository) : IProductReviewService
{
    private readonly IProductReviewRepository _repository = repository;

    /// <summary>
    /// Retrieves product reviews as DTOs by product identifier.
    /// <inheritdoc />
    public async Task<IEnumerable<ProductReviewDto>> GetProductReviewByIdAsync(int productId)
    {
        var reviews = await _repository.GetProductReviewByIdAsync(productId);
        return reviews.Select(MapToDto).Where(x => x != null)!;
    }

    /// <summary>
    /// Retrieves rating counts for a specific product as DTOs.
    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetProductRatingCountAsync(int productId)
    {
        var reviews = await _repository.GetProductRatingCountAsync(productId);
        return reviews.Select(MapToDto).Where(x => x != null).Cast<ProductReviewDto>().ToList();
    }

    /// <summary>
    /// Retrieves detailed product reviews as DTOs with various filters and pagination.
    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType)
    {
        var reviews = await _repository.GetProductReviewDetailAsync(productId, starOne, starTwo, starThree, starFour, starFive, readerSpoiler, recomendThis, sortByFilter, pageNo, noOfRow, readerType, languageType);
        return reviews.Select(MapToDto).Where(x => x != null).Cast<ProductReviewDto>().ToList();
    }

    /// <summary>
    /// Retrieves reviews posted by a specific user profile as DTOs.
    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
    {
        var reviews = await _repository.GetUserProfileReviewsAsync(customerProfileId, pageNo, noOfRow);
        return reviews.Select(MapToDto).Where(x => x != null).Cast<ProductReviewDto>().ToList();
    }

    /// <summary>
    /// Retrieves reader types associated with a product's reviews as DTOs.
    /// <inheritdoc />
    public async Task<List<ReviewReaderTypeDto>> GetReviewReaderTypeAsync(int productId)
    {
        var types = await _repository.GetReviewReaderTypeAsync(productId);
        return types.Select(t => new ReviewReaderTypeDto { ReaderTypeId = t.ReaderTypeId, ReaderType = t.ReaderType }).ToList();
    }

    /// <summary>
    /// Retrieves all available review reader types as DTOs.
    /// <inheritdoc />
    public async Task<List<ReviewReaderTypeDto>> GetAllReviewReaderTypeAsync()
    {
        var types = await _repository.GetAllReviewReaderTypeAsync();
        return types.Select(t => new ReviewReaderTypeDto { ReaderTypeId = t.ReaderTypeId, ReaderType = t.ReaderType }).ToList();
    }

    /// <summary>
    /// Retrieves all available review tag names as DTOs.
    /// <inheritdoc />
    public async Task<List<ReviewTagNameDto>> GetReviewTagsNameAsync()
    {
        var tags = await _repository.GetReviewTagsNameAsync();
        return tags.Select(t => new ReviewTagNameDto { Id = t.Id, TagName = t.TagName }).ToList();
    }

    /// <summary>
    /// Records a user's vote on a product review and returns the result as a DTO.
    /// <inheritdoc />
    public async Task<ReviewHelpFulDto?> TakeUserVotingAsync(int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType)
    {
        var result = await _repository.TakeUserVotingAsync(custProfileId, productId, productReviewId, userCookiesId, votingType);
        if (result == null) return null;
        
        return new ReviewHelpFulDto
        {
            ReviewHelpId = result.ReviewHelpId,
            Helpful = result.Helpful,
            NotHelpFul = result.NotHelpFul,
            Reported = result.Reported
        };
    }

    /// <summary>
    /// Adds a new product review along with tags and images.
    /// <inheritdoc />
    public async Task<int> AddProductReviewAsync(ProductReviewInputDto inputDto)
    {
        if (inputDto == null) return 0;

        var entity = new ProductReview
        {
            ProductId = inputDto.ProductId,
            ReviewTitle = inputDto.ReviewTitle,
            ReviewBy = inputDto.ReviewBy,
            Description = inputDto.Description,
            ReviewStatus = inputDto.ReviewStatus,
            Rating = inputDto.Rating,
            RecommendThis = inputDto.RecommendThis,
            ReaderType = inputDto.ReaderType,
            ReaderSpoiler = inputDto.ReaderSpoiler,
            UserEmail = inputDto.UserEmail,
            IsActive = true,
            IsDeleted = false,
            DateCreated = DateTime.Now,
            ReviewTagNames = inputDto.ReviewTagNames?.Select(t => new ReviewTagName
            {
                Id = t.Id,
                TagName = t.TagName
            }).ToList() ?? new List<ReviewTagName>(),
            ProductReviewImages = inputDto.ProductReviewImages?.Select(i => new ProductReviewImage
            {
                ProductReviewId = i.ProductReviewId,
                ImageLocation = i.ImageLocation,
                ImageCaption = i.ImageCaption
            }).ToList() ?? new List<ProductReviewImage>()
        };

        return await _repository.AddProductReviewAsync(entity);
    }

    /// <summary>
    /// Maps a ProductReview entity to a ProductReviewDto.
    /// </summary>
    /// <param name="entity">The product review entity.</param>
    /// <returns>A mapped product review DTO.</returns>
    private ProductReviewDto? MapToDto(ProductReview? entity)

    {
        if (entity == null) return null;

        return new ProductReviewDto
        {
            ProductReviewId = entity.ProductReviewId,
            ProductId = entity.ProductId,
            ReviewTitle = entity.ReviewTitle,
            ReviewBy = entity.ReviewBy,
            Description = entity.Description,
            ReviewStatus = entity.ReviewStatus,
            Rating = entity.Rating,
            AvgRating = entity.AvgRating,
            TotalReview = entity.TotalReview,
            PostDate = entity.PostDate,
            RatingCount = entity.RatingCount,
            ReaderType = entity.ReaderType,
            UserEmail = entity.UserEmail,
            Vote = entity.Vote,
            ReviewTags = entity.ReviewTags,
            StarOne = entity.StarOne,
            StarTwo = entity.StarTwo,
            StarThree = entity.StarThree,
            StarFour = entity.StarFour,
            StarFive = entity.StarFive,
            RecommendThis = entity.RecommendThis,
            ReaderSpoiler = entity.ReaderSpoiler,
            TotalRecord = entity.TotalRecord,
            CustomerProfileId = entity.CustomerProfileId,
            ProductReviewImages = entity.ProductReviewImages?.Select(i => new ProductReviewImageDto 
            { 
                ProductReviewId = i.ProductReviewId, 
                ImageLocation = i.ImageLocation, 
                ImageCaption = i.ImageCaption 
            }).ToList() ?? new List<ProductReviewImageDto>(),
            ReviewHelpFul = new ReviewHelpFulDto 
            { 
                ReviewHelpId = entity.ReviewHelpFul?.ReviewHelpId ?? 0, 
                Helpful = entity.ReviewHelpFul?.Helpful ?? 0, 
                NotHelpFul = entity.ReviewHelpFul?.NotHelpFul ?? 0, 
                Reported = entity.ReviewHelpFul?.Reported ?? 0 
            }
        };
    }
}
