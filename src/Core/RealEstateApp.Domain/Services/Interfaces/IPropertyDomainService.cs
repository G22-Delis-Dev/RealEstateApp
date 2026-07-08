namespace RealEstateApp.Domain.Services.Interfaces;

public interface IPropertyDomainService
{
    Task DeletePropertyWithValidationAsync(int propertyId, string agentId);
}