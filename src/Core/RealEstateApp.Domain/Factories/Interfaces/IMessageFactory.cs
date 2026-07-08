using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Domain.Factories.Interfaces;

public interface IMessageFactory
{
    Message Create(int propertyId, string clientId, string agentId, string senderId, string content);
}