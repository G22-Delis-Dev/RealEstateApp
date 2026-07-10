using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class OfferRepository : GenericRepository<Offer>, IOfferRepository
{
    public OfferRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Offer>> GetByClientAndPropertyAsync(string clientId, int propertyId)
        => await _dbSet
            .Where(o => o.ClientId == clientId && o.PropertyId == propertyId)
            .OrderByDescending(o => o.OfferedAt)
            .ToListAsync();

    public async Task<IEnumerable<Offer>> GetPendingByPropertyAsync(int propertyId, int? excludeOfferId = null)
    {
        var query = _dbSet.Where(o =>
            o.PropertyId == propertyId &&
            o.Status == OfferStatus.Pendiente);

        if (excludeOfferId.HasValue)
            query = query.Where(o => o.Id != excludeOfferId.Value);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Offer>> GetByPropertyAsync(int propertyId)
        => await _dbSet
            .Where(o => o.PropertyId == propertyId)
            .OrderByDescending(o => o.OfferedAt)
            .ToListAsync();
}