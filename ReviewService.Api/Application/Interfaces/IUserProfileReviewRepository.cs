using ReviewService.Api.Application.Models;

namespace ReviewService.Api.Application.Interfaces;

public interface IUserProfileReviewRepository
{
    // return a single Parent DTO, which contains the List of Books inside it
    Task<UserProfileReviewDto?> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow);
}
