using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Factories.Interfaces;

public interface IOfferFactory
{
    Offer Create(Property property, string clientId, decimal amount, IEnumerable<Offer> clientOffersForThisProperty);
}