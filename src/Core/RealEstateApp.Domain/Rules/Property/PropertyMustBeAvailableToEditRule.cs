using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Rules.Property;

public sealed class PropertyMustBeAvailableToEditRule : IBusinessRule
{
    private readonly PropertyStatus _status;
    public PropertyMustBeAvailableToEditRule(PropertyStatus status) => _status = status;

    public bool IsBroken() => _status != PropertyStatus.Disponible;
    public string Message => "No se puede modificar una propiedad que ya fue vendida.";
}