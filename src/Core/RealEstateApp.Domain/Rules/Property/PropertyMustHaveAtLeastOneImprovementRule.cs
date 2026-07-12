namespace RealEstateApp.Domain.Rules.Property;

public sealed class PropertyMustHaveAtLeastOneImprovementRule : IBusinessRule
{
    private readonly int _improvementCount;
    public PropertyMustHaveAtLeastOneImprovementRule(int improvementCount) => _improvementCount = improvementCount;

    public bool IsBroken() => _improvementCount < 1;
    public string Message => "Debe seleccionarse al menos una mejora.";
}