using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;
using ReviewService.Api.Infrastructure.Data;
using ReviewService.Api.Infrastructure.Models;

namespace ReviewService.Api.Infrastructure.Repositories;

public class UserProfileReviewRepository(AppDbContext context) : IUserProfileReviewRepository
{
    public async Task<UserProfileReviewDto?> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
    {
        // Convert UI page number (starts at 1) to SQL math (starts at 0)
        var sqlPageNo = pageNo - 1;

        var sql = "EXEC [dbo].[SProc_GetUserProfileReviews_WithImages] @Id_CustProfile, @PageNo, @NoOfRow";
        var parameters = new[]
        {
            new SqlParameter("@Id_CustProfile", customerProfileId),
            new SqlParameter("@PageNo", sqlPageNo),
            new SqlParameter("@NoOfRow", noOfRow)
        };

        // fetching raw results from SP and map them using Loading Dock class (UserProfileReviewSpResult)
        var spResults = await context.Database
            .SqlQueryRaw<UserProfileReviewSpResult>(sql, parameters)
            .ToListAsync();

        // if the user has no reviews, return null to handle it gracefully in the Service
        if (!spResults.Any()) return null;

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Grab the User's details from the very first row (since it's repeated on every row)
        var firstRow = spResults.First();

        // Assemble the Parent Folder and the Nested Array
        return new UserProfileReviewDto
        {
            ReviewBy = firstRow.User_Name,        
            UserEmail = firstRow.Email_Id,
            TotalReview = firstRow.TotalReview,
            ReaderType = firstRow.ReaderType,

            // Loop over all rows and map only the book specific data into the array
            ReviewedBooks = spResults.Select(r => new ReviewedBookDto
            {
                ISBN13 = r.ISBN13,
                ProductTitle = r.Product_Title,
                ProductTitleUrl = r.Product_TitleUrl,
                ProductImageLocation = r.Image_Location,
                Rating = r.Rating,
                DateCreated = r.Date_Created.ToString("MMM dd, yyyy"), // Cleanly formatting Date_Created here!
                ReaderSpoiler = r.ReaderSpoilers,
                ReviewTitle = r.Review_Title,
                Description = r.Description,

                // deserialize the JSON array of images!
                ProductReviewImages = string.IsNullOrWhiteSpace(r.ReviewImagesJson)
                    ? new List<ProductReviewImageDto>()
                    : JsonSerializer.Deserialize<List<ProductReviewImageDto>>(r.ReviewImagesJson, jsonOptions) ?? new()
            }).ToList()
        };
    }
}
