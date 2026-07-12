namespace RealEstateApp.Domain.Rules.Offer;

public sealed class OfferAmountMustBePositiveRule : IBusinessRule
{
    private readonly decimal _amount;
    public OfferAmountMustBePositiveRule(decimal amount) => _amount = amount;

    public bool IsBroken() => _amount <= 0;
    public string Message => "El monto de la oferta debe ser un valor numérico mayor que cero.";
}