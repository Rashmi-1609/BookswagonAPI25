using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces;

/// <summary>
/// Interface for Product Review repository operations.
/// </summary>
public interface IProductReviewRepository
{
    /// <summary>
    /// Retrieves product reviews by product identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product reviews.</returns>
    Task<IEnumerable<ProductReview>> GetProductReviewByIdAsync(int productId);

    /// <summary>
    /// Retrieves rating counts for a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product reviews containing rating counts.</returns>
    Task<IEnumerable<ProductReview>> GetProductRatingCountAsync(int productId);

    /// <summary>
    /// Retrieves detailed product reviews with various filters and pagination.
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
    /// <returns>A collection of detailed product reviews.</returns>
    Task<IEnumerable<ProductReview>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType);

    /// <summary>
    /// Retrieves reviews posted by a specific user profile.
    /// </summary>
    /// <param name="customerProfileId">The unique identifier of the customer profile.</param>
    /// <param name="pageNo">The page number for pagination.</param>
    /// <param name="noOfRow">The number of rows per page.</param>
    /// <returns>A collection of product reviews by the user.</returns>
    Task<IEnumerable<ProductReview>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow);

    /// <summary>
    /// Retrieves reader types associated with a product's reviews.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of review reader types.</returns>
    Task<IEnumerable<ReviewReaderType>> GetReviewReaderTypeAsync(int productId);


    /// <summary>
    /// Retrieves all available review reader types.
    /// </summary>
    /// <returns>A collection of all review reader types.</returns>
    Task<IEnumerable<ReviewReaderType>> GetAllReviewReaderTypeAsync();

    /// <summary>
    /// Retrieves all available review tag names.
    /// </summary>
    /// <returns>A collection of review tag names.</returns>
    Task<IEnumerable<ReviewTagName>> GetReviewTagsNameAsync();

    /// <summary>
    /// Records a user's vote on a product review.
    /// </summary>
    /// <param name="custProfileId">The unique identifier of the customer profile.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productReviewId">The unique identifier of the product review.</param>
    /// <param name="userCookiesId">The user's cookie identifier for guest voting.</param>
    /// <param name="votingType">The type of vote (e.g., helpful, report).</param>
    /// <returns>The updated helpfulness data for the review.</returns>
    Task<ReviewHelpFul?> TakeUserVotingAsync(int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType);

    /// <summary>
    /// Adds a new product review along with tags and images.
    /// </summary>
    /// <param name="productReview">The review details to add.</param>
    /// <returns>The generated Product Review ID, or 0 if failed.</returns>
    Task<int> AddProductReviewAsync(ProductReview productReview);
}

