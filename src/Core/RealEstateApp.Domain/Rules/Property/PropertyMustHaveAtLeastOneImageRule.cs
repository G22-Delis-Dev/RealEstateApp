namespace RealEstateApp.Domain.Rules.Property;

public sealed class PropertyMustHaveAtLeastOneImageRule : IBusinessRule
{
    private readonly int _imageCount;
    public PropertyMustHaveAtLeastOneImageRule(int imageCount) => _imageCount = imageCount;

    public bool IsBroken() => _imageCount < 1;
    public string Message => "Debe cargar al menos una imagen de la propiedad.";
}