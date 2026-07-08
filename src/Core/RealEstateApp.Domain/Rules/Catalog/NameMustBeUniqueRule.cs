namespace RealEstateApp.Domain.Rules.Catalog;

public sealed class NameMustBeUniqueRule : IBusinessRule
{
    private readonly bool _nameAlreadyExists;
    private readonly string _entityLabel;

    public NameMustBeUniqueRule(bool nameAlreadyExists, string entityLabel)
    {
        _nameAlreadyExists = nameAlreadyExists;
        _entityLabel = entityLabel;
    }

    public bool IsBroken() => _nameAlreadyExists;
    public string Message => $"Ya existe un(a) {_entityLabel} registrado(a) con este nombre.";
}