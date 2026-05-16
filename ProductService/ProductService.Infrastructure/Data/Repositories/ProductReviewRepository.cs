using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for accessing Product Review data via stored procedures.
/// </summary>
public class ProductReviewRepository(ProductDbContext dbContext) : IProductReviewRepository
{
    private readonly ProductDbContext _dbContext = dbContext;

    /// <summary>
    /// Retrieves product reviews by product identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product reviews.</returns>
    public async Task<IEnumerable<ProductReview>> GetProductReviewByIdAsync(int productId)
    {
        var lstProductReview = await _dbContext.ProductReviews
            .Where(p => p.ProductId == productId && p.ReviewStatus == 1 && p.IsActive == true && p.IsDeleted == false)
            .ToListAsync();

        int avgRating = 0;
        int totalReview = 0;
        if (lstProductReview.Count > 0)
        {
            avgRating = (int)lstProductReview.Average(p => p.Rating);
            totalReview = lstProductReview.Count;
        }

        foreach (var pr in lstProductReview)
        {
            pr.AvgRating = avgRating;
            pr.TotalReview = totalReview;
            pr.PostDate = (DateTime.Now - pr.DateCreated).Days.ToString() + " Days Ago";
        }

        return lstProductReview;
    }

    /// <summary>
    /// Retrieves rating counts for a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of product reviews containing rating counts.</returns>
    public async Task<IEnumerable<ProductReview>> GetProductRatingCountAsync(int productId)
    {
        var productReviews = new List<ProductReview>();
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_GetProductRateingCount";
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            productReviews.Add(new ProductReview
            {
                Rating = Convert.ToInt32(reader["Rating"]),
                RatingCount = Convert.ToInt32(reader["RatingCount"])
            });
        }

