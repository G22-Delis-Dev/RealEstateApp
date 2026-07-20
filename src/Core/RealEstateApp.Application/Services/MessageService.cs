using AutoMapper;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.ViewModels.Messages;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Interfaces.Repositories;

namespace RealEstateApp.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageFactory _messageFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageService(
        IMessageRepository messageRepository,
        IMessageFactory messageFactory,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _messageRepository = messageRepository;
        _messageFactory = messageFactory;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MessageViewModel>> GetConversationAsync(int propertyId, string clientId, string agentId)
    {
        var messages = await _messageRepository.GetConversationAsync(propertyId, clientId, agentId);
        return _mapper.Map<IEnumerable<MessageViewModel>>(messages);
    }

    public async Task<IEnumerable<string>> GetClientIdsWithConversationAsync(int propertyId)
        => await _messageRepository.GetClientIdsWithConversationAsync(propertyId);

    public async Task<MessageViewModel> SendAsync(int propertyId, string clientId, string agentId, string senderId, string content)
    {
        var message = _messageFactory.Create(propertyId, clientId, agentId, senderId, content);

        await _messageRepository.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MessageViewModel>(message);
    }
}