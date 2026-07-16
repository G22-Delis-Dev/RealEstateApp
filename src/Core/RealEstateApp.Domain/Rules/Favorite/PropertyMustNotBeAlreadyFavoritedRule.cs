namespace RealEstateApp.Domain.Rules.Favorite;

public sealed class PropertyMustNotBeAlreadyFavoritedRule : IBusinessRule
{
    private readonly bool _alreadyFavorited;
    public PropertyMustNotBeAlreadyFavoritedRule(bool alreadyFavorited) => _alreadyFavorited = alreadyFavorited;

    public bool IsBroken() => _alreadyFavorited;
    public string Message => "Esta propiedad ya se encuentra en su listado de favoritas.";
}