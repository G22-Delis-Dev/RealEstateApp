using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
{
    public PropertyRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<Property?> GetByIdAsync(int id)
        => await _dbSet
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Include(p => p.Improvements)
            .Include(p => p.Offers)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Property?> GetByCodeAsync(string code)
        => await _dbSet
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Include(p => p.Improvements)
            .FirstOrDefaultAsync(p => p.Code == code);

    public async Task<bool> CodeExistsAsync(string code)
        => await _dbSet.AnyAsync(p => p.Code == code);

    public async Task<IEnumerable<Property>> GetAvailableAsync()
        => await _dbSet
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.Status == Domain.Enums.PropertyStatus.Disponible)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Property>> GetByAgentIdAsync(string agentId)
        => await _dbSet
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.AgentId == agentId && p.Status == Domain.Enums.PropertyStatus.Disponible)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Property>> FilterAsync(
        int? propertyTypeId, decimal? minPrice, decimal? maxPrice, int? rooms, int? bathrooms)
    {
        var query = _dbSet
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Where(p => p.Status == Domain.Enums.PropertyStatus.Disponible);

        if (propertyTypeId.HasValue)
            query = query.Where(p => p.PropertyTypeId == propertyTypeId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (rooms.HasValue)
            query = query.Where(p => p.Rooms == rooms.Value);

        if (bathrooms.HasValue)
            query = query.Where(p => p.Bathrooms == bathrooms.Value);

        return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }
}