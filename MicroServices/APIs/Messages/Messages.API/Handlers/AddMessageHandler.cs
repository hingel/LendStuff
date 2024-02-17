using LendStuff.Shared;
using MediatR;
using Messages.API.CommandsAndQueries;
using Messages.API.Helpers;
using Messages.DataAccess.Models;

namespace Messages.API.Handlers;

public class AddMessageHandler(IRepository<InternalMessage> repository) : IRequestHandler<AddMessageCommand, ServiceResponse<string>>
{
	public async Task<ServiceResponse<string>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
	{
		var result = await repository.FindByKey(m => m.Id == request.NewMessageDto.MessageId && m.Id == request.NewMessageDto.SentToUserId);

		if (result.Any())
		{
			await repository.Update(MessageDtoConverter.ConvertDtoToMessage(request.NewMessageDto));

			return new ServiceResponse<string>()
			{
				Message = "Message sent.",
				Success = false
			};
		}

		//TODO:
		//måste kolla med UserManager APIet:
		//Göra anrop till interna apiet.
		//var resultPerson = await _userManager.FindByNameAsync(newMessageDto.SentToUserName);
		//if (resultPerson is not null)
		//{
		//	var newAddedMessage = await repository.AddItem(MessageDtoConverter.ConvertDtoToMessage(request.NewMessageDto));

		//	return new ServiceResponse<string>()
		//	{
		//		Message = "Message sent.",
		//		Success = true
		//	};
		//}

		return new ServiceResponse<string>()
		{
			Message = "Message not sent. No user found.",
			Success = true
		};
	}
}