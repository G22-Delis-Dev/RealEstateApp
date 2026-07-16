using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Rules.Offer;

public sealed class PropertyMustBeAvailableForOfferRule : IBusinessRule
{
    private readonly PropertyStatus _status;
    public PropertyMustBeAvailableForOfferRule(PropertyStatus status) => _status = status;

    public bool IsBroken() => _status != PropertyStatus.Disponible;
    public string Message => "Esta propiedad ya no se encuentra disponible para recibir ofertas.";
}