namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IAgentQueryRepository
{
    Task<IEnumerable<string>> GetActiveAgentIdsAsync();
    Task<int> CountPropertiesByAgentAsync(string agentId);
}