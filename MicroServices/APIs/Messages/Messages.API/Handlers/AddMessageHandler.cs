using LendStuff.Shared;
using MediatR;
using Messages.API.CommandsAndQueries;
using Messages.DataAccess.Models;
using Messages.DataAccess.Repositories;

namespace Messages.API.Handlers;

public class AddMessageHandler(IRepository<InternalMessage> repository) : IRequestHandler<AddMessageCommand, ServiceResponse<string>>
{
	public Task<ServiceResponse<string>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
	{
		var result = await repository.FindByKey(m => m.Id == request.NewMessageDto.MessageId && m.Id == request.NewMessageDto.SentToUserId);

		if (result.Any())
		{
			await repository.Update(await ConvertDtoToMessage(newMessageDto));

			return new ServiceResponse<string>()
			{
				Message = "Message sent.",
				Success = false
			};
		}

		var resultPerson = await _userManager.FindByNameAsync(newMessageDto.SentToUserName);
		if (resultPerson is not null)
		{
			var newAddedMessage = await repository.AddItem(await ConvertDtoToMessage(newMessageDto)); //TODO: Ordna ett svarsmeddelande

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
}