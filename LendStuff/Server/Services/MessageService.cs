using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LendStuff.Server.Services;

public class MessageService
{
	private readonly IRepository<InternalMessage> _messageRepository;
	private readonly UserManager<ApplicationUser> _userManager;

	public MessageService(IRepository<InternalMessage> messageRepository, UserManager<ApplicationUser> userManager)
	{
		_messageRepository = messageRepository;
		_userManager = userManager;
	}

	public async Task<ServiceResponse<string>> AddMessage(MessageDto newMessageDto)
	{
		var result = await _messageRepository.FindByKey(m => m.MessageId == newMessageDto.MessageId && m.SentToUser.UserName == newMessageDto.SentToUserName);

		if (result.Any())
		{
			await _messageRepository.Update(await ConvertDtoToMessage(newMessageDto));

			return new ServiceResponse<string>()
			{
				Message = "Message sent.",
				Success = false
			};
		}

		var resultPerson = await _userManager.FindByNameAsync(newMessageDto.SentToUserName);
		if (resultPerson is not null)
		{
			var newAddedMessage = await _messageRepository.AddItem(await ConvertDtoToMessage(newMessageDto)); //TODO: Ordna ett svarsmeddelande

			return new ServiceResponse<string>()
			{
				Message = "Message sent.",
				Success = true
			};
		}

		return new ServiceResponse<string>()
		{
			Message = "Message not sent. No user found.",
			Success = true
		};
	}

	public async Task<ServiceResponse<IEnumerable<MessageDto>>> GetUserMessages(string userName)
	{
		var user = await _userManager.FindByNameAsync(userName);

		var result = await _messageRepository
			.FindByKey(m => m.SentFromUserName == user.UserName || m.SentToUser.Id == user.Id); //TODO: detta kan göras bättre.

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
		var result = await _messageRepository.Update(await ConvertDtoToMessage(updatedMessageDto));

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
			SentFromUserName = message.SentFromUserName,
			SentToUserName = message.SentToUser.UserName,
			IsRead = message.IsRead
		};
	}

	private async Task<InternalMessage> ConvertDtoToMessage(MessageDto messageDto)
	{
		return new InternalMessage()
		{
			MessageId = messageDto.MessageId, // == null ? 0 : messageDto.MessageId,
			Message = messageDto.Message,
			SentFromUserName = messageDto.SentFromUserName,
			SentToUser = (await _userManager.FindByNameAsync(messageDto.SentToUserName))!,
			IsRead = messageDto.IsRead
		};
	}
}