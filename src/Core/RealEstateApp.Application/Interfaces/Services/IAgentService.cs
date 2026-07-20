using RealEstateApp.Application.ViewModels.Agents;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IAgentService
{
    Task<IEnumerable<AgentListItemViewModel>> GetAllAsync();
    Task<AgentViewModel?> GetByIdAsync(string agentId);
    Task<IEnumerable<ViewModels.Properties.PropertyViewModel>> GetAgentPropertiesAsync(string agentId);
    Task ChangeStatusAsync(string agentId, bool isActive);
}