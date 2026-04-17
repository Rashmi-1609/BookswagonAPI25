using PublisherService.Api.Application.Common;
using PublisherService.Api.Domain.Entities;
using PublisherService.Api.Application.Interfaces;

namespace PublisherService.Api.Application.Services;

//  using primary constructor syntax to inject the IPublisherRepository dependency
public class PublisherService(IPublisherRepository repository) : IPublisherService
{
    public async Task<ServiceResult<Publisher>> GetPublisherByIdAsync(int id)
    {
        // Business Rule: IDs must be positive
        if (id <= 0)
        {
            return ServiceResult<Publisher>.Failure("Invalid Publisher ID. ID must be greater than 0.");
        }

        try
        {
            var publisher = await repository.GetByIdAsync(id);

            if (publisher == null)
            {
                return ServiceResult<Publisher>.Failure($"Publisher with ID {id} not found.");
            }

            return ServiceResult<Publisher>.Success(publisher);
        }
        catch (Exception ex)
        {
            // Logging would happen here in a real production app
            return ServiceResult<Publisher>.Failure("An error occurred while fetching the publisher.");
        }
    }

    public ServiceResult<IQueryable<Publisher>> GetPublishersByName(string name)
    {
        // Business Rule: Don't allow empty searches
        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult<IQueryable<Publisher>>.Failure("Search name cannot be empty.");
        }

        try
        {
            var publishers = repository.GetByName(name);
            return ServiceResult<IQueryable<Publisher>>.Success(publishers);
        }
        catch (Exception)
        {
            return ServiceResult<IQueryable<Publisher>>.Failure("An error occurred during the search.");
        }
    }
}
