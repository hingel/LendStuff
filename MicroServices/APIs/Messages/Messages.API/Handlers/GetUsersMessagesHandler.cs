using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;
using Messages.API.CommandsAndQueries;
using Messages.API.Helpers;
using Messages.DataAccess.Repositories;

namespace Messages.API.Handlers;

public class GetUsersMessagesHandler(IMessageRepository repository) : IRequestHandler<GetUsersMessages, ServiceResponse<IEnumerable<MessageDto>>>
{
	public async Task<ServiceResponse<IEnumerable<MessageDto>>> Handle(GetUsersMessages request, CancellationToken cancellationToken)
	{
		//TODO: Kan kolla att det är rätt person inloggad på något sätt för att hämta meddelanden.

		var result = await repository.GetByUserId(request.UserId);
		
		if (result.Any())
		{
			return new ServiceResponse<IEnumerable<MessageDto>>
			{
				Data = result.Select(MessageDtoConverter.ConvertMessageToDto),
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
}