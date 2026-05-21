using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Data.Repositories.Models;

namespace ProductService.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for accessing Product Review data using Modern EF Core capabilities.
/// </summary>
public class ProductReviewRepository(ProductDbContext dbContext) : IProductReviewRepository
{
    private readonly ProductDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task<IEnumerable<ProductReviewDto>> GetProductReviewByIdAsync(int productId)
    {
        var query = _dbContext.ProductReviews
            .Where(p => p.ProductId == productId && p.ReviewStatus == 1 && p.IsActive == true && p.IsDeleted == false);

        var totalCount = await query.CountAsync();

        if (totalCount == 0)
        {
            return Enumerable.Empty<ProductReviewDto>();
        }

        var avgRating = (int)await query.AverageAsync(p => p.Rating);

        return await query
            .Select(p => new ProductReviewDto
            {
                ProductReviewId = p.ProductReviewId,
                ProductId = p.ProductId,
                ReviewTitle = p.ReviewTitle,
                ReviewBy = p.ReviewBy,
                Description = p.Description,
                ReviewStatus = p.ReviewStatus,
                Rating = p.Rating,
                DateCreated = p.DateCreated,
                TotalReview = totalCount,
                AvgRating = avgRating
            })
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetProductRatingCountAsync(int productId)
    {
        var results = await _dbContext.Database.SqlQueryRaw<ProductRatingCountSpResult>(
            "EXEC SProc_GetProductRateingCount @ID_Product",
            new SqlParameter("@ID_Product", productId)
        ).ToListAsync();

        return results.Select(r => new ProductReviewDto
        {
            RatingCount = r.RatingCount ?? 0,
            Rating = r.Rating ?? 0
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType)
    {
        var spResults = await _dbContext.Database.SqlQueryRaw<ProductReviewDetailSpResult>(
            "EXEC SProc_GetReview @ID_Product, @StarOne, @StarTwo, @StarThree, @StarFour, @StarFive, @ReaderSpoilers, @RecomendThis, @SortByFilter, @PageNo, @NoOfRow, @Readertype, @LanguageType",
            new SqlParameter("@ID_Product", productId),
            new SqlParameter("@StarOne", starOne),
            new SqlParameter("@StarTwo", starTwo),
            new SqlParameter("@StarThree", starThree),
            new SqlParameter("@StarFour", starFour),
            new SqlParameter("@StarFive", starFive),
            new SqlParameter("@ReaderSpoilers", readerSpoiler),
            new SqlParameter("@RecomendThis", recomendThis),
            new SqlParameter("@SortByFilter", sortByFilter),
            new SqlParameter("@PageNo", pageNo),
            new SqlParameter("@NoOfRow", noOfRow),
            new SqlParameter("@Readertype", (object)readerType ?? DBNull.Value),
            new SqlParameter("@LanguageType", (object)languageType ?? DBNull.Value)
        ).ToListAsync();

        var reviews = spResults.Select(r => new ProductReviewDto
        {
            ProductReviewId = r.ProductReviewId,
            TotalRecord = r.TotalRecord ?? 0,
            ReviewBy = r.ReviewBy ?? string.Empty,
            UserEmail = r.UserEmail ?? string.Empty,
            ReaderType = r.ReaderType ?? string.Empty,
            ReaderSpoiler = r.ReaderSpoiler ?? string.Empty,
            TotalReview = r.TotalReview ?? 0,
            Vote = r.Vote ?? 0,
            ReviewTags = r.ReviewTags ?? string.Empty,
            ReviewTitle = r.ReviewTitle ?? string.Empty,
            Description = r.Description ?? string.Empty,
            Rating = r.Rating ?? 0,
            StarFive = r.StarFive ?? 0,
            StarFour = r.StarFour ?? 0,
            CustomerProfileId = r.CustomerProfileId,
            RecommendThis = r.RecommendThis ?? string.Empty,
            DateCreated = r.DateCreated ?? DateTime.MinValue
        }).ToList();

        foreach (var review in reviews)
        {
            review.ProductReviewImages = await GetProductReviewImagesAsync(review.ProductReviewId, review.CustomerProfileId);
            review.ReviewHelpFul = await GetUserVotingAsync(review.CustomerProfileId, productId, review.ProductReviewId) ?? new ReviewHelpFulDto();
        }

        return reviews;
    }

    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
    {
        var spResults = await _dbContext.Database.SqlQueryRaw<UserProfileReviewSpResult>(
            "EXEC SProc_GetUserReview @Id_CustProfile, @PageNo, @NoOfRow",
            new SqlParameter("@Id_CustProfile", customerProfileId),
            new SqlParameter("@PageNo", pageNo),
            new SqlParameter("@NoOfRow", noOfRow)
        ).ToListAsync();

        var reviews = spResults.Select(r => new ProductReviewDto
        {
            ProductReviewId = r.ProductReviewId,
            ProductId = 0,
            TotalRecord = r.TotalRecord ?? 0,
            ReviewBy = r.ReviewBy ?? string.Empty,
            UserEmail = r.UserEmail ?? string.Empty,
            ReviewTitle = r.ReviewTitle ?? string.Empty,
            Description = r.Description ?? string.Empty,
            Rating = r.Rating ?? 0,
            DateCreated = r.DateCreated ?? DateTime.MinValue
        }).ToList();

        foreach (var review in reviews)
        {
            review.ProductReviewImages = await GetProductReviewImagesAsync(review.ProductReviewId, customerProfileId);
        }

        return reviews;
    }

    /// <inheritdoc />
    public async Task<List<ReviewReaderTypeDto>> GetReviewReaderTypeAsync(int productId)
    {
        var results = await _dbContext.Database.SqlQueryRaw<ProductReviewReaderTypeSpResult>(
            "EXEC SProc_GetReviewReaderType @ID_Product",
            new SqlParameter("@ID_Product", productId)
        ).ToListAsync();

        return results.Select(r => new ReviewReaderTypeDto
        {
            ReaderTypeId = r.ReaderTypeId ?? 0,
            ReaderType = r.ReaderType ?? string.Empty
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewReaderTypeDto>> GetAllReviewReaderTypeAsync()
    {
        var results = await _dbContext.Database.SqlQueryRaw<ReviewReaderTypeSpResult>(
            "EXEC SP_GetReviewReaderType"
        ).ToListAsync();

        return results.Select(r => new ReviewReaderTypeDto
        {
            ReaderTypeId = r.ReaderTypeId ?? 0,
            ReaderType = r.ReaderType ?? string.Empty
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewTagNameDto>> GetReviewTagsNameAsync()
    {
        var results = await _dbContext.Database.SqlQueryRaw<ReviewTagNameSpResult>(
            "EXEC SP_GetReviewTagName"
        ).ToListAsync();

        return results.Select(r => new ReviewTagNameDto
        {
            Id = r.Id ?? 0,
            TagName = r.TagName ?? string.Empty
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<ReviewHelpFulDto?> TakeUserVotingAsync(int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType)
    {
        var results = await _dbContext.Database.SqlQueryRaw<ReviewHelpFulSpResult>(
            "EXEC SProc_InsertReviewVoting @CustomerID, @ID_Product, @ProductReviewID, @UserIDinCookies, @Votingtype",
            new SqlParameter("@CustomerID", custProfileId),
            new SqlParameter("@ID_Product", productId),
            new SqlParameter("@ProductReviewID", productReviewId),
            new SqlParameter("@UserIDinCookies", (object)userCookiesId ?? DBNull.Value),
            new SqlParameter("@Votingtype", votingType)
        ).ToListAsync();
        
        var r = results.FirstOrDefault();
        if (r == null) return null;
        
        return new ReviewHelpFulDto
        {
            ReviewHelpId = 0,
            Helpful = r.Helpful ?? 0,
            NotHelpFul = r.NotHelpFul ?? 0,
            Reported = r.Reported ?? 0
        };
    }

    private async Task<List<ProductReviewImageDto>> GetProductReviewImagesAsync(long productReviewId, int customerProfileId)
    {
        var results = await _dbContext.Database.SqlQueryRaw<ProductReviewImageSpResult>(
            "EXEC SProc_GetProdReviewImage @ID_ProductReview, @Id_CustProfile",
            new SqlParameter("@ID_ProductReview", productReviewId),
            new SqlParameter("@Id_CustProfile", customerProfileId)
        ).ToListAsync();

        return results.Select(r => new ProductReviewImageDto
        {
            ProductReviewId = r.ProductReviewId ?? 0,
            ImageLocation = r.ImageLocation ?? string.Empty,
            ImageCaption = r.ImageCaption ?? string.Empty
        }).ToList();
    }

    private async Task<ReviewHelpFulDto?> GetUserVotingAsync(int custProfileId, int productId, int productReviewId)
    {
        var results = await _dbContext.Database.SqlQueryRaw<ReviewHelpFulSpResult>(
            "EXEC SProc_GetReviewVoting @CustomerID, @ID_Product, @ProductReviewID",
            new SqlParameter("@CustomerID", custProfileId),
            new SqlParameter("@ID_Product", productId),
            new SqlParameter("@ProductReviewID", productReviewId)
        ).ToListAsync();
        
        var r = results.FirstOrDefault();
        if (r == null) return null;
        
        return new ReviewHelpFulDto
        {
            ReviewHelpId = 0,
            Helpful = r.Helpful ?? 0,
            NotHelpFul = r.NotHelpFul ?? 0,
            Reported = r.Reported ?? 0
        };
    }

    /// <inheritdoc />
    public async Task<int> AddProductReviewAsync(ProductReviewInputDto request)
    {
        try
        {
            int custProfileId = await GetReviewUserProfileAsync(request.UserEmail, 0);
            if (custProfileId == 0)
            {
                custProfileId = await AddReviewUserProfileAsync(
                    request.ProductId, 
                    request.ReviewBy, 
                    string.Empty, 
                    request.UserEmail, 
                    string.Empty, 
                    request.RecommendThis == "Yes", 
                    int.TryParse(request.ReaderType, out int rt) ? rt : 0
                );
            }

            if (await CheckForUserProductReviewAsync(custProfileId, request.ProductId))
            {
                return 0;
            }

            int productReviewId = 0;
            var scalarResult = await _dbContext.Database.SqlQueryRaw<int>(
                "EXEC SProc_AddProductReviewNew @ID_Product, @Review_Title, @Description, @ReviewBy, @Status, @Rating, @Id_CustProfile, @RecomendThis, @ReaderType, @ReaderSpoilers",
                new SqlParameter("@ID_Product", request.ProductId),
                new SqlParameter("@Review_Title", request.ReviewTitle),
                new SqlParameter("@Description", request.Description),
                new SqlParameter("@ReviewBy", request.ReviewBy),
                new SqlParameter("@Status", request.ReviewStatus),
                new SqlParameter("@Rating", request.Rating),
                new SqlParameter("@Id_CustProfile", custProfileId),
                new SqlParameter("@RecomendThis", request.RecommendThis == "Yes"),
                new SqlParameter("@ReaderType", int.TryParse(request.ReaderType, out int rType) ? rType : 0),
                new SqlParameter("@ReaderSpoilers", request.ReaderSpoiler == "Yes" ? 1 : 0)
            ).ToListAsync();

            if (scalarResult.Any())
            {
                productReviewId = scalarResult.First();
            }

            if (request.ReviewTagNames != null && productReviewId > 0)
            {
                foreach (var reviewTag in request.ReviewTagNames)
                {
                    await AddReviewTagUserAsync(request.ProductId, productReviewId, custProfileId, reviewTag.Id);
                }
            }

            if (request.ProductReviewImages != null && productReviewId > 0)
            {
                foreach (var reviewImage in request.ProductReviewImages)
                {
                    await AddReviewImageAsync(request.ProductId, productReviewId, custProfileId, reviewImage.ImageLocation, reviewImage.ImageCaption);
                }
            }

            return productReviewId;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    private async Task<int> GetReviewUserProfileAsync(string custProfileEmail, int custProfileId)
    {
        var results = await _dbContext.Database.SqlQueryRaw<int>(
            "EXEC Sproc_GetReviewUserProfile @Email_ID, @Id_CustProfile",
            new SqlParameter("@Email_ID", custProfileEmail),
            new SqlParameter("@Id_CustProfile", custProfileId)
        ).ToListAsync();
        
        return results.FirstOrDefault(custProfileId);
    }

    private async Task<int> AddReviewUserProfileAsync(long productId, string userName, string userImageLoc, string userEmail, string location, bool recommendThis, int readerTypeId)
    {
        var results = await _dbContext.Database.SqlQueryRaw<int>(
            "EXEC Sproc_InsertReviewUserProfile @ID_Product, @User_Name, @UserImgLoc, @Email_ID, @location, @RecomendThis, @Id_ReaderType",
            new SqlParameter("@ID_Product", productId),
            new SqlParameter("@User_Name", userName),
            new SqlParameter("@UserImgLoc", userImageLoc),
            new SqlParameter("@Email_ID", userEmail),
            new SqlParameter("@location", location),
            new SqlParameter("@RecomendThis", recommendThis),
            new SqlParameter("@Id_ReaderType", readerTypeId)
        ).ToListAsync();
        
        return results.FirstOrDefault();
    }

    private async Task<bool> CheckForUserProductReviewAsync(int custProfileId, long productId)
    {
        var results = await _dbContext.Database.SqlQueryRaw<int>(
            "EXEC SProc_AllReadySubmitReview @Id_CustProfile, @ID_Product",
            new SqlParameter("@Id_CustProfile", custProfileId),
            new SqlParameter("@ID_Product", productId)
        ).ToListAsync();
        
        return results.FirstOrDefault() > 0;
    }

    private async Task AddReviewTagUserAsync(long productId, int productReviewId, int customerId, int reviewTagId)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "EXEC SProc_AddReviewTagUser @Id_CustProfile, @Id_Tag, @ID_ProductReview, @ID_Product",
            new SqlParameter("@Id_CustProfile", customerId),
            new SqlParameter("@Id_Tag", reviewTagId),
            new SqlParameter("@ID_ProductReview", productReviewId),
            new SqlParameter("@ID_Product", productId)
        );
    }

    private async Task AddReviewImageAsync(long productId, int productReviewId, int customerId, string imageLocation, string imgCaption)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "EXEC SProc_AddProductReviewImage @ID_Product, @ID_ProductReview, @ID_Customer, @Image_Location, @ImgCaption",
            new SqlParameter("@ID_Product", productId),
            new SqlParameter("@ID_ProductReview", productReviewId),
            new SqlParameter("@ID_Customer", customerId),
            new SqlParameter("@Image_Location", imageLocation),
            new SqlParameter("@ImgCaption", imgCaption)
        );
    }
}
