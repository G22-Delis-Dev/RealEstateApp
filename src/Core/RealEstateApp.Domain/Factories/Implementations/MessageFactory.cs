using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Rules;
using RealEstateApp.Domain.Rules.Message;

namespace RealEstateApp.Domain.Factories.Implementations;

public class MessageFactory : IMessageFactory
{
    public Message Create(int propertyId, string clientId, string agentId, string senderId, string content)
    {
        BusinessRuleValidator.CheckRule(new MessageMustNotBeEmptyRule(content));

        return new Message
        {
            PropertyId = propertyId,
            ClientId = clientId,
            AgentId = agentId,
            SenderId = senderId,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
    }
}