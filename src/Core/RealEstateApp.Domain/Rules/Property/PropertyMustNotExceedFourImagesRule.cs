namespace RealEstateApp.Domain.Rules.Property;

public sealed class PropertyMustNotExceedFourImagesRule : IBusinessRule
{
    private readonly int _imageCount;
    public PropertyMustNotExceedFourImagesRule(int imageCount) => _imageCount = imageCount;

    public bool IsBroken() => _imageCount > 4;
    public string Message => "Solo se permite registrar hasta 4 imágenes por propiedad.";
}