using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IPropertyRepository : IBaseRepository<Property>
{
    Task<Property?> GetByCodeAsync(string code);
    Task<bool> CodeExistsAsync(string code);
    Task<IEnumerable<Property>> GetAvailableAsync();
    Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId);
    Task<IEnumerable<Property>> FilterAsync(int? propertyTypeId, decimal? minPrice, decimal? maxPrice, int? rooms, int? bathrooms);
}