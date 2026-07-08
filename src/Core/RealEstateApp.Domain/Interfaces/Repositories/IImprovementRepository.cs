using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IImprovementRepository : IBaseRepository<Improvement>
{
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
    Task<IEnumerable<Improvement>> GetByIdsAsync(IEnumerable<int> ids);
}