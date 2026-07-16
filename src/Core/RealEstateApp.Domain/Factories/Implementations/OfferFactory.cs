using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Offer;

namespace RealEstateApp.Domain.Factories.Implementations;

public class OfferFactory : IOfferFactory
{
    public Offer Create(Property property, string clientId, decimal amount, IEnumerable<Offer> clientOffersForThisProperty)
    {
        BusinessRuleValidator.CheckRules(
            new OfferAmountMustBePositiveRule(amount),
            new PropertyMustBeAvailableForOfferRule(property.Status),
            new PropertyMustNotHaveAcceptedOfferRule(property.Offers),
            new ClientMustNotHavePendingOfferRule(clientOffersForThisProperty)
        );

        return new Offer
        {
            PropertyId = property.Id,
            ClientId = clientId,
            Amount = amount,
            Status = OfferStatus.Pendiente,
            OfferedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
    }
}