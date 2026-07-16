using RealEstateApp.Application.ViewModels.Messages;

namespace RealEstateApp.Application.Interfaces.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageViewModel>> GetConversationAsync(int propertyId, string clientId, string agentId);
    Task<IEnumerable<string>> GetClientIdsWithConversationAsync(int propertyId);
    Task<MessageViewModel> SendAsync(int propertyId, string clientId, string agentId, string senderId, string content);
}