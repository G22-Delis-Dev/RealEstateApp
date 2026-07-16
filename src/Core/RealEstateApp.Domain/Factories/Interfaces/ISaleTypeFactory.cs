using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Factories.Interfaces;

public interface ISaleTypeFactory
{
    SaleType Create(string name, string description, bool nameAlreadyExists);
}