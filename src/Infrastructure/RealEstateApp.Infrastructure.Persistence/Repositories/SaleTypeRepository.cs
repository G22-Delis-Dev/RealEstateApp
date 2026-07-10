using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
{
    public SaleTypeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        var query = _dbSet.Where(st => st.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
            query = query.Where(st => st.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}