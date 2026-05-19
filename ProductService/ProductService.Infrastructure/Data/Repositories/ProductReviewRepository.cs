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
    /// <inheritdoc />
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
    /// <inheritdoc />
    public async Task<List<ProductReview>> GetProductRatingCountAsync(int productId)
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
    /// <inheritdoc />
    public async Task<List<ProductReview>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType)
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
    /// <inheritdoc />
    public async Task<List<ProductReview>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
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
    /// <inheritdoc />
    public async Task<List<ReviewReaderType>> GetReviewReaderTypeAsync(int productId)
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
    /// <inheritdoc />
    public async Task<List<ReviewReaderType>> GetAllReviewReaderTypeAsync()
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
    /// <inheritdoc />
    public async Task<List<ReviewTagName>> GetReviewTagsNameAsync()
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
    /// <inheritdoc />
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

    /// <summary>
    /// Adds a new product review along with tags and images using legacy stored procedures.
    /// <inheritdoc />
    public async Task<int> AddProductReviewAsync(ProductReview productReview)
    {
        try
        {
            int custProfileId = await GetReviewUserProfileAsync(productReview.UserEmail, 0);
            if (custProfileId == 0)
            {
                custProfileId = await AddReviewUserProfileAsync(
                    productReview.ProductId, 
                    productReview.ReviewBy, 
                    string.Empty, 
                    productReview.UserEmail, 
                    string.Empty, 
                    productReview.RecommendThis == "Yes", 
                    int.TryParse(productReview.ReaderType, out int rt) ? rt : 0
                );
            }

            // Check if customer already has a review for this product
            if (await CheckForUserProductReviewAsync(custProfileId, productReview.ProductId))
            {
                return 0;
            }

            // Insert product review
            int productReviewId = 0;
            var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SProc_AddProductReviewNew";
            command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productReview.ProductId });
            command.Parameters.Add(new SqlParameter("@Review_Title", SqlDbType.VarChar) { Value = productReview.ReviewTitle });
            command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = productReview.Description });
            command.Parameters.Add(new SqlParameter("@ReviewBy", SqlDbType.VarChar) { Value = productReview.ReviewBy });
            command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = productReview.ReviewStatus });
            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Int) { Value = productReview.Rating });
            command.Parameters.Add(new SqlParameter("@Id_CustProfile", SqlDbType.Int) { Value = custProfileId });
            command.Parameters.Add(new SqlParameter("@RecomendThis", SqlDbType.Bit) { Value = productReview.RecommendThis == "Yes" });
            command.Parameters.Add(new SqlParameter("@ReaderType", SqlDbType.Int) { Value = int.TryParse(productReview.ReaderType, out int rType) ? rType : 0 });
            command.Parameters.Add(new SqlParameter("@ReaderSpoilers", SqlDbType.Int) { Value = productReview.ReaderSpoiler == "Yes" ? 1 : 0 });

            await _dbContext.Database.OpenConnectionAsync();
            var scalarResult = await command.ExecuteScalarAsync();
            if (scalarResult != null)
            {
                productReviewId = Convert.ToInt32(scalarResult);
            }
            productReview.ProductReviewId = productReviewId;

            // Add review tags
            if (productReview.ReviewTagNames != null)
            {
                foreach (var reviewTag in productReview.ReviewTagNames)
                {
                    await AddReviewTagUserAsync(productReview.ProductId, productReview.ProductReviewId, custProfileId, reviewTag.Id);
                }
            }

            // Add review images
            if (productReview.ProductReviewImages != null)
            {
                foreach (var reviewImage in productReview.ProductReviewImages)
                {
                    await AddReviewImageAsync(productReview.ProductId, productReview.ProductReviewId, custProfileId, reviewImage.ImageLocation, reviewImage.ImageCaption);
                }
            }

            return productReview.ProductReviewId;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    private async Task<int> GetReviewUserProfileAsync(string custProfileEmail, int custProfileId)
    {
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Sproc_GetReviewUserProfile";
        command.Parameters.Add(new SqlParameter("@Email_ID", SqlDbType.VarChar) { Value = custProfileEmail });
        command.Parameters.Add(new SqlParameter("@Id_CustProfile", SqlDbType.Int) { Value = custProfileId });

        await _dbContext.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return Convert.ToInt32(reader["Id_CustProfile"]);
        }
        return custProfileId;
    }

    private async Task<int> AddReviewUserProfileAsync(long productId, string userName, string userImageLoc, string userEmail, string location, bool recommendThis, int readerTypeId)
    {
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Sproc_InsertReviewUserProfile";
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });
        command.Parameters.Add(new SqlParameter("@User_Name", SqlDbType.VarChar) { Value = userName });
        command.Parameters.Add(new SqlParameter("@UserImgLoc", SqlDbType.VarChar) { Value = userImageLoc });
        command.Parameters.Add(new SqlParameter("@Email_ID", SqlDbType.VarChar) { Value = userEmail });
        command.Parameters.Add(new SqlParameter("@location", SqlDbType.VarChar) { Value = location });
        command.Parameters.Add(new SqlParameter("@RecomendThis", SqlDbType.Bit) { Value = recommendThis });
        command.Parameters.Add(new SqlParameter("@Id_ReaderType", SqlDbType.Int) { Value = readerTypeId });

        await _dbContext.Database.OpenConnectionAsync();
        var scalarResult = await command.ExecuteScalarAsync();
        return scalarResult != null ? Convert.ToInt32(scalarResult) : 0;
    }

    private async Task<bool> CheckForUserProductReviewAsync(int custProfileId, long productId)
    {
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_AllReadySubmitReview";
        command.Parameters.Add(new SqlParameter("@Id_CustProfile", SqlDbType.Int) { Value = custProfileId });
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.BigInt) { Value = productId });

        await _dbContext.Database.OpenConnectionAsync();
        var scalarResult = await command.ExecuteScalarAsync();
        int reviewCount = scalarResult != null ? Convert.ToInt32(scalarResult) : 0;
        return reviewCount > 0;
    }

    private async Task AddReviewTagUserAsync(long productId, int productReviewId, int customerId, int reviewTagId)
    {
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_AddReviewTagUser";
        command.Parameters.Add(new SqlParameter("@Id_CustProfile", SqlDbType.Int) { Value = customerId });
        command.Parameters.Add(new SqlParameter("@Id_Tag", SqlDbType.Int) { Value = reviewTagId });
        command.Parameters.Add(new SqlParameter("@ID_ProductReview", SqlDbType.Int) { Value = productReviewId });
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });

        await _dbContext.Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
    }

    private async Task AddReviewImageAsync(long productId, int productReviewId, int customerId, string imageLocation, string imgCaption)
    {
        var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SProc_AddProductReviewImage";
        command.Parameters.Add(new SqlParameter("@ID_Product", SqlDbType.Int) { Value = productId });
        command.Parameters.Add(new SqlParameter("@ID_ProductReview", SqlDbType.Int) { Value = productReviewId });
        command.Parameters.Add(new SqlParameter("@ID_Customer", SqlDbType.Int) { Value = customerId });
        command.Parameters.Add(new SqlParameter("@Image_Location", SqlDbType.VarChar) { Value = imageLocation });
        command.Parameters.Add(new SqlParameter("@ImgCaption", SqlDbType.VarChar) { Value = imgCaption });

        await _dbContext.Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
    }
}
