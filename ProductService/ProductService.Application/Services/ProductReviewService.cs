using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;

namespace ProductService.Application.Services;

/// <summary>
/// Service layer for handling Product Review business logic.
/// </summary>
public class ProductReviewService(IProductReviewRepository repository) : IProductReviewService
{
    private readonly IProductReviewRepository _repository = repository;

    /// <inheritdoc />
    public async Task<IEnumerable<ProductReviewDto>> GetProductReviewByIdAsync(int productId)
    {
        return await _repository.GetProductReviewByIdAsync(productId);
    }

    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetProductRatingCountAsync(int productId)
    {
        return await _repository.GetProductRatingCountAsync(productId);
    }

    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetProductReviewDetailAsync(int productId, int starOne, int starTwo, int starThree, int starFour, int starFive, int readerSpoiler, int recomendThis, int sortByFilter, int pageNo, int noOfRow, string readerType, string languageType)
    {
        return await _repository.GetProductReviewDetailAsync(productId, starOne, starTwo, starThree, starFour, starFive, readerSpoiler, recomendThis, sortByFilter, pageNo, noOfRow, readerType, languageType);
    }

    /// <inheritdoc />
    public async Task<List<ProductReviewDto>> GetUserProfileReviewsAsync(int customerProfileId, int pageNo, int noOfRow)
    {
        return await _repository.GetUserProfileReviewsAsync(customerProfileId, pageNo, noOfRow);
    }

    /// <inheritdoc />
    public async Task<List<ReviewReaderTypeDto>> GetReviewReaderTypeAsync(int productId)
    {
        return await _repository.GetReviewReaderTypeAsync(productId);
    }

    /// <inheritdoc />
    public async Task<List<ReviewReaderTypeDto>> GetAllReviewReaderTypeAsync()
    {
        return await _repository.GetAllReviewReaderTypeAsync();
    }

    /// <inheritdoc />
    public async Task<List<ReviewTagNameDto>> GetReviewTagsNameAsync()
    {
        return await _repository.GetReviewTagsNameAsync();
    }

    /// <inheritdoc />
    public async Task<ReviewHelpFulDto?> TakeUserVotingAsync(int custProfileId, int productId, int productReviewId, string userCookiesId, int votingType)
    {
        return await _repository.TakeUserVotingAsync(custProfileId, productId, productReviewId, userCookiesId, votingType);
    }

    /// <inheritdoc />
    public async Task<int> AddProductReviewAsync(ProductReviewInputDto inputDto)
    {
        if (inputDto == null) return 0;
        return await _repository.AddProductReviewAsync(inputDto);
    }
}
