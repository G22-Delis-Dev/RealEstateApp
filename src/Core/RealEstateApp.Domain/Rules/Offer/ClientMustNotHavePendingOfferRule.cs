using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Rules.Offer;

public sealed class ClientMustNotHavePendingOfferRule : IBusinessRule
{
    private readonly IEnumerable<Entities.Offer> _clientOffersForThisProperty;
    public ClientMustNotHavePendingOfferRule(IEnumerable<Entities.Offer> clientOffersForThisProperty)
        => _clientOffersForThisProperty = clientOffersForThisProperty;

    public bool IsBroken() => _clientOffersForThisProperty.Any(o => o.Status == OfferStatus.Pendiente);
    public string Message => "Ya tiene una oferta pendiente para esta propiedad.";
}