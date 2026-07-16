using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Factories.Interfaces;

public interface IFavoriteFactory
{
    Favorite Create(string clientId, int propertyId, bool alreadyFavorited);
}