using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class ImprovementRepository : GenericRepository<Improvement>, IImprovementRepository
{
    public ImprovementRepository(ApplicationDbContext context) : base(context) { }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        var query = _dbSet.Where(i => i.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
            query = query.Where(i => i.Id != excludeId.Value);

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Improvement>> GetByIdsAsync(IEnumerable<int> ids)
        => await _dbSet.Where(i => ids.Contains(i.Id)).ToListAsync();
}