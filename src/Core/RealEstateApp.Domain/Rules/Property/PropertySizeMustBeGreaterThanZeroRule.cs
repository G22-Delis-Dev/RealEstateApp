namespace RealEstateApp.Domain.Rules.Property;

public sealed class PropertySizeMustBeGreaterThanZeroRule : IBusinessRule
{
    private readonly decimal _size;
    public PropertySizeMustBeGreaterThanZeroRule(decimal size) => _size = size;

    public bool IsBroken() => _size <= 0;
    public string Message => "El tamaño de la propiedad debe ser mayor que cero.";
}