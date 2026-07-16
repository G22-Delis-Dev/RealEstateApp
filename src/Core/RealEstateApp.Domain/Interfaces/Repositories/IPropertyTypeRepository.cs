using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IPropertyTypeRepository : IBaseRepository<PropertyType>
{
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
}