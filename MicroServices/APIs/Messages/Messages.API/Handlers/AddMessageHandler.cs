using LendStuff.Shared;
using MediatR;
using Messages.API.CommandsAndQueries;
using Messages.API.Helpers;
using Messages.DataAccess.Repositories;

namespace Messages.API.Handlers;

public class AddMessageHandler(IMessageRepository repository)
    : IRequestHandler<AddMessageCommand, ServiceResponse<string>>
{
    public async Task<ServiceResponse<string>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
	{
		//TODO: Detta måste uppdateras
		//Först kolla om meddelande redan finns: Leta upp meddelande från användaren och mottagare och sortera efter skapat. Kolla om texten i meddelandet är det samma.
		//Sen kolla om användaren som ska skickas till finns
		//Annars spara ner meddelandet

        var result = await repository.GetById(request.NewMessageDto.MessageId);

		if (result != null && result.Id == request.NewMessageDto.SentToUserId)
		{
			await repository.Update(MessageDtoConverter.ConvertDtoToMessage(request.NewMessageDto));

			return new ServiceResponse<string>()
			{
				Message = "Message sent.",
				Success = false
			};
		}

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