using HotChocolate;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;

namespace ProductService.Api.GraphQL.Queries;

/// <summary>
/// GraphQL queries for Product Reviews.
/// </summary>
public class ProductReviewQuery
{
    /// <summary>
    /// Retrieves product reviews by product identifier.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product review DTOs.</returns>
    public Task<IEnumerable<ProductReviewDto>> GetProductReviewById([Service] IProductReviewService svc, int productId)
        => svc.GetProductReviewByIdAsync(productId);

    /// <summary>
    /// Retrieves rating counts for a specific product.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A list of product review DTOs containing rating counts.</returns>
    public Task<List<ProductReviewDto>> GetProductRatingCount([Service] IProductReviewService svc, int productId)
        => svc.GetProductRatingCountAsync(productId);

    /// <summary>
    /// Retrieves detailed product reviews with various filters and pagination.
    /// </summary>
    /// <param name="svc">The product review service.</param>
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
    /// <returns>A list of detailed product review DTOs.</returns>
    public Task<List<ProductReviewDto>> GetProductReviewDetail([Service] IProductReviewService svc, 
        int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, 
        int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, 
        string readerType, string languageType)
        => svc.GetProductReviewDetailAsync(productId, starOne, starTwo, starThree, starFour, starFive, readerSpoiler, recomendThis, sortByFilter, pageNo, noOfRow, readerType, languageType);

    /// <summary>
    /// Retrieves reviews posted by a specific user profile.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <param name="customerProfileId">The unique identifier of the customer profile.</param>
    /// <param name="pageNo">The page number for pagination.</param>
    /// <param name="noOfRow">The number of rows per page.</param>
    /// <returns>A list of product review DTOs by the user.</returns>
    public Task<List<ProductReviewDto>> GetUserProfileReviews([Service] IProductReviewService svc, int customerProfileId, int pageNo, int noOfRow)
        => svc.GetUserProfileReviewsAsync(customerProfileId, pageNo, noOfRow);

    /// <summary>
    /// Retrieves reader types associated with a product's reviews.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A list of review reader type DTOs.</returns>
    public Task<List<ReviewReaderTypeDto>> GetReviewReaderType([Service] IProductReviewService svc, int productId)
        => svc.GetReviewReaderTypeAsync(productId);

    /// <summary>
    /// Retrieves all available review reader types.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <returns>A list of all review reader type DTOs.</returns>
    public Task<List<ReviewReaderTypeDto>> GetAllReviewReaderType([Service] IProductReviewService svc)
        => svc.GetAllReviewReaderTypeAsync();

    /// <summary>
    /// Retrieves all available review tag names.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <returns>A list of review tag name DTOs.</returns>
    public Task<List<ReviewTagNameDto>> GetReviewTagsName([Service] IProductReviewService svc)
        => svc.GetReviewTagsNameAsync();

    /// <summary>
    /// Records a user's vote on a product review.
    /// </summary>
    /// <param name="svc">The product review service.</param>
    /// <param name="custProfileId">The unique identifier of the customer profile.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productReviewId">The unique identifier of the product review.</param>
    /// <param name="userCookiesId">The user's cookie identifier for guest voting.</param>
    /// <param name="votingType">The type of vote (e.g., helpful, report).</param>
    /// <returns>The updated helpfulness data for the review.</returns>
    public Task<ReviewHelpFulDto?> TakeUserVoting([Service] IProductReviewService svc, int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType)
        => svc.TakeUserVotingAsync(custProfileId, productId, productReviewId, userCookiesId, votingType);
}
