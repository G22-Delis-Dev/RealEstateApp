using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Catalog;

namespace RealEstateApp.Domain.Factories.Implementations;

public class SaleTypeFactory : ISaleTypeFactory
{
    public SaleType Create(string name, string description, bool nameAlreadyExists)
    {
        BusinessRuleValidator.CheckRules(
            new NameMustNotBeEmptyOrWhitespaceRule(name),
            new NameMustBeUniqueRule(nameAlreadyExists, "tipo de venta")
        );

        return new SaleType { Name = name.Trim(), Description = description };
    }
}