        return productReviews;
    }

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
    public async Task<IEnumerable<ProductReview>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType)
    {
        var productReviews = new List<ProductReview>();
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_GetReview";
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });
        command.Parameters.Add(new SqlParameter("@StarOne", SqlDbType.Int) { Value = starOne });
        command.Parameters.Add(new SqlParameter("@StarTwo", SqlDbType.Int) { Value = starTwo });
        command.Parameters.Add(new SqlParameter("@StarThree", SqlDbType.Int) { Value = starThree });
        command.Parameters.Add(new SqlParameter("@StarFour", SqlDbType.Int) { Value = starFour });
        command.Parameters.Add(new SqlParameter("@StarFive", SqlDbType.Int) { Value = starFive });
        command.Parameters.Add(new SqlParameter("@ReaderSpoilers", SqlDbType.Int) { Value = readerSpoiler });
        command.Parameters.Add(new SqlParameter("@RecomendThis", SqlDbType.Int) { Value = recomendThis });
        command.Parameters.Add(new SqlParameter("@SortByFilter", SqlDbType.Int) { Value = sortByFilter });
        command.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int) { Value = pageNo });
        command.Parameters.Add(new SqlParameter("@NoOfRow", SqlDbType.Int) { Value = noOfRow });
        command.Parameters.Add(new SqlParameter("@Readertype", SqlDbType.VarChar) { Value = (object)readerType ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@LanguageType", SqlDbType.VarChar) { Value = (object)languageType ?? DBNull.Value });

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var review = new ProductReview
            {
                ProductReviewId = Convert.ToInt32(reader["ID_ProductReview"]),
                TotalRecord = Convert.ToInt32(reader["TotalRecord"]),
                ReviewBy = reader["User_Name"]?.ToString() ?? string.Empty,
                UserEmail = reader["Email_Id"]?.ToString() ?? string.Empty,
                ReaderType = reader["ReaderType"]?.ToString() ?? string.Empty,
                ReaderSpoiler = reader["ReaderSpoilers"]?.ToString() ?? string.Empty,
                TotalReview = Convert.ToInt32(reader["TotalReview"]),
                Vote = Convert.ToInt32(reader["Vote"]),
                ReviewTags = reader["ReviewTag_Name"]?.ToString() ?? string.Empty,
                ReviewTitle = reader["Review_Title"]?.ToString() ?? string.Empty,
                Description = reader["Description"]?.ToString() ?? string.Empty,
                Rating = Convert.ToInt32(reader["Rating"]),
                StarFive = Convert.ToInt32(reader["StarFive"]),
                StarFour = Convert.ToInt32(reader["StarFour"]),
                CustomerProfileId = Convert.ToInt32(reader["Id_CustProfile"]),
                RecommendThis = reader["RecomendThis"]?.ToString() ?? string.Empty,
                PostDate = reader["Date_Created"]?.ToString() ?? string.Empty
            };

            review.ProductReviewImages = await GetProductReviewImagesAsync(review.ProductReviewId, review.CustomerProfileId);
            review.ReviewHelpFul = await GetUserVotingAsync(review.CustomerProfileId, productId, review.ProductReviewId) ?? new ReviewHelpFul();
            productReviews.Add(review);
        }

        return productReviews;
    }

    /// <summary>
    /// Retrieves reviews posted by a specific user profile.
    /// </summary>
    /// <param name="customerProfileId">The unique identifier of the customer profile.</param>
    /// <param name="pageNo">The page number for pagination.</param>
    /// <param name="noOfRow">The number of rows per page.</param>
    /// <returns>A collection of product reviews by the user.</returns>
    public async Task<IEnumerable<ProductReview>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
    {
        var productReviews = new List<ProductReview>();
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_GetUserReview";
        command.Parameters.Add(new SqlParameter("@Id_CustProfile", SqlDbType.Int) { Value = customerProfileId });
        command.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int) { Value = pageNo });
        command.Parameters.Add(new SqlParameter("@NoOfRow", SqlDbType.Int) { Value = noOfRow });

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var review = new ProductReview
            {
                ReviewBy = reader["User_Name"]?.ToString() ?? string.Empty,
                UserEmail = reader["Email_Id"]?.ToString() ?? string.Empty,
                ReaderType = reader["ReaderType"]?.ToString() ?? string.Empty,
                ReaderSpoiler = reader["ReaderSpoilers"]?.ToString() ?? string.Empty,
                TotalReview = Convert.ToInt32(reader["TotalReview"]),
                ReviewTitle = reader["Review_Title"]?.ToString() ?? string.Empty,
                Description = reader["Description"]?.ToString() ?? string.Empty,
                Rating = Convert.ToInt32(reader["Rating"]),
                PostDate = reader["Date_Created"]?.ToString() ?? string.Empty
            };

            review.ProductReviewImages = await GetProductReviewImagesAsync(Convert.ToInt32(reader["ID_ProductReview"]), customerProfileId);
            productReviews.Add(review);
        }

        return productReviews;
    }

    /// <summary>
    /// Retrieves reader types associated with a product's reviews.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of review reader types.</returns>
    public async Task<IEnumerable<ReviewReaderType>> GetReviewReaderTypeAsync(int productId)
    {
        var lst = new List<ReviewReaderType>();
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_GetReviewReaderType";
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lst.Add(new ReviewReaderType
            {
                ReaderTypeId = Convert.ToInt32(reader["ReviewID"]),
                ReaderType = reader["ReaderType"]?.ToString() ?? string.Empty
            });
        }
        return lst;
    }

    /// <summary>
    /// Retrieves all available review reader types.
    /// </summary>
    /// <returns>A collection of all review reader types.</returns>
    public async Task<IEnumerable<ReviewReaderType>> GetAllReviewReaderTypeAsync()
    {
        var lst = new List<ReviewReaderType>();
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SP_GetReviewReaderType";

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lst.Add(new ReviewReaderType
            {
                ReaderTypeId = Convert.ToInt32(reader["Id"]),
                ReaderType = reader["ReaderType"]?.ToString() ?? string.Empty
            });
        }
        return lst;
    }

    /// <summary>
    /// Retrieves all available review tag names.
    /// </summary>
    /// <returns>A collection of review tag names.</returns>
    public async Task<IEnumerable<ReviewTagName>> GetReviewTagsNameAsync()
    {
        var lst = new List<ReviewTagName>();
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SP_GetReviewTagName";

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lst.Add(new ReviewTagName
            {
                Id = Convert.ToInt32(reader["Id_Tag"]),
                TagName = reader["ReviewTag_Name"]?.ToString() ?? string.Empty
            });
        }
        return lst;
    }

    /// <summary>
    /// Records a user's vote on a product review.
    /// </summary>
    /// <param name="custProfileId">The unique identifier of the customer profile.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productReviewId">The unique identifier of the product review.</param>
    /// <param name="userCookiesId">The user's cookie identifier for guest voting.</param>
    /// <param name="votingType">The type of vote (e.g., helpful, report).</param>
    /// <returns>The updated helpfulness data for the review.</returns>
    public async Task<ReviewHelpFul?> TakeUserVotingAsync(int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType)
    {
        ReviewHelpFul? reviewHelpFul = null;
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_InsertReviewVoting";
        command.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int) { Value = custProfileId });
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });
        command.Parameters.Add(new SqlParameter("@ProductReviewID", SqlDbType.Int) { Value = productReviewId });
        command.Parameters.Add(new SqlParameter("@UserIDinCookies", SqlDbType.VarChar) { Value = (object)userCookiesId ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@Votingtype", SqlDbType.Int) { Value = votingType });

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            reviewHelpFul = new ReviewHelpFul
            {
                Helpful = Convert.ToInt32(reader["Helpful"]),
                NotHelpFul = Convert.ToInt32(reader["NotHelpFul"]),
                Reported = Convert.ToInt32(reader["Reported"])
            };
        }
        return reviewHelpFul;
    }

    // Helper methods internally
    /// <summary>
    /// Retrieves images for a specific product review.
    /// </summary>
    /// <param name="productReviewId">The unique identifier of the product review.</param>
    /// <param name="customerProfileId">The unique identifier of the customer profile.</param>
    /// <returns>A list of product review images.</returns>
    private async Task<List<ProductReviewImage>> GetProductReviewImagesAsync(long productReviewId, int customerProfileId)
    {
        var images = new List<ProductReviewImage>();
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_GetProdReviewImage";
        command.Parameters.Add(new SqlParameter("@ID_ProductReview", SqlDbType.Int) { Value = productReviewId });
        command.Parameters.Add(new SqlParameter("@Id_CustProfile", SqlDbType.Int) { Value = customerProfileId });

        await _dbContext.Database.OpenConnectionAsync(); // Should be fine as long as MARS is enabled, or we await it correctly. 
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            images.Add(new ProductReviewImage
            {
                ProductReviewId = Convert.ToInt32(reader["ID_ProductReview"]),
                ImageCaption = reader["ImgCaption"]?.ToString() ?? string.Empty,
                ImageLocation = reader["Image_Location"]?.ToString() ?? string.Empty
            });
        }
        return images;
    }

    /// <summary>
    /// Retrieves a user's voting status for a specific review.
    /// </summary>
    /// <param name="custProfileId">The unique identifier of the customer profile.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productReviewId">The unique identifier of the product review.</param>
    /// <returns>The helpfulness status for the review.</returns>
    private async Task<ReviewHelpFul?> GetUserVotingAsync(int custProfileId, int productId, int productReviewId)
    {
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_GetReviewVoting";
        command.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int) { Value = custProfileId });
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });
        command.Parameters.Add(new SqlParameter("@ProductReviewID", SqlDbType.Int) { Value = productReviewId });

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new ReviewHelpFul
            {
                Helpful = Convert.ToInt32(reader["Helpful"]),
                NotHelpFul = Convert.ToInt32(reader["NotHelpFul"]),
                Reported = Convert.ToInt32(reader["Reported"])
            };
        }
        return null;
    }

}
