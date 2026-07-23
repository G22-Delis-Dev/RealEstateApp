using RealEstateApp.Application.DTOs.Properties;
using RealEstateApp.Application.ViewModels.Agents;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IAgentService
{
    Task<IEnumerable<AgentListItemViewModel>> GetAllAsync();
    Task<AgentViewModel?> GetByIdAsync(string agentId);
    Task<IEnumerable<ViewModels.Properties.PropertyViewModel>> GetAgentPropertiesAsync(string agentId);
    Task<IEnumerable<ViewModels.Properties.PropertyViewModel>> GetAgentPropertiesIncludingSoldAsync(string agentId);
    Task ChangeStatusAsync(string agentId, bool isActive);
    Task DeleteAgentAsync(string agentId);

    // Exclusivo para la API — retorna PropertyDto, nunca el ViewModel de la WebApp
    Task<IEnumerable<PropertyDto>> GetAgentPropertiesForApiAsync(string agentId);
}