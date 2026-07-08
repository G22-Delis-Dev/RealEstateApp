using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Rules.Property;

public sealed class PropertyMustBeAvailableToDeleteRule : IBusinessRule
{
    private readonly PropertyStatus _status;
    public PropertyMustBeAvailableToDeleteRule(PropertyStatus status) => _status = status;

    public bool IsBroken() => _status != PropertyStatus.Disponible;
    public string Message => "No se puede eliminar una propiedad que ya fue vendida.";
}