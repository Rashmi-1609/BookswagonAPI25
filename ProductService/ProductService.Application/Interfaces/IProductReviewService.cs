using ProductService.Application.DTOs;

namespace ProductService.Application.Interfaces;

/// <summary>
/// Interface for Product Review service operations.
/// </summary>
public interface IProductReviewService
{
    /// <summary>
    /// Retrieves product reviews as DTOs by product identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product review DTOs.</returns>
    Task<IEnumerable<ProductReviewDto>> GetProductReviewByIdAsync(int productId);

    /// <summary>
    /// Retrieves rating counts for a specific product as DTOs.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product review DTOs containing rating counts.</returns>
    Task<IEnumerable<ProductReviewDto>> GetProductRatingCountAsync(int productId);

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
    Task<IEnumerable<ProductReviewDto>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType);

    /// <summary>
    /// Retrieves reviews posted by a specific user profile as DTOs.
    /// </summary>
    /// <param name="customerProfileId">The unique identifier of the customer profile.</param>
    /// <param name="pageNo">The page number for pagination.</param>
    /// <param name="noOfRow">The number of rows per page.</param>
    /// <returns>A collection of product review DTOs by the user.</returns>
    Task<IEnumerable<ProductReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow);

    /// <summary>
    /// Retrieves reader types associated with a product's reviews as DTOs.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of review reader type DTOs.</returns>
    Task<IEnumerable<ReviewReaderTypeDto>> GetReviewReaderTypeAsync(int productId);

    /// <summary>
    /// Retrieves all available review reader types as DTOs.
    /// </summary>
    /// <returns>A collection of all review reader type DTOs.</returns>
    Task<IEnumerable<ReviewReaderTypeDto>> GetAllReviewReaderTypeAsync();

    /// <summary>
    /// Retrieves all available review tag names as DTOs.
    /// </summary>
    /// <returns>A collection of review tag name DTOs.</returns>
    Task<IEnumerable<ReviewTagNameDto>> GetReviewTagsNameAsync();

    /// <summary>
    /// Records a user's vote on a product review and returns the result as a DTO.
    /// </summary>
    /// <param name="custProfileId">The unique identifier of the customer profile.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productReviewId">The unique identifier of the product review.</param>
    /// <param name="userCookiesId">The user's cookie identifier for guest voting.</param>
    /// <param name="votingType">The type of vote (e.g., helpful, report).</param>
    /// <returns>The updated helpfulness DTO for the review.</returns>
    Task<ReviewHelpFulDto?> TakeUserVotingAsync(int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType);

    /// <summary>
    /// Adds a new product review along with tags and images.
    /// </summary>
    /// <param name="inputDto">The review input details.</param>
    /// <returns>The generated review ID, or 0 if failed.</returns>
    Task<int> AddProductReviewAsync(ProductReviewInputDto inputDto);
}

