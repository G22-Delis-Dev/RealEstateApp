using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Catalog;

namespace RealEstateApp.Domain.Factories.Implementations;

public class ImprovementFactory : IImprovementFactory
{
    public Improvement Create(string name, string description, bool nameAlreadyExists)
    {
        BusinessRuleValidator.CheckRules(
            new NameMustNotBeEmptyOrWhitespaceRule(name),
            new NameMustBeUniqueRule(nameAlreadyExists, "mejora")
        );

        return new Improvement { Name = name.Trim(), Description = description };
    }
}