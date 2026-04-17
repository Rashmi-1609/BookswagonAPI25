using PublisherService.Api.Application.Common;
using PublisherService.Api.Domain.Entities;

namespace PublisherService.Api.Application.Interfaces;

// This is a CONTRACT -it defines WHAT operations exist
// The actual implementation is in PublisherService.cs
public interface IPublisherService
{
    Task<ServiceResult<Publisher>> GetPublisherByIdAsync(int id);
    ServiceResult<IQueryable<Publisher>> GetPublishersByName(string name);
}
