using LendStuff.Shared.Messages;
using MassTransit;
using Messages.DataAccess.Repositories;

namespace Messages.API.Consumers;

public class RemoveUserMessagesConsumer(IMessageRepository repository) : IConsumer<DeleteMessages>
{
    public async Task Consume(ConsumeContext<DeleteMessages> context)
    {
        var messages = await repository.GetByUserId(context.Message.UserId);

        foreach (var message in messages)
        {
            if (message.SentFromUserId == context.Message.UserId)
            {
                message.SentFromUserId = Guid.Empty;
            }
            else
            {
                message.SentToUserId = Guid.Empty;
            }
        }

        await repository.SaveChanges();

       // await bus.Publish(new UserReferenceRemovedMessage(new Guid(context.Message.UserId)));
    }
}