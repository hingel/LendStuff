using LendStuff.DataAccess.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LendStuff.Server.Services;

public class OrderService
{


	//Funktion 1:
	//private async Task<List<InternalMessage>> FindMessages(List<MessageDto> messageDtosToFind)
	//{
	//	var listWithMessages = new List<InternalMessage>();

	//	foreach (var messageDto in messageDtosToFind)
	//	{
	//		var foundMessage = (await _messageRepository.FindByKey(m => m.MessageId == messageDto.MessageId)).FirstOrDefault();

	//		if (foundMessage is null)
	//		{
	//			var newAddedMessage = await _messageRepository.AddItem(new InternalMessage()
	//			{
	//				Message = messageDto.Message,
	//				IsRead = messageDto.IsRead,
	//				MessageSent = messageDto.MessageSent,
	//				SentFromUserName = messageDto.SentFromUserName,
	//				SentToUser = (await _userManager.FindByNameAsync(messageDto.SentToUserName))!
	//			});

	//			listWithMessages.Add(newAddedMessage);
	//		}

	//		else
	//		{
	//			listWithMessages.Add(foundMessage);
	//		}
	//	}

	//	return listWithMessages;
	//}

	////Funktion 2:
	//private async Task<InternalMessage> FindOneMessage(MessageDto messageToFind)
	//{
	//	var internalMessage = (await _messageRepository.FindByKey(m => m.MessageId == messageToFind.MessageId))
	//		.FirstOrDefault();

	//	if (internalMessage is not null) 
	//		return internalMessage;

	//	var newMessage = await _messageRepository.AddItem(new InternalMessage()
	//	{
	//		Message = messageToFind.Message,
	//		IsRead = messageToFind.IsRead,
	//		MessageSent = messageToFind.MessageSent,
	//		SentFromUserName = messageToFind.SentFromUserName,
	//		SentToUser = (await _userManager.FindByNameAsync(messageToFind.SentToUserName))!
	//	});
		
	//	return newMessage;
	//}
}