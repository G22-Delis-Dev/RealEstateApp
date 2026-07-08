namespace RealEstateApp.Domain.Rules.Catalog;

public sealed class NameMustNotBeEmptyOrWhitespaceRule : IBusinessRule
{
    private readonly string _name;
    public NameMustNotBeEmptyOrWhitespaceRule(string name) => _name = name;

    public bool IsBroken() => string.IsNullOrWhiteSpace(_name);
    public string Message => "El nombre no puede estar vacío ni contener solo espacios en blanco.";
}