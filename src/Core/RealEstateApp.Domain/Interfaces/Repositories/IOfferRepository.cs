using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IOfferRepository : IBaseRepository<Offer>
{
    Task<IEnumerable<Offer>> GetByClientAndPropertyAsync(string clientId, int propertyId);
    Task<IEnumerable<Offer>> GetPendingByPropertyAsync(int propertyId, int? excludeOfferId = null);
    Task<IEnumerable<Offer>> GetByPropertyAsync(int propertyId);
}