using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Factories.Interfaces;

public interface IImprovementFactory
{
    Improvement Create(string name, string description, bool nameAlreadyExists);
}