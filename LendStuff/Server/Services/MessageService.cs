using Duende.IdentityServer.Models;
using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;

namespace LendStuff.DataAccess.Services;

public class MessageService
{
	private readonly IRepository<InternalMessage> _messageRepository;

	public MessageService(IRepository<InternalMessage> messageRepository)
	{
		_messageRepository = messageRepository;
	}

	public async Task<ServiceResponse<string>> AddMessage(MessageDto newMessageDto)
	{
		await _messageRepository.AddItem(ConvertDtoToMessage(newMessageDto));

		return new ServiceResponse<string>()
		{
			Message = "Message sent.",
			Success = true
		};
	}

	public async Task<ServiceResponse<IEnumerable<MessageDto>>> GetUserMessages(string userId)
	{
		//TODO: Hur kolla att det är den aktuella användaren som skickar in userId?  
		var result = await _messageRepository.FindByKey(m => m.SentFromUserGuid == userId || m.SentToUserGuid == userId);

		if (result.Count() > 0)
		{
			return new ServiceResponse<IEnumerable<MessageDto>>()
			{
				Data = result.Select(ConvertMessageToDto),
				Message = "Messages found",
				Success = true
			};
		}

		return new ServiceResponse<IEnumerable<MessageDto>>()
		{
			Message = "No messages found",
			Success = false
		};
	}

	public async Task<ServiceResponse<string>> DeleteMessage(int id)
	{
		var result = await _messageRepository.Delete(id.ToString());

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}

	public async Task<ServiceResponse<MessageDto>> UpdateMessage(MessageDto updatedMessageDto)
	{
		var result = await _messageRepository.Update(ConvertDtoToMessage(updatedMessageDto));

		if (result is null)
		{
			return new ServiceResponse<MessageDto>()
			{
				Message = "Message not updated",
				Success = false
			};
		}

		return new ServiceResponse<MessageDto>()
		{
			Data = ConvertMessageToDto(result),
			Message = "Message updated",
			Success = true
		};
	}

	private static MessageDto ConvertMessageToDto(InternalMessage message)
	{
		return new MessageDto()
		{
			MessageId = message.MessageId,
			Message = message.Message,
			MessageSent = message.MessageSent,
			SentFromUserGuid = message.SentFromUserGuid,
			SentToUserGuid = message.SentToUserGuid
		};
	}

	private static InternalMessage ConvertDtoToMessage(MessageDto messageDto)
	{
		return new InternalMessage()
		{
			MessageId = messageDto.MessageId, // == null ? 0 : messageDto.MessageId,
			Message = messageDto.Message,
			SentFromUserGuid = messageDto.SentFromUserGuid,
			SentToUserGuid = messageDto.SentToUserGuid
		};
	}
}