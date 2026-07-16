using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Interfaces.Repositories;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<IEnumerable<Message>> GetConversationAsync(int propertyId, string clientId, string agentId);
    Task<IEnumerable<string>> GetClientIdsWithConversationAsync(int propertyId);
}