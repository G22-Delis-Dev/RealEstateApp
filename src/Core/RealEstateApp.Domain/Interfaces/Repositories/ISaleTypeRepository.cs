using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface ISaleTypeRepository : IBaseRepository<SaleType>
{
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
}