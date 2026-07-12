using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Catalog;

namespace RealEstateApp.Domain.Factories.Implementations;

public class PropertyTypeFactory : IPropertyTypeFactory
{
    public PropertyType Create(string name, string description, bool nameAlreadyExists)
    {
        BusinessRuleValidator.CheckRules(
            new NameMustNotBeEmptyOrWhitespaceRule(name),
            new NameMustBeUniqueRule(nameAlreadyExists, "tipo de propiedad")
        );

        return new PropertyType { Name = name.Trim(), Description = description };
    }
}