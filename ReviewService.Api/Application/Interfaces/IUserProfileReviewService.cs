using ReviewService.Api.Application.Common;
using ReviewService.Api.Application.Models;

namespace ReviewService.Api.Application.Interfaces;

public interface IUserProfileReviewService
{
    // Notice we wrap the DTO in our standard ServiceResult wrapper
    Task<ServiceResult<UserProfileReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow);
}
