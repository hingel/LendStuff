using BoardGame.API.CommandsAndQueries;
using BoardGame.API.Helpers;
using LendStuff.Shared;
using MediatR;

namespace BoardGame.API.Handlers;

public class DeleteGameHandler : IRequestHandler<DeleteBoardGameCommand, ServiceResponse<string>>
{
	private readonly UnitOfWork _unitOfWork;

	public DeleteGameHandler(IRepository<DataAccess.Models.BoardGame> repository, UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
	public async Task<ServiceResponse<string>> Handle(DeleteBoardGameCommand request, CancellationToken cancellationToken)
	{
		var result = await _unitOfWork.BoardGameRepository.Delete(request.Id);
		var saveResult = await _unitOfWork.SaveChanges();

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}
}