using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Factories.Interfaces;

public interface IPropertyTypeFactory
{
    PropertyType Create(string name, string description, bool nameAlreadyExists);
}