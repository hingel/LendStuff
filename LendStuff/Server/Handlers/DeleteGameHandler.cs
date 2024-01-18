using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Services;
using LendStuff.Server.Commands;
using LendStuff.Shared;
using MediatR;

namespace LendStuff.Server.Handlers;

public class DeleteGameHandler : IRequestHandler<DeleteBoardGameCommand, ServiceResponse<string>>
{
	private readonly UnitOfWork _unitOfWork;

	public DeleteGameHandler(BoardGameRepository repository, UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
	public async Task<ServiceResponse<string>> Handle(DeleteBoardGameCommand request, CancellationToken cancellationToken)
	{
		var result = await _unitOfWork.BoardGameRepository.Delete(request.BoardGameId);
		var saveResult = await _unitOfWork.SaveChanges();

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}
}