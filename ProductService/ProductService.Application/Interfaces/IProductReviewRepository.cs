using ProductService.Application.DTOs;

namespace ProductService.Application.Interfaces;

/// <summary>
/// Interface for Product Review repository operations using Application DTOs.
/// </summary>
public interface IProductReviewRepository
{
    /// <summary>
    /// Retrieves product reviews as DTOs by product identifier.
    /// </summary>
    Task<IEnumerable<ProductReviewDto>> GetProductReviewByIdAsync(int productId);

    /// <summary>
    /// Retrieves rating counts for a specific product as DTOs.
    /// </summary>
    Task<List<ProductReviewDto>> GetProductRatingCountAsync(int productId);

    /// <summary>
    /// Retrieves detailed product reviews as DTOs with various filters and pagination.
    /// </summary>
    Task<List<ProductReviewDto>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType);

    /// <summary>
    /// Retrieves reviews posted by a specific user profile as DTOs.
    /// </summary>
    Task<List<ProductReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow);

    /// <summary>
    /// Retrieves reader types associated with a product's reviews as DTOs.
    /// </summary>
    Task<List<ReviewReaderTypeDto>> GetReviewReaderTypeAsync(int productId);

    /// <summary>
    /// Retrieves all available review reader types as DTOs.
    /// </summary>
    Task<List<ReviewReaderTypeDto>> GetAllReviewReaderTypeAsync();

    /// <summary>
    /// Retrieves all available review tag names as DTOs.
    /// </summary>
    Task<List<ReviewTagNameDto>> GetReviewTagsNameAsync();

    /// <summary>
    /// Records a user's vote on a product review and returns the result as a DTO.
    /// </summary>
    Task<ReviewHelpFulDto?> TakeUserVotingAsync(int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType);

    /// <summary>
    /// Adds a new product review along with tags and images.
    /// </summary>
    Task<int> AddProductReviewAsync(ProductReviewInputDto inputDto);
}
