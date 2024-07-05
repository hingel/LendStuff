using BoardGame.DataAccess.Repository;
using LendStuff.Shared.Messages;
using MassTransit;

namespace BoardGame.API.Consumers;

public class SetBoardGameAvailability(IBoardGameRepository repository) : IConsumer<ChangeBoardGameAvailability>
{
    public async Task Consume(ConsumeContext<ChangeBoardGameAvailability> context)
    {
        var boardGame = await repository.GetById(context.Message.BoardGameId);

        if (boardGame == null) return; //TODO: Borde kasta exception eller liknande, eller publicera meddelande om aborted.

        if (!boardGame.Available)
        {
            boardGame.Available = false;
        }

        await repository.SaveChanges();

        //TODO: Publicera meddelande om deleted
    }
}