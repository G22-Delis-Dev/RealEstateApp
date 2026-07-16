using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class AgentQueryRepository : IAgentQueryRepository
{
    private readonly ApplicationDbContext _context;

    public AgentQueryRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<string>> GetActiveAgentIdsAsync()
    {

        return await _context.Properties
            .Select(p => p.AgentId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<int> CountPropertiesByAgentAsync(string agentId)
        => await _context.Properties.CountAsync(p => p.AgentId == agentId);
}