using System;
using System.Collections.Generic;
using System.Text;
using PublisherService.Domain.Entities;

namespace PublisherService.Application.Interfaces;

/// <summary>
/// Defines the business logic and validation rules for Publisher operations.
/// </summary>
public interface IPublisherService
{
    /// <summary>
    /// Validates the ID and retrieves a publisher.
    /// </summary>
    /// <param name="id">The unique ID of the publisher.</param>
    /// <returns>The Publisher entity, or null if validation fails or it is not found.</returns>
    Task<Publisher?> GetPublisherByIdAsync(int id);

    /// <summary>
    /// Validates the search string and retrieves matching publishers.
    /// </summary>
    /// <param name="name">The search string.</param>
    /// <returns>An IQueryable of matching publishers, or an empty query if validation fails.</returns>
    IQueryable<Publisher> GetPublishersByName(string name);
}
