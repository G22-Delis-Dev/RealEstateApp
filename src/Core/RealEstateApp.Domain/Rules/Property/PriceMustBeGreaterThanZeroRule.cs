namespace RealEstateApp.Domain.Rules.Property;

public sealed class PriceMustBeGreaterThanZeroRule : IBusinessRule
{
    private readonly decimal _price;
    public PriceMustBeGreaterThanZeroRule(decimal price) => _price = price;

    public bool IsBroken() => _price <= 0;
    public string Message => "El precio debe ser mayor que cero.";
}