using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using MediatR;
using Messages.API.CommandsAndQueries;
using Messages.API.Helpers;
using Messages.DataAccess.Models;

namespace Messages.API.Handlers;

public class UpdateMessageHandler(IRepository<InternalMessage> repository) : IRequestHandler<UpdateMessageCommand, ServiceResponse<MessageDto>>
{
	public async Task<ServiceResponse<MessageDto>> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
	{
		var result = await repository.Update(MessageDtoConverter.ConvertDtoToMessage(request.MessageToUpdateDto));

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
			Data = MessageDtoConverter.ConvertMessageToDto(result),
			Message = "Message updated",
			Success = true
		};
	}
}