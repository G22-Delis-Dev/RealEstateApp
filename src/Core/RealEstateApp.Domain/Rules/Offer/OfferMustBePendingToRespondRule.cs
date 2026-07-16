using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Rules.Offer;

public sealed class OfferMustBePendingToRespondRule : IBusinessRule
{
    private readonly OfferStatus _status;
    public OfferMustBePendingToRespondRule(OfferStatus status) => _status = status;

    public bool IsBroken() => _status != OfferStatus.Pendiente;
    public string Message => "Esta oferta ya fue respondida.";
}