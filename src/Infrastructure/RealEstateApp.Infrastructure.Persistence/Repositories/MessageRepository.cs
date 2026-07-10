using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Message>> GetConversationAsync(int propertyId, string clientId, string agentId)
        => await _dbSet
            .Where(m => m.PropertyId == propertyId && m.ClientId == clientId && m.AgentId == agentId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

    public async Task<IEnumerable<string>> GetClientIdsWithConversationAsync(int propertyId)
        => await _dbSet
            .Where(m => m.PropertyId == propertyId)
            .Select(m => m.ClientId)
            .Distinct()
            .ToListAsync();
}