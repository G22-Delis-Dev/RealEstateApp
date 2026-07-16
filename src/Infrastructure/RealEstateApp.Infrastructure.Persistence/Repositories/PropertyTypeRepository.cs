using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
{
    public PropertyTypeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        var query = _dbSet.Where(pt => pt.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
            query = query.Where(pt => pt.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}