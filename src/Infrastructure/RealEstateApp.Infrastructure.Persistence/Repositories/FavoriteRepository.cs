using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(ApplicationDbContext context) : base(context) { }

    public async Task<bool> ExistsAsync(string clientId, int propertyId)
        => await _dbSet.AnyAsync(f => f.ClientId == clientId && f.PropertyId == propertyId);

    public async Task<IEnumerable<Favorite>> GetByClientAsync(string clientId)
        => await _dbSet
            .Include(f => f.Property)
                .ThenInclude(p => p.PropertyType)
            .Include(f => f.Property)
                .ThenInclude(p => p.SaleType)
            .Include(f => f.Property)
                .ThenInclude(p => p.Images)
            .Where(f => f.ClientId == clientId && f.Property.Status == Domain.Enums.PropertyStatus.Disponible)
            .OrderByDescending(f => f.MarkedAt)
            .ToListAsync();

    public async Task RemoveAsync(string clientId, int propertyId)
    {
        var favorite = await _dbSet.FirstOrDefaultAsync(f => f.ClientId == clientId && f.PropertyId == propertyId);
        if (favorite is not null)
            _dbSet.Remove(favorite);
    }
}