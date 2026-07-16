using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IFavoriteRepository : IBaseRepository<Favorite>
{
    Task<bool> ExistsAsync(string clientId, int propertyId);
    Task<IEnumerable<Favorite>> GetByClientAsync(string clientId);
    Task RemoveAsync(string clientId, int propertyId);
}