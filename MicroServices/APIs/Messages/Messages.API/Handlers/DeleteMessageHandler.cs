using LendStuff.Shared;
using MediatR;
using Messages.API.CommandsAndQueries;
using Messages.DataAccess.Repositories;

namespace Messages.API.Handlers;

public class DeleteMessageHandler(IMessageRepository repository) : IRequestHandler<DeleteMessageCommand, ServiceResponse<string>>
{
	public async Task<ServiceResponse<string>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
	{
		var result = await repository.Delete(request.MessageId);

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}
}