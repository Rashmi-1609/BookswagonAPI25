using PublisherService.Application.Interfaces;
using PublisherService.Application.DTO;
using PublisherService.Domain.Interfaces;


namespace PublisherService.Application.Services;

/// <summary>
/// Implements the core business rules for publishers before delegating to the repository.
/// </summary>
public class PublisherService(IPublisherRepository repository) : IPublisherService
{
    public async Task<PublisherDto?> GetPublisherByIdAsync(int id)
    {
        // Rule -> IDs must be positive
        if (id <= 0)
        {
            return null;
        }

        var publisher = await repository.GetPublisherByIdAsync(id);

        if (publisher == null) return null;

        return new PublisherDto
        {
            PublisherId = publisher.PublisherId,
            CompanyName = publisher.CompanyName,
            PublisherImage = publisher.PublisherImage,
            Description = publisher.Description,
            MetaDescription = publisher.MetaDescription,
            MetaKeywords = publisher.MetaKeywords,
            PageTitle = publisher.PageTitle
        };
    }

    public IQueryable<PublisherDto> GetPublishersByName(string name)
    {
        // Rule -> Don't search if name is empty
        if (string.IsNullOrWhiteSpace(name))
        {
            return Enumerable.Empty<PublisherDto>().AsQueryable();
        }

        // Map IQueryable<Entity> -> IQueryable<DTO> using Select
        // EF Core will translate this into a highly optimized SQL SELECT statement!
        return repository.GetPublishersByName(name).Select(p => new PublisherDto
        {
            PublisherId = p.PublisherId,
            CompanyName = p.CompanyName,
            PublisherImage = p.PublisherImage,
            Description = p.Description,
            MetaDescription = p.MetaDescription,
            MetaKeywords = p.MetaKeywords,
            PageTitle = p.PageTitle
        });
    }
}
