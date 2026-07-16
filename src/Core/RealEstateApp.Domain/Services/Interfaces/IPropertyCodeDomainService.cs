namespace RealEstateApp.Domain.Services.Interfaces;

public interface IPropertyCodeDomainService
{
    Task<string> GenerateUniqueCodeAsync();
}