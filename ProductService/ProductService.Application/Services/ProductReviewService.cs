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
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product review DTOs.</returns>
    public async Task<IEnumerable<ProductReviewDto>> GetProductReviewByIdAsync(int productId)
    {
        var reviews = await _repository.GetProductReviewByIdAsync(productId);
        return reviews.Select(MapToDto).Where(x => x != null)!;
    }

    /// <summary>
    /// Retrieves rating counts for a specific product as DTOs.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product review DTOs containing rating counts.</returns>
    public async Task<IEnumerable<ProductReviewDto>> GetProductRatingCountAsync(int productId)
    {
        var reviews = await _repository.GetProductRatingCountAsync(productId);
        return reviews.Select(MapToDto).Where(x => x != null)!;
    }

    /// <summary>
    /// Retrieves detailed product reviews as DTOs with various filters and pagination.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="starOne">Filter for 1-star reviews.</param>
    /// <param name="starTwo">Filter for 2-star reviews.</param>
    /// <param name="starThree">Filter for 3-star reviews.</param>
    /// <param name="starFour">Filter for 4-star reviews.</param>
    /// <param name="starFive">Filter for 5-star reviews.</param>
    /// <param name="readerSpoiler">Filter for reader spoilers.</param>
    /// <param name="recomendThis">Filter for recommended reviews.</param>
    /// <param name="sortByFilter">The sort criteria.</param>
    /// <param name="pageNo">The page number for pagination.</param>
    /// <param name="noOfRow">The number of rows per page.</param>
    /// <param name="readerType">Filter by reader type.</param>
    /// <param name="languageType">Filter by language type.</param>
    /// <returns>A collection of detailed product review DTOs.</returns>
    public async Task<IEnumerable<ProductReviewDto>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType)
    {
        var reviews = await _repository.GetProductReviewDetailAsync(productId, starOne, starTwo, starThree, starFour, starFive, readerSpoiler, recomendThis, sortByFilter, pageNo, noOfRow, readerType, languageType);
        return reviews.Select(MapToDto).Where(x => x != null)!;
    }

    /// <summary>
    /// Retrieves reviews posted by a specific user profile as DTOs.
    /// </summary>
    /// <param name="customerProfileId">The unique identifier of the customer profile.</param>
    /// <param name="pageNo">The page number for pagination.</param>
    /// <param name="noOfRow">The number of rows per page.</param>
    /// <returns>A collection of product review DTOs by the user.</returns>
    public async Task<IEnumerable<ProductReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
    {
        var reviews = await _repository.GetUserProfileReviewsAsync(customerProfileId, pageNo, noOfRow);
        return reviews.Select(MapToDto).Where(x => x != null)!;
    }

    /// <summary>
    /// Retrieves reader types associated with a product's reviews as DTOs.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of review reader type DTOs.</returns>
    public async Task<IEnumerable<ReviewReaderTypeDto>> GetReviewReaderTypeAsync(int productId)
    {
        var types = await _repository.GetReviewReaderTypeAsync(productId);
        return types.Select(t => new ReviewReaderTypeDto { ReaderTypeId = t.ReaderTypeId, ReaderType = t.ReaderType });
    }

    /// <summary>
    /// Retrieves all available review reader types as DTOs.
    /// </summary>
    /// <returns>A collection of all review reader type DTOs.</returns>
    public async Task<IEnumerable<ReviewReaderTypeDto>> GetAllReviewReaderTypeAsync()
    {
        var types = await _repository.GetAllReviewReaderTypeAsync();
        return types.Select(t => new ReviewReaderTypeDto { ReaderTypeId = t.ReaderTypeId, ReaderType = t.ReaderType });
    }

    /// <summary>
    /// Retrieves all available review tag names as DTOs.
    /// </summary>
    /// <returns>A collection of review tag name DTOs.</returns>
    public async Task<IEnumerable<ReviewTagNameDto>> GetReviewTagsNameAsync()
    {
        var tags = await _repository.GetReviewTagsNameAsync();
        return tags.Select(t => new ReviewTagNameDto { Id = t.Id, TagName = t.TagName });
    }

    /// <summary>
    /// Records a user's vote on a product review and returns the result as a DTO.
    /// </summary>
    /// <param name="custProfileId">The unique identifier of the customer profile.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productReviewId">The unique identifier of the product review.</param>
    /// <param name="userCookiesId">The user's cookie identifier for guest voting.</param>
    /// <param name="votingType">The type of vote (e.g., helpful, report).</param>
    /// <returns>The updated helpfulness DTO for the review.</returns>
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
