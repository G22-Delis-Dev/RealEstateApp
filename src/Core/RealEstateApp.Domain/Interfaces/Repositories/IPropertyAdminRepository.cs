using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IPropertyAdminRepository
{
    Task<IEnumerable<Property>> GetByAgentIdIncludingSoldAsync(string agentId);
    Task<IEnumerable<Property>> GetByPropertyTypeIdAsync(int propertyTypeId); 
    Task<IEnumerable<Property>> GetBySaleTypeIdAsync(int saleTypeId);
    Task DeleteWithRelatedDataAsync(int propertyId);
}