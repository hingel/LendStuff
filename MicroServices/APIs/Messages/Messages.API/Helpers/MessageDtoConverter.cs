using LendStuff.Shared.DTOs;
using Messages.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace Messages.API.Helpers;

public static class MessageDtoConverter
{
	public static MessageDto ConvertMessageToDto(InternalMessage message)
	{
		return new MessageDto()
		{
			MessageId = message.Id,
			Message = message.Message,
			MessageSent = message.MessageSent,
			SentFromUserId = message.SentFromUserId,
			SentToUserId = message.SentToUserId,
			IsRead = message.IsRead
		};
	}

	public static InternalMessage ConvertDtoToMessage(MessageDto messageDto)
	{
		return new InternalMessage()
		{
			Id = messageDto.MessageId, // == null ? 0 : messageDto.MessageId,
			Message = messageDto.Message,
			SentFromUserId = messageDto.SentFromUserId,
			SentToUserId = messageDto.SentToUserId,
			IsRead = messageDto.IsRead
		};
	}
}