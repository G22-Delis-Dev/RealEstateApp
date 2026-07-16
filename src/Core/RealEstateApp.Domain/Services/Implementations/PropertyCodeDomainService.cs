using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Domain.Services.Interfaces;

namespace RealEstateApp.Domain.Services.Implementations;

public class PropertyCodeDomainService : IPropertyCodeDomainService
{
    private readonly IPropertyRepository _propertyRepository;
    private static readonly Random _random = new();

    public PropertyCodeDomainService(IPropertyRepository propertyRepository)
        => _propertyRepository = propertyRepository;

    public async Task<string> GenerateUniqueCodeAsync()
    {
        string code;
        do
        {
            code = _random.Next(100000, 999999).ToString();
        } while (await _propertyRepository.CodeExistsAsync(code));

        return code;
    }
}