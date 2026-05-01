using ReviewService.Api.Application.Common;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Models;

namespace ReviewService.Api.Application.Services;

public class UserProfileReviewService(IUserProfileReviewRepository repository) : IUserProfileReviewService
{
    public async Task<ServiceResult<UserProfileReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
    {
        if (customerProfileId <= 0)
        {
            return ServiceResult<UserProfileReviewDto>.Failure("Invalid Customer Profile ID.");
        }

        if (pageNo < 1)
        {
            return ServiceResult<UserProfileReviewDto>.Failure("Page number must be 1 or greater.");
        }

        if (noOfRow <= 0 || noOfRow > 50)
        {
            return ServiceResult<UserProfileReviewDto>.Failure("Number of rows must be between 1 and 50 per request to prevent system overload.");
        }

        // fetch the data from the repository
        var result = await repository.GetUserProfileReviewsAsync(customerProfileId, pageNo, noOfRow);

        // handle the "Empty State" Trap safely
        if (result == null)
        {
            // If a user has 0 reviews, it is NOT an error. It's just an empty state.
            // We return a Success with an empty DTO so the frontend doesn't crash trying to render it.
            return ServiceResult<UserProfileReviewDto>.Success(new UserProfileReviewDto());
        }

        return ServiceResult<UserProfileReviewDto>.Success(result);
    }
}
