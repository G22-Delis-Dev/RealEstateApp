using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IPropertyAdminRepository
{
    Task<IEnumerable<Property>> GetByAgentIdIncludingSoldAsync(string agentId);
    Task DeleteWithRelatedDataAsync(int propertyId);
}