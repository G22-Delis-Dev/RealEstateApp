using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Property;
using RealEstateApp.Domain.Services.Interfaces;

namespace RealEstateApp.Domain.Services.Implementations;

public class PropertyDomainService : IPropertyDomainService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PropertyDomainService(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeletePropertyWithValidationAsync(int propertyId, string agentId)
    {
        var property = await _propertyRepository.GetByIdAsync(propertyId)
            ?? throw new InvalidOperationException("La propiedad no existe.");

        BusinessRuleValidator.CheckRule(new PropertyMustBeAvailableToDeleteRule(property.Status));

        _propertyRepository.Remove(property);
        await _unitOfWork.SaveChangesAsync();
    }
}