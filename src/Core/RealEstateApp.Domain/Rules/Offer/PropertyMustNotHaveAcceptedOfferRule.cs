using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Rules.Offer;

public sealed class PropertyMustNotHaveAcceptedOfferRule : IBusinessRule
{
    private readonly IEnumerable<Entities.Offer> _propertyOffers;
    public PropertyMustNotHaveAcceptedOfferRule(IEnumerable<Entities.Offer> propertyOffers) => _propertyOffers = propertyOffers;

    public bool IsBroken() => _propertyOffers.Any(o => o.Status == OfferStatus.Aceptada);
    public string Message => "Esta propiedad ya tiene una oferta aceptada y no permite nuevas ofertas.";
}