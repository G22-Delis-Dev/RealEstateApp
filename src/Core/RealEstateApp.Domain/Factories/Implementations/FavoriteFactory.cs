using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Favorite;

namespace RealEstateApp.Domain.Factories.Implementations;

public class FavoriteFactory : IFavoriteFactory
{
    public Favorite Create(string clientId, int propertyId, bool alreadyFavorited)
    {
        BusinessRuleValidator.CheckRule(new PropertyMustNotBeAlreadyFavoritedRule(alreadyFavorited));

        return new Favorite
        {
            ClientId = clientId,
            PropertyId = propertyId,
            MarkedAt = DateTime.UtcNow
        };
    }
}