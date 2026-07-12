using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Factories.Interfaces;

public interface IPropertyFactory
{
    Property Create(
        string agentId, int propertyTypeId, int saleTypeId, decimal price,
        string description, decimal size, int rooms, int bathrooms,
        List<string> imageUrls, List<Improvement> improvements, string code);
}