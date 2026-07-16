using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Property;

namespace RealEstateApp.Domain.Factories.Implementations;

public class PropertyFactory : IPropertyFactory
{
    public Property Create(
        string agentId, int propertyTypeId, int saleTypeId, decimal price,
        string description, decimal size, int rooms, int bathrooms,
        List<string> imageUrls, List<Improvement> improvements, string code)
    {
        BusinessRuleValidator.CheckRules(
            new PriceMustBeGreaterThanZeroRule(price),
            new PropertySizeMustBeGreaterThanZeroRule(size),
            new RoomsAndBathroomsCannotBeNegativeRule(rooms, bathrooms),
            new PropertyMustHaveAtLeastOneImageRule(imageUrls.Count),
            new PropertyMustNotExceedFourImagesRule(imageUrls.Count),
            new PropertyMustHaveAtLeastOneImprovementRule(improvements.Count)
        );

        return new Property
        {
            Code = code,
            AgentId = agentId,
            PropertyTypeId = propertyTypeId,
            SaleTypeId = saleTypeId,
            Price = price,
            Description = description,
            Size = size,
            Rooms = rooms,
            Bathrooms = bathrooms,
            Status = PropertyStatus.Disponible,
            Images = imageUrls.Select(url => new PropertyImage { Url = url }).ToList(),
            Improvements = improvements,
            CreatedAt = DateTime.UtcNow
        };
    }
